using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Player : Character
{    
    public const int MaxEnergy=100;
    private const int EnergyDuration=7;
    //狂暴状态飞机移动动画时间
    private const float EnergyMoveAnimationDuration=1;
    private const float EnergyAttackSpeedFactor=1.5f;
    private const int EnergyAttackFactor=2;
    private bool mIsOnUseEnergy=false;    //能量状态动画时会限制移动
    private bool mCanMove=true;
    private int mScore=0;
    public static Vector3 BasePosition=new Vector3(0,-300,(int)DisplayLayer.Character);
    private static Player sCurrent;
    protected const float CollideProtect=2;
    //碰撞保护闪烁次数
    protected const int CollideProtectFickerCount=6;
    protected int mEnergy;
    protected float mCollideProtect=0;
    protected bool mShootflickerFlag=true;
    protected GameObject mAttackRangeArea=null;
    protected GameObject mEnergyDisplay;
    protected SpriteRenderer mEnergySpriteRender;
    protected bool mIsMove=false;
    protected override void Init()
    {
        base.Init(CharacterId.Player);
        mMoveSpeed=1000;
        mEnergy=0;
        mShootTime=0.3f;
        mAttack=10;
        sCurrent=this;
        mMaxHealth=400;
        mHealth=mMaxHealth;
        mEnergyDisplay=transform.Find("Display/EnergyDisplay").gameObject;
        mEnergyDisplay.SetActive(false);
        mEnergySpriteRender=mEnergyDisplay.GetComponent<SpriteRenderer>();
        //添加子弹发射器
        mShooter = new BulletShooter(()=>
        {
            Shoot();
        },mShootTime);
    }

    public static Player Create()
    {
        GameObject playerPrefab=Resources.Load<GameObject>("FightObject/Character/Player");
        GameObject playerObject=Instantiate(playerPrefab,BasePosition,playerPrefab.transform.rotation);
        Player player=playerObject.AddComponent<Player>();
        player.Init();
        return player;
    }

    public override void GetDamage(int points)
    {
        if(mIsDead)
        {
            return;
        }
        if(InCollideProtect())
        {
            return;
        }
        AudioManager.GetCurrent().PlayCharacterHurtSound();
        mHealth=Mathf.Clamp(mHealth-points,0,mMaxHealth);
        mHealthBar.OnHealthChanged();
        SetCollideProtect();
        if(mHealth<=0)
        {
            mIsDead=true;
            FightManager.GetCurrent().GameOver(false);
            PlayDestroyAnimation();
            AudioManager.GetCurrent().PlayCharacterExplosionSound();
        }
        Sequence sequence1 = DOTween.Sequence();
        Sequence sequence2 = DOTween.Sequence();
        float fickerTime=CollideProtect/CollideProtectFickerCount;
        sequence1.Append(mSpriteRenderer.DOFade(0,fickerTime/2));
        sequence1.Append(mSpriteRenderer.DOFade(1,fickerTime/2));
        sequence2.Append(mEnergySpriteRender.DOFade(0,fickerTime/2));
        sequence2.Append(mEnergySpriteRender.DOFade(1,fickerTime/2));
        sequence1.SetLoops(CollideProtectFickerCount,LoopType.Restart);
        sequence2.SetLoops(CollideProtectFickerCount,LoopType.Restart);
    }

    private void IsBorder()
    {
        int widthHalf=Utility.WindowWidth/2;
        int heightHalf=Utility.WindowHeight/2;
        int offset=45;
        if(transform.position.x<-widthHalf+offset)
        {
            transform.position=new Vector3(-widthHalf+offset,transform.position.y,transform.position.z);
        }
        if(transform.position.y<-heightHalf+offset)
        {
            transform.position=new Vector3(transform.position.x,-heightHalf+offset,transform.position.z);
        }
        if(transform.position.x>widthHalf-offset)
        {
           transform.position=new Vector3(widthHalf-offset,transform.position.y,transform.position.z);
        }
        if(transform.position.y>heightHalf-offset)
        {
            transform.position=new Vector3(transform.position.x,heightHalf-offset,transform.position.z);
        }
    }
    public void Move(Vector3 direction)
    {
        if(!mCanMove)
        {
            return;
        }
        IsBorder();
        mPrePosition=transform.position;
        Vector3 prePosition=transform.position;
        transform.Translate(direction*mMoveSpeed*Time.deltaTime);
        //IsBorder();
        if(prePosition==transform.position)
        {
            return;
        }
    }

    protected override void Shoot()
    {
        PlayerBullet bullet=PlayerBullet.Create(mAttack,mIsOnUseEnergy);
        FightModel.GetCurrent().AddPlayerBullet(bullet);
    }

    public override void PlayDestroyAnimation()
    {
        Destroy(mHealthBar);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(mCollideProtect>=0)
        {
            mCollideProtect-=Time.deltaTime;
        }
        mShooter.OnUpdate();
    }

    protected override void OnDamage()
    {
        base.OnDamage();
    }

    public static Player GetCurrent()
    {
        return sCurrent;
    }
    public void SetCollideProtect(){mCollideProtect=CollideProtect;}
    public bool InCollideProtect(){return mCollideProtect>=0;}
    public int GetLevel()
    {
        return mLevel;
    }
    public void AddEnergy(int points)
    {
        if(mIsOnUseEnergy)
        {
            return;
        }
        mEnergy+=points;
        mEnergy=Mathf.Clamp(mEnergy,0,MaxEnergy);
        MainScene.GetCurrent().GetEnergyBar().OnEnergyChanged();
        //攻速增加
        if(mEnergy==MaxEnergy)
        {
            OnEnergy();
        }
    }

    private void OnEnergy()
    {
        float energyDuration=EnergyDuration+EnergyMoveAnimationDuration;
        SwitchDisplay(()=>
        {
            mShooter.SetShootSpeedFactor(EnergyAttackSpeedFactor);
            mAttack*=EnergyAttackFactor;
            MainScene.GetCurrent().GetEnergyBar().UseEnergy(energyDuration);
            mIsOnUseEnergy=true;
            mEnergyDisplay.SetActive(true);
        });
        DOVirtual.DelayedCall(energyDuration,()=>
        {
            SwitchDisplay(()=>
            {
                mIsOnUseEnergy=false;
                mShooter.SetShootSpeedFactor(1/EnergyAttackSpeedFactor);
                mAttack/=EnergyAttackFactor;
                mEnergyDisplay.SetActive(false);
                mEnergy=0;
            });

        });
        
    }

    private void SwitchDisplay(Callback startCallback)
    {
        mCanMove=false;
        Sequence sequence=DOTween.Sequence();
        //向左
        sequence.Append(transform.DOLocalMoveX(-1500,EnergyMoveAnimationDuration/2));
        sequence.AppendCallback(() =>
        {
            transform.position = new Vector3(1500, transform.position.y, transform.position.z);
            startCallback();
        });
         
        //向右
        sequence.Append(transform.DOLocalMoveX(0,EnergyMoveAnimationDuration/2));
        sequence.AppendCallback(()=>
        {
            mCanMove=true;
        });
    }
    public int GetEnergy(){return mEnergy;}
    public int GetScore(){return mScore;}
    public void AddScore(int points)
    {
        mScore+=points;
        MainScene.GetCurrent().GetScoreText().OnScoreChanged();
    }

}
