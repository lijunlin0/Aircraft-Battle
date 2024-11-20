using DG.Tweening;
using TMPro;
using UnityEngine;
public class ScoreText:MonoBehaviour
{
    private TMP_Text mText;
    private const int AnimationDuration=1;
    public static ScoreText Create()
    {
        GameObject prefab=Resources.Load<GameObject>("UI/ScoreText");
        GameObject gameObject=Instantiate(prefab,GameObject.Find("HighUICanvas").transform);
        ScoreText textObject=gameObject.AddComponent<ScoreText>();
        textObject.Init();
        return textObject;
    }

    private void Init()
    {
        mText=GetComponent<TextMeshProUGUI>();
        mText.text="Score:0";
    }

    public void OnScoreChanged()
    {
        mText.text="Score:"+Player.GetCurrent().GetScore().ToString();
    }

    public void PlayEndAnimation()
    {
        transform.DOMove(Vector3.zero,AnimationDuration).SetId("ScoreText");
    }
}