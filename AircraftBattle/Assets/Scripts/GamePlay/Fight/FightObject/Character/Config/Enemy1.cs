
using UnityEngine;

public class Enemy1 : Enemy
{
    public static Enemy1 Create(Vector3 position)
    {
        GameObject enemyObject=FightManager.GetCurrent().GetPoolManager().GetGameObject("Character/Enemy1");
        enemyObject.transform.position=position;
        Enemy1 enemy=enemyObject.AddComponent<Enemy1>();
        enemy.Init();
        return enemy;
    } 

    protected override void Init()
    {
        base.Init(CharacterId.Enemy1);
        mShootTime=2f;
        mMoveSpeed=400;
        mAttack=5;
        mMaxHealth=50;
        mHealth=mMaxHealth;
        //添加子弹发射器
        mShooter = new BulletShooter(()=>
        {
            Shoot();
        },mShootTime);
    }
    protected override void Shoot()
    {
        EnemyBullet bullet1=EnemyBullet.Create(this,mAttack);
        EnemyBullet bullet2=EnemyBullet.Create(this,mAttack,30);
        EnemyBullet bullet3=EnemyBullet.Create(this,mAttack,-30);
        FightModel.GetCurrent().AddEnemyBullets(bullet1);
        FightModel.GetCurrent().AddEnemyBullets(bullet2);
        FightModel.GetCurrent().AddEnemyBullets(bullet3);
    }

    protected override void Move()
    {        
        Vector3 moveDirection=FightUtility.RotationToDirection(gameObject.transform.rotation);
        if(Mathf.Abs(Player.GetCurrent().transform.position.x-transform.position.x)<20)
        {

        }
        else if(Player.GetCurrent().transform.position.x>transform.position.x)
        {
            moveDirection+=new Vector3(1,0,0);
        }
        else
        {
            moveDirection+=new Vector3(-1,0,0); 
        }
        float xScaleFactor = 0.1f;
        moveDirection.x *= xScaleFactor;
        transform.position+=moveDirection.normalized*mMoveSpeed*Time.deltaTime;
    }

}
