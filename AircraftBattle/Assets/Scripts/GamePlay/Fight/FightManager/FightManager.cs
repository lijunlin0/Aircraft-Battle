using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager
{
    private static FightManager sCurrent;
    private bool mGameOver=false;
    protected FightModel mFightModel;
    private PoolManager mPoolManager;
    public static FightManager GetCurrent(){return sCurrent;}

    public FightManager()
    {
        Time.timeScale=1;
        sCurrent=this;
        mFightModel=new FightModel();
        mPoolManager=new PoolManager();
    }
    public void OnUpdate()
    {
        if(mGameOver)
        {
            return;
        }
        mFightModel.OnUpdate();
    }

    public void GameOver(bool isWin)
    {

        RedoButton.Create(isWin);
        Time.timeScale=0;
        mGameOver=true;
    }

    public bool IsGameOver(){return mGameOver;}
    public PoolManager GetPoolManager(){return mPoolManager;}
}
