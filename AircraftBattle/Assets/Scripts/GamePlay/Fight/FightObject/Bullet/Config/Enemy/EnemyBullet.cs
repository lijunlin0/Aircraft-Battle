using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//敌人子弹
public class EnemyBullet : Bullet
{
    public static EnemyBullet Create(Character enemy,int points,int angle=0,Vector3? position=null)
    {
        GameObject bulletObject=FightManager.GetCurrent().GetPoolManager().GetGameObject("Bullet/"+enemy.GetCharacterId().ToString()+"Bullet");
        bulletObject.transform.position=position??enemy.transform.position;
        Quaternion newRotation = bulletObject.transform.rotation * Quaternion.Euler(0, 0, angle);
        bulletObject.transform.rotation=newRotation;
        EnemyBullet bullet=bulletObject.AddComponent<EnemyBullet>();
        bullet.Init(enemy,points);
        return bullet;
    }
    protected override void Init(Character character,int points)
    {
        base.Init(character,points);
        mMoveSpeed=600;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if(mIsDead)
        {
            return;
        }
        FightUtility.Move(gameObject,mMoveSpeed);
    }
}