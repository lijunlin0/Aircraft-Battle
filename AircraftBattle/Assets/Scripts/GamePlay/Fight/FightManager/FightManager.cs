using DG.Tweening;
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
        DOTween.PauseAll();
        DOTween.Play("ScoreText");
        MainScene.GetCurrent().GetScoreText().PlayEndAnimation();
        EndWindow.Create(isWin);
        mGameOver=true;
    }

    public bool IsGameOver(){return mGameOver;}
    public PoolManager GetPoolManager(){return mPoolManager;}
}
