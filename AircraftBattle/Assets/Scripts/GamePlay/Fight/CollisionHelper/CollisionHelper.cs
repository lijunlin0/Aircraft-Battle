using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class CollisionHelper
{
    public static bool IsColliding(MyCollider collider1,MyCollider collider2)
    {
        if(!collider1.IsEnable()||!collider2.IsEnable())
        {
            return false;
        }
        return collider1.GetCollider().Distance(collider2.GetCollider()).isOverlapped;
    }

    public static void Collide()
    {
        FightModel fightModel=FightModel.GetCurrent();
        PlayerWithEnemyCollide(fightModel.GetEnemies());
        PlayerBulletHitEnemy(fightModel.GetPlayerBullets(),fightModel.GetEnemies());
        EnemyBulletHitPlayer(fightModel.GetEnemyBullets());

    }
    //玩家与敌人碰撞
    private static void PlayerWithEnemyCollide(List<Character> enemies)
    {
        Player player= Player.GetCurrent();
        MyCollider collider1=player.GetCollider();
        foreach(Character enemy in enemies)
        {
            MyCollider collider2=enemy.GetCollider();
            if(IsColliding(collider1,collider2))
            {
                int points=enemy.GetAttack();
                player.GetDamage(points);
                break;
            }
        }
    }

    //玩家子弹命中敌人
    private static void PlayerBulletHitEnemy(List<Bullet> bullets,List<Character> enemies)
    {
        foreach(Bullet bullet in bullets)
        {
            MyCollider collider1=bullet.GetCollider();
            foreach(Character enemy in enemies)
            {
                MyCollider collider2=enemy.GetCollider();
                if(IsColliding(collider1,collider2))
                {
                    Player.GetCurrent().AddEnergy(3);
                    bullet.OnCollideCharacter(enemy);
                }
            }
        }
    }

    //敌人子弹命中玩家
    private static void EnemyBulletHitPlayer(List<Bullet> bullets)
    {
        Player player=Player.GetCurrent();
        MyCollider collider1=player.GetCollider();
        foreach(Bullet bullet in bullets)
        {
            MyCollider collider2=bullet.GetCollider();
            if(IsColliding(collider1,collider2)) 
            {
                bullet.OnCollideCharacter(player);
            }
        }
    }
}