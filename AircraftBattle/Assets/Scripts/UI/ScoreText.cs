using TMPro;
using UnityEngine;
public class ScoreText:MonoBehaviour
{
    private TMP_Text mText;
    public static ScoreText Create()
    {
        GameObject prefab=Resources.Load<GameObject>("UI/ScoreText");
        GameObject gameObject=Instantiate(prefab,GameObject.Find("UICanvas").transform);
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
}