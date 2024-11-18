using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;

public enum CharacterId
{
    Player,
    Enemy1,
    Enemy2,
    Enemy3,
}

public class Character : FightObject
{
    protected float mDestroyAnimationDuration=0.5f;
    private CharacterId mCharacterId;
    protected int mHealth;
    protected int mMaxHealth;
    protected int mMoveSpeed;
    private float mGasDefaultTime=0;
    private const float GasCreateTime=0.1f;
    private const int GasOffsetY=-60;

    protected HealthBar mHealthBar;
    protected BulletShooter mShooter;
    protected float mShootTime;
    protected int mAttack;
    protected SpriteRenderer mSpriteRenderer;
    protected Vector3 mPrePosition;
    protected Animator mAnimator;
    protected int mLevel;
    protected virtual void Init(CharacterId characterId)
    {
        base.Init();
        mCharacterId=characterId;
        mMaxHealth=100;
        mHealth=mMaxHealth;
        mShootTime=3;
        mAttack=10;
        mMoveSpeed=500;
        mAnimator=mDisplay.GetComponent<Animator>();
        mAnimator.enabled=false;
        mSpriteRenderer=mDisplay.GetComponent<SpriteRenderer>();
        mHealthBar=HealthBar.Create(this);
    }
    //被攻击
    protected virtual void OnDamage()
    {
        
    }

    protected virtual void Shoot()
    {

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        mShooter.OnUpdate();
        mGasDefaultTime+=Time.deltaTime;
        if(mGasDefaultTime>=GasCreateTime)
        {
            int offsetY=IsEnemy()?-GasOffsetY:GasOffsetY;
            Gas.Create(transform.position+new Vector3(0,offsetY,0),false);
            mGasDefaultTime=0;
        }
    }

    public int GetHealth(){return mHealth;}
    public int GetMaxHealth(){return mMaxHealth;}
    public CharacterId GetCharacterId(){return mCharacterId;}
    public void SetDead()
    {
        mIsDead=true;
    }
    public virtual void GetDamage(int points)
    {
        if(mIsDead)
        {
            return;
        }
        mHealthBar.OnHealthChanged();
        AudioManager.GetCurrent().PlayCharacterHurtSound();
        mHealth=Mathf.Clamp(mHealth-points,0,mMaxHealth);
        if(mHealth<=0)
        {
            mIsDead=true;
            AudioManager.GetCurrent().PlayCharacterExplosionSound();
            if(IsEnemy())
            {
                Player.GetCurrent().AddScore(1);
            }
        }
        Color color=mSpriteRenderer.color;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(mSpriteRenderer.material.DOColor(Color.red,0.1f));
        sequence.Append(mSpriteRenderer.material.DOColor(color,0.1f));
        sequence.Play();
    }

    public override void PlayDestroyAnimation()
    {
        mAnimator.enabled=true;
        DOVirtual.DelayedCall(mDestroyAnimationDuration,()=>
        {
            DOTween.Kill(gameObject);
            Destroy(gameObject);
        });

    }

    public Vector3 GetPrePosition(){return mPrePosition;}

    public int GetAttack(){return mAttack;}
    public int GetMoveSpeed(){return mMoveSpeed;}
    public bool IsEnemy(){return mCharacterId!=CharacterId.Player;}
    public bool IsPlayer(){return mCharacterId==CharacterId.Player;}

    protected List<Vector3> GetBulletCirclePositionList(Vector3 center,int bulletCount,int r)
    {
        float angleStep = 360f/bulletCount;
        List<Vector3> positionList=new List<Vector3>();
         for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 enemyPosition = CalculatePosition(center,angle,r);
            positionList.Add(enemyPosition);
        }
        return positionList;
    }
    private Vector3 CalculatePosition(Vector3 center,float angle,int r)
    {
        float radian=angle*Mathf.Deg2Rad;
        float x = center.x + r * Mathf.Cos(radian);
        float y = center.y + r * Mathf.Sin(radian);
        return new Vector3(x, y,-1);
    }
}