using System.Collections.Generic;
using UnityEngine;

public class Bullet : FightObject
{    
    //子弹移动速度
    protected float mMoveSpeed=1500;
    protected double mLiveTime;
    protected double mMaxLifeTime;
    //子弹来源角色
    protected Character mSource;
    //子弹伤害
    protected int mPoints=1;
    //子弹能否穿透
    protected virtual void Init(Character source,int points)
    {
        base.Init();
        mCollider=new MyCollider(mDisplay.GetComponent<BoxCollider2D>());
        mSource = source;
        mPoints = points;
        mLiveTime=0;
        mMaxLifeTime=2;
        transform.position=new Vector3(transform.position.x,transform.position.y,(int)DisplayLayer.Bullet);
    }
    public override void OnUpdate()
    { 
        base.OnUpdate();
        mLiveTime+=Time.deltaTime;
        if(mLiveTime>=mMaxLifeTime)
        {
            mIsDead=true;
        }
    }

    //子弹碰撞角色造成伤害
    public virtual void OnCollideCharacter(Character target)
    {
        target.GetDamage(mPoints);
        mIsDead=true;
    }

    public Character GetSource()
    {
        return mSource;
    }

}
