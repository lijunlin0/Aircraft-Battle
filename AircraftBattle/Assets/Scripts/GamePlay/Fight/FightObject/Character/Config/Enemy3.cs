using System.Collections.Generic;
using UnityEngine;
public class Enemy3 : Enemy
{
    private const int BulletCount=10;
    private const int BulletCircleradius=100;
    private const int BulletCenterOffsetY=-120;
    public static Enemy3 Create(Vector3 position)
    {
        GameObject enemyObject=FightManager.GetCurrent().GetPoolManager().GetGameObject("Character/Enemy3");
        enemyObject.transform.position=position;
        Enemy3 enemy=enemyObject.AddComponent<Enemy3>();
        enemy.Init();
        return enemy;
    }

    protected override void Init()
    {
        base.Init(CharacterId.Enemy3);
        mShootTime=2;
        mMoveSpeed=350;
        mAttack=10;
        mMaxHealth=40;
        mHealth=mMaxHealth;
        mShooter = new BulletShooter(()=>
        {
            Shoot();
        },mShootTime);
    }
    

    protected override void Shoot()
    {
        List<Vector3> positionList=GetBulletCirclePositionList(transform.position+new Vector3(0,BulletCenterOffsetY,0),BulletCount,BulletCircleradius);
        foreach(var position in positionList)
        {
            EnemyBullet bullet=EnemyBullet.Create(this,mAttack,0,position);
            FightModel.GetCurrent().AddEnemyBullets(bullet);
        }
    }



}
