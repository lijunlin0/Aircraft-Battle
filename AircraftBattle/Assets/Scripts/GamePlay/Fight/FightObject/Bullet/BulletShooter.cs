using DG.Tweening;
using UnityEngine;

public class BulletShooter
{
    private Callback mShootCallback;
    private float mShootTime;
    private float mDefaultTime=0;

    
    public BulletShooter(Callback callback,float shootTime)
    {
        mShootCallback = callback;
        mShootTime = shootTime;
    }

    public void OnUpdate()
    {
        //攻击
        if (mDefaultTime>=mShootTime)
        {
            mShootCallback();
            mDefaultTime=0;
        }
        mDefaultTime+=Time.deltaTime;

    }

    public void SetShootSpeedFactor(float factor)
    {
        mShootTime/=factor;
    }
}