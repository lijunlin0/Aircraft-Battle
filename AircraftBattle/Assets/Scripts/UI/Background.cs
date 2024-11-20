using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Background : MonoBehaviour
{
    private int MaxMoveDistance=Utility.WindowHeight;
    private int mMoveSpeed=300;
    private float mMoveDistance=0;
    public  static Background Create()
    {
        Canvas canvas=GameObject.Find("BackgroundCanvas").GetComponent<Canvas>();
        GameObject prefab=Resources.Load<GameObject>("UI/Background");
        GameObject gameObject=Instantiate(prefab,canvas.transform);
        Background background=gameObject.AddComponent<Background>();
        background.Init();
        return background;
    }

    private void Init()
    {
        RectTransform[] childTransforms = transform.GetComponentsInChildren<RectTransform>();
        for(int i=1;i<childTransforms.Count();i++)
        {
            childTransforms[i].sizeDelta=new Vector2(Utility.WindowWidth,Utility.WindowHeight);
            childTransforms[i].position=new Vector3(0,(i-1)*Utility.WindowHeight,(int)DisplayLayer.Background);
        }
    }


    public void Update()
    {
        if(FightManager.GetCurrent().IsGameOver())
        {
            return;
        }
        transform.position+=Vector3.down*mMoveSpeed*Time.deltaTime;
        mMoveDistance += mMoveSpeed * Time.deltaTime;
        if(mMoveDistance>=MaxMoveDistance)
        {
            transform.position+=new Vector3(0,MaxMoveDistance,0);
            mMoveDistance=0;
        }
    }
}