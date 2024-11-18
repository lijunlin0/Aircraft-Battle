
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class FightUtility
{  
    public static Vector3 RotationToDirection(Quaternion rotation)
    {
        float radian=rotation.eulerAngles.z*Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radian),MathF.Sin(radian),0).normalized;
    }

    public static Quaternion DirectionToRotation(Vector3 direction)
    {
        return Quaternion.Euler(0,0,DirectionToRadian(direction)*Mathf.Rad2Deg);
    }

    public static float DirectionToRadian(Vector3 direction)
    {
        return Mathf.Atan2(direction.y,direction.x);
    }

    public static  Quaternion RadianToRotation(float radian)
    {
        return Quaternion.Euler(0,0,radian*Mathf.Rad2Deg);
    }
    
    public static float SqrDistance2D(Vector3 position1,Vector3 position2)
    {
        float dx=position1.x-position2.x;
        float dy=position1.y-position2.y;
        return dx*dx+dy*dy;
    }
    // 获取两个弧度之间的差值，以相差角度小的方向旋转
    public static float GetDeltaRadian(float startRadian, float targetRadian)
    {
        float twoPI=Mathf.PI*2;
        float deltaRadian=targetRadian-startRadian;
        if(Mathf.Abs(deltaRadian)>MathF.PI)
        {
            startRadian+=startRadian<0?twoPI:0;
            targetRadian+=targetRadian<0?twoPI:0;
            deltaRadian=targetRadian-startRadian;
        }
        return deltaRadian;
    }

    public static void Move(GameObject gameObject,float moveSpeed)
    {
        Vector3 moveDirection=RotationToDirection(gameObject.transform.rotation);
        gameObject.transform.localPosition+=moveDirection*moveSpeed*Time.deltaTime;
    }
    public static void TrackMove(GameObject gameObject, Vector3 targetPosition, float moveSpeed)
    {
        float trackSpeed = 3f; // 每秒旋转的最大角度
        Vector3 currentDirection = RotationToDirection(gameObject.transform.rotation);
        Vector3 targetDirection = (targetPosition - gameObject.transform.position).normalized;

        float currentRadian=DirectionToRadian(currentDirection);
        float targetRadian=DirectionToRadian(targetDirection);
        float deltaRadian=GetDeltaRadian(currentRadian,targetRadian);

        float frameMaxRadian=Time.deltaTime*trackSpeed;
        if (Mathf.Abs(deltaRadian) > frameMaxRadian)
        {
            deltaRadian = frameMaxRadian * (deltaRadian < 0 ? -1 : 1);
        }

        gameObject.transform.rotation=RadianToRotation(currentRadian+deltaRadian);
        // 根据旋转方向前进
        Vector3 moveDirection = RotationToDirection(gameObject.transform.rotation);
        gameObject.transform.localPosition += moveDirection * moveSpeed * Time.deltaTime;
    }

     

}
