using DG.Tweening;
using UnityEngine;

public class Enemy:Character
{
    protected bool mHaveInto=false;
    protected void IsBorder()
    {
        if(!mHaveInto)
        {
            return;
        }
        int widthHalf=Utility.WindowWidth/2;
        int heightHalf=Utility.WindowHeight/2;
        int offset=45;
        if(transform.position.x<-widthHalf+offset)
        {
            mIsDead=true;
        }
        if(transform.position.y<-heightHalf-offset)
        {
            mIsDead=true;
        }
        if(transform.position.x>widthHalf-offset)
        {
           mIsDead=true;
        }
    }

    //进入屏幕判断
    protected void Border()
    {
        int offset=45;
        int widthHalf=Utility.WindowWidth/2;
        int heightHalf=Utility.WindowHeight/2;
        if(mHaveInto)
        {
            IsBorder();
        }
        else
        {
            if(transform.position.x>-widthHalf+offset&&transform.position.x<widthHalf-offset)
            {
                mHaveInto=true;
            }
            if(transform.position.y>-heightHalf+offset&&transform.position.y<heightHalf-offset)
            {
                mHaveInto=true;
            }
        }
        
    }

    public override void PlayDestroyAnimation()
    {
        mAnimator.enabled=true;
        DOVirtual.DelayedCall(mDestroyAnimationDuration,()=>
        {
            gameObject.SetActive(false);
        });
        DOVirtual.DelayedCall(3,()=>
        {
            DOTween.Kill(gameObject);
            if(mIsPoolObject)
            {
                // 重置
                Color color = mSpriteRenderer.color;
                color.a = 1;
                mSpriteRenderer.color = color;
                mCollider.GetCollider().enabled=true;
                mAnimator.enabled=false;
                FightManager.GetCurrent().GetPoolManager().PutGameObject(gameObject);
                Destroy(this);
            }
            else
            {
                Destroy(gameObject,3);
            }
            Destroy(gameObject);
        });
    }

    

    protected virtual void Move()
    {
        //不跟踪
        FightUtility.Move(gameObject,mMoveSpeed);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Border();
        Move();
    }
}