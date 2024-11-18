using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HealthBar : MonoBehaviour
{
    private const int YOffset=60;
    private Character mCharacter;
    private Slider mHealthSlider;
    private Slider mDamageSlider;
    private Tween mTween;
    public static HealthBar Create(Character character)
    {
        
        GameObject prefab=Resources.Load<GameObject>("UI/HealthBar");
        GameObject healthObject=Instantiate(prefab,character.transform.position,Quaternion.identity,GameObject.Find("UICanvas").transform);
        HealthBar healthBar=healthObject.AddComponent<HealthBar>();
        healthBar.Init(character);
        return healthBar;
    }
    
    private void Init(Character character)
    {
        mCharacter=character;
        mHealthSlider=transform.Find("Health").GetComponent<Slider>();
        mDamageSlider=transform.Find("Damage").GetComponent<Slider>();
        mHealthSlider.value=1;
        mDamageSlider.value=1;
    }

    public void OnHealthChanged()
    {
        float maxHealth=mCharacter.GetMaxHealth();
        float currentHealth=mCharacter.GetHealth();
        float targetValue=currentHealth/maxHealth;
        mHealthSlider.value=targetValue;
        //加血时，伤害条直接补齐
        if(mDamageSlider.value<targetValue)
        {
            mDamageSlider.value=targetValue;
        }
        //掉血时，动画
        else
        {
            if(mTween!=null)
            {
                mTween.Kill();
            }
            mTween=mDamageSlider.DOValue(targetValue,0.2f).SetEase(Ease.InQuint).OnComplete(()=>
            {
                mTween=null;
            });
        }
    }

    public void Update()
    {
        if(mCharacter.IsDead())
        {
            mHealthSlider.value=0;
            mTween.Kill();
            Destroy(gameObject);
        }
        transform.position = mCharacter.transform.position + new Vector3(0, YOffset, 0); 
    }
}
