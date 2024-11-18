using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家子弹
public class PlayerBullet: Bullet
{ 
    public static PlayerBullet Create(int points,bool isEnergy=false)
    {
        string prefabName;
        if(isEnergy)
        {
            prefabName="Bullet/PlayerEnergyBullet";
        }
        else
        {
            prefabName="Bullet/PlayerBullet";
        }
        GameObject bulletObject=FightManager.GetCurrent().GetPoolManager().GetGameObject(prefabName);
        bulletObject.transform.position=Player.GetCurrent().transform.position;
        PlayerBullet bullet=bulletObject.AddComponent<PlayerBullet>();
        bullet.Init(Player.GetCurrent(),points);
        return bullet;
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
