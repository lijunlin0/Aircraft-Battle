
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    private int MaxMoveDistance=Utility.WindowHeight;
    private int mMoveSpeed=300;
    private float mMoveDistance=0;
    public  static Background Create()
    {
        Canvas canvas=GameObject.Find("BackgroundCanvas").GetComponent<Canvas>();
        GameObject prefab=Resources.Load<GameObject>("UI/Background");
        GameObject gameObject=GameObject.Instantiate(prefab,canvas.transform);
        gameObject.transform.position+=new Vector3(0,0,(int)DisplayLayer.Background);
        Background background=gameObject.AddComponent<Background>();
        return background;
    }


    public void Update()
    {
        if(FightManager.GetCurrent().IsGameOver())
        {
            return;
        }
        transform.position+=Vector3.down*mMoveSpeed*Time.deltaTime;
        mMoveDistance += mMoveSpeed * Time.deltaTime;
        if(mMoveDistance>=MaxMoveDistance)
        {
            transform.position+=new Vector3(0,MaxMoveDistance,0);
            mMoveDistance=0;
        }
    }
}