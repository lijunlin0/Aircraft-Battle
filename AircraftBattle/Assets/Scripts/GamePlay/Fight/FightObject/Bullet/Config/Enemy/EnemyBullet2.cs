using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

//敌人跟踪子弹
public class EnemyBullet2 : Bullet
{
    private float mGasDefaultTime=0;
    private const float GasCreateTime=0.1f;
    private bool mNaturalDead=false;
    private Rigidbody2D mRigidbody;
    public static EnemyBullet2 Create(Character enemy,int points,int angle=0)
    {
        GameObject bulletObject=FightManager.GetCurrent().GetPoolManager().GetGameObject("Bullet/"+enemy.GetCharacterId().ToString()+"Bullet");
        bulletObject.transform.position=enemy.transform.position;
        Quaternion newRotation = bulletObject.transform.rotation * Quaternion.Euler(0, 0, angle);
        bulletObject.transform.rotation=newRotation;
        EnemyBullet2 bullet=bulletObject.AddComponent<EnemyBullet2>();
        bullet.Init(enemy,points);
        return bullet;
    }
    protected override void Init(Character character,int points)
    {
        base.Init(character,points);
        mRigidbody=mDisplay.GetComponent<Rigidbody2D>();
        Vector3 direction = (Player.GetCurrent().transform.position - transform.position).normalized;
        transform.rotation=FightUtility.DirectionToRotation(direction);
        mMoveSpeed=600;
        mMaxLifeTime=2;
    }
    public override void OnUpdate()
    {
        mCollider.OnUpdate();
        if(mNaturalDead||mIsDead)
        {
            return;
        }
        mGasDefaultTime+=Time.deltaTime;
        if(mGasDefaultTime>=GasCreateTime)
        {
            Gas.Create(transform.position,true);
            mGasDefaultTime=0;
        }
        mLiveTime+=Time.deltaTime;
        if(mLiveTime>=mMaxLifeTime)
        {
            mNaturalDead=true;
            PlayNaturalDestroyAnimation();
            return;
        }
        FightUtility.TrackMove(gameObject,Player.GetCurrent().gameObject.transform.position,mMoveSpeed);
    }

    public void PlayNaturalDestroyAnimation()
    {
        mCollider.SetEnable(false);
        mRigidbody.gravityScale = 150f;
        mRigidbody.velocity = new Vector2(0f, -10f);

        DOVirtual.DelayedCall(3,()=>
        {
            mIsDead=true;
        });
    }
}