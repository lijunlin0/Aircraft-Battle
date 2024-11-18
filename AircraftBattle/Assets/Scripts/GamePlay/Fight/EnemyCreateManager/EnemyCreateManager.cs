using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum EnemyCreateChance
{
    Enemy1=40,
    Enemy3=30,
    Enemy2=10,
}

public class EnemyCreateManager
{
    private float mWaveEnemyCreateTime=3f;
    private float mEnemyCreateTime=0.8f;
    private List<Character> mEnemies;
    private List<List<CharacterId>> mPendingEnemy;
    private bool mIsEnemyWaveCreate=true;
    private int mCurrentWaveIndex=0;
    //敌人生成概率表
    public EnemyCreateManager()
    {
        mEnemies=FightModel.GetCurrent().GetEnemies();
        mPendingEnemy=new List<List<CharacterId>>();
        Level1EnemyListInit();
    }

    private Vector3 GetEnemyValidPosition()
    {
        int offset=45;
        //随机一个位置
        float x=0;
        float y=+Utility.WindowHeight/2-offset;
        x=RandomHelper.RandomInt(-Utility.WindowWidth/2+offset,Utility.WindowWidth/2-offset);
        return new Vector3(x,y,(int)DisplayLayer.Character);
    }
    private Character EnemyCreate(CharacterId characterId)
    {
        //生成敌人
        Character enemy = CharacterIdToEnemy(characterId,GetEnemyValidPosition());
        return enemy;
    }
    private Character CharacterIdToEnemy(CharacterId id,Vector3 position)
    {
        switch(id)
        {
            case CharacterId.Enemy1:return Enemy1.Create(position);
            case CharacterId.Enemy2:return Enemy2.Create(position);
            case CharacterId.Enemy3:return Enemy3.Create(position);
            default:return null;
        }
    }

    private void AddEnemyIdList(CharacterId characterId,int num)
    {
        List<CharacterId> idList=new List<CharacterId>();
        for(int i=0;i<num;i++)
        {
            idList.Add(characterId);
        }
        mPendingEnemy.Add(idList);
    }

    public  void Level1EnemyListInit()
    {
        mPendingEnemy.Clear();
        List<Tuple<CharacterId,int>> enemyList=new List<Tuple<CharacterId, int>>()
        {
            new Tuple<CharacterId, int>(CharacterId.Enemy1, 15),
            new Tuple<CharacterId, int>(CharacterId.Enemy2, 15),
            new Tuple<CharacterId, int>(CharacterId.Enemy3, 15),
            new Tuple<CharacterId, int>(CharacterId.Enemy2, 30),
            new Tuple<CharacterId, int>(CharacterId.Enemy1, 20),
            new Tuple<CharacterId, int>(CharacterId.Enemy3, 15),
        };
        foreach(Tuple<CharacterId,int> pair in enemyList)
        {
            AddEnemyIdList(pair.Item1,pair.Item2);
        }
    }

    public void CreateEnemyWave()
    {
        int count=mPendingEnemy[mCurrentWaveIndex].Count;
        for(int i=0;i<count;i++)
        {
            //保存当前变量供延迟函数用
            int index=i;
            int currentWaveIndex=mCurrentWaveIndex;
            DOVirtual.DelayedCall(i*mEnemyCreateTime,()=>
            { 
                CharacterId characterId=mPendingEnemy[currentWaveIndex][index];
                Character enemy=EnemyCreate(characterId);
                mEnemies.Add(enemy);
                if(index==count-1)
                {
                    mIsEnemyWaveCreate=true;
                }
            },false);
        }
        mCurrentWaveIndex++;
    }


    public void OnUpdate()
    {
        //场上还有敌人或当前这波敌人没生成完成则不生成下一波
        if(mEnemies.Count!=0||!mIsEnemyWaveCreate)
        {
            return;
        }
        if(mCurrentWaveIndex<mPendingEnemy.Count)
        {
            mIsEnemyWaveCreate=false;
            DOVirtual.DelayedCall(mWaveEnemyCreateTime,()=>
            {
                CreateEnemyWave();
            },false);
        }
        else
        {
            FightManager.GetCurrent().GameOver(true);
        }
    }

    
}