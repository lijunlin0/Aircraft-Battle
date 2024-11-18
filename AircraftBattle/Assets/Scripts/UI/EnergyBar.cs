using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class EnergyBar : MonoBehaviour
{
    private Tween mTween;
    private Slider mEnergySlider;
    public static EnergyBar Create( )
    {

        GameObject prefab=Resources.Load<GameObject>("UI/EnergyBar");
        GameObject EnergyObject=Instantiate(prefab,GameObject.Find("UICanvas").transform);
        EnergyBar EnergyBar=EnergyObject.AddComponent<EnergyBar>();
        
        EnergyBar.Init();
        return EnergyBar;
    }
    
    private void Init()
    {
        mEnergySlider=transform.Find("Energy").GetComponent<Slider>();
        mEnergySlider.value=0;
    }

    public void OnEnergyChanged()
    {
        float currentEnergy=Player.GetCurrent().GetEnergy();
        float targetValue=currentEnergy/Player.MaxEnergy;
        mEnergySlider.value=targetValue;
    }

    public void Update()
    {
        if(Player.GetCurrent().IsDead())
        {
            return;
        }
    }

    public void UseEnergy(float duration)
    {
        mTween=mEnergySlider.DOValue(0,duration).SetEase(Ease.Linear);
    }
}
