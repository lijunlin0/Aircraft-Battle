using UnityEngine;

public class MyCollider
{
    private BoxCollider2D mCollider;
    private bool mEnable;
    private float mCreateTime;

    public MyCollider(BoxCollider2D collider)
    {
        mCreateTime=0;
        mCollider = collider;
        mEnable=false;
    }
    public void OnUpdate()
    {
        mCreateTime+= Time.deltaTime;
        if(mCreateTime>=0.05)
        {
            mEnable = true;
        }
    }

    public BoxCollider2D GetCollider(){return mCollider;}
    public bool IsEnable(){return mEnable;}
    public void SetEnable(bool enable){mEnable=enable;}
}