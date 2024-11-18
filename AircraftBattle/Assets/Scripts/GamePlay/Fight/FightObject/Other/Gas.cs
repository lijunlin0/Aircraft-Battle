using DG.Tweening;
using UnityEngine;

public class Gas:MonoBehaviour
{
    private const float MaxLifeTime=0f;
    private bool mIsDead=false;
    private float mLiveTime=0;
    private SpriteRenderer mRenderer;
    private const float FadeTime=0.3f;
    public static Gas Create(Vector3 position,bool IsBullet)
    {
        string prefabName;
        if(IsBullet)
        {
            prefabName="Other/Gas";
        }
        else
        {
            prefabName="Other/Gas2";
        }
        GameObject gameObject=FightManager.GetCurrent().GetPoolManager().GetGameObject(prefabName);
        gameObject.transform.position=position;
        Gas gas=gameObject.AddComponent<Gas>();
        gas.Init();
        return gas;
    }

    private void Init()
    {
        mRenderer=transform.Find("Display").GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if(mIsDead)
        {
            return;
        }
        mLiveTime+=Time.deltaTime;
        if(mLiveTime>=MaxLifeTime)
        {
            mIsDead=true;
            PlayDestroyAnimation();
        }
    }

    private void PlayDestroyAnimation()
    {
        Sequence sequence=DOTween.Sequence();
        sequence.Append(gameObject.transform.DOScale(0,FadeTime));
        sequence.Join(mRenderer.DOColor(new Color(1,1,1,0),FadeTime));
        sequence.OnComplete(()=>
        {
            transform.localScale=new Vector3(1, 1, 1);
            mRenderer.color=new Color(1,1,1,1);
            FightManager.GetCurrent().GetPoolManager().PutGameObject(gameObject);
            Destroy(this);
        });
        sequence.Play();
    }
}