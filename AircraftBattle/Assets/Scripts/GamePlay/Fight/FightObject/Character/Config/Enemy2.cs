using DG.Tweening;
using UnityEngine;


public class Enemy2 : Enemy
{
    public static Enemy2 Create(Vector3 position)
    {
        GameObject enemyObject=FightManager.GetCurrent().GetPoolManager().GetGameObject("Character/Enemy2");
        enemyObject.transform.position=position;
        Enemy2 enemy=enemyObject.AddComponent<Enemy2>();
        enemy.Init();
        return enemy;
    }

    protected override void Init()
    {
        base.Init(CharacterId.Enemy2);
        mShootTime=1.5f;
        mMoveSpeed=450;
        mAttack=15;
        mMaxHealth=40;
        mHealth=mMaxHealth;
        mShooter = new BulletShooter(()=>
        {
            Shoot();
        },mShootTime);
    }


    protected override void Shoot()
    {
        base.Shoot();
        //创建子弹
        EnemyBullet2 bullet=EnemyBullet2.Create(this,mAttack);
        //方向朝着玩家
        Vector3 direction=(FightModel.GetCurrent().GetPlayer().transform.position-bullet.transform.position).normalized;
        bullet.transform.localRotation=FightUtility.DirectionToRotation(direction);
        FightModel.GetCurrent().AddEnemyBullets(bullet);
    }

}
