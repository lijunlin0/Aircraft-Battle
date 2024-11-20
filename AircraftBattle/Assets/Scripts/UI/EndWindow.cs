using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndWindow:MonoBehaviour
{
    private const float AnimationDuration=1;
    private CanvasGroup mCanvasGroup;
    public static EndWindow Create(bool isWin)
    {
        Transform parent=GameObject.Find("WindowCanvas").transform;
        Instantiate(Resources.Load<GameObject>("UI/Mask").gameObject,parent);
        GameObject prefab=Resources.Load<GameObject>("UI/EndWindow");
        GameObject gameObject=Instantiate(prefab,parent);
        EndWindow window=gameObject.AddComponent<EndWindow>();
        window.Init(isWin);
        return window;
    }

    private void Init(bool isWin)
    {
        mCanvasGroup=GetComponent<CanvasGroup>();
        mCanvasGroup.alpha=0;
        TMP_Text text=transform.Find("Placard/Text").GetComponent<TextMeshProUGUI>();
        text.text=isWin?" Win!":"Lose";
        Button button=transform.Find("Redo").GetComponent<Button>();
        button.onClick.AddListener(()=>
        {
            AudioManager.GetCurrent().PlayButtonOnClick();
            DOTween.KillAll();
            SceneManager.LoadScene("Main");
        });
        PlayEndAnimation();
    }
    public void PlayEndAnimation()
    {
        mCanvasGroup.DOFade(1,AnimationDuration);
    }
}