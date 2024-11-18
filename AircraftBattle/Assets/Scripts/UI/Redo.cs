using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RedoButton:MonoBehaviour
{
    public static RedoButton Create(bool isWin)
    {
        GameObject prefab=Resources.Load<GameObject>("UI/Redo");
        GameObject gameObject=Instantiate(prefab,GameObject.Find("UICanvas").transform);
        RedoButton redoButton=gameObject.AddComponent<RedoButton>();
        redoButton.Init(isWin);
        return redoButton;
    }

    private void Init(bool isWin)
    {
        TMP_Text text=transform.Find("Text").GetComponent<TextMeshProUGUI>();
        text.text=isWin?"Win":"Lose";
        Button button=GetComponent<Button>();
        button.onClick.AddListener(()=>
        {
            DOTween.KillAll();
            SceneManager.LoadScene("Main");
        });
    }
}