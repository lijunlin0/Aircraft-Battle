using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlButton:MonoBehaviour
{
    private bool mIsTouchControl=true;
    private Sprite mTouchImage;
    private Sprite mRockerImage;
    private Image mImage;
    private TMP_Text mText;
    public static ControlButton Create()
    {
        GameObject prefab=Resources.Load<GameObject>("UI/ControlButton/ControlButton");
        GameObject gameObject=Instantiate(prefab,GameObject.Find("UICanvas").transform);
        ControlButton ControlButton=gameObject.AddComponent<ControlButton>();
        ControlButton.Init();
        return ControlButton;
    }

    private void Init()
    {
        mText=transform.Find("Text").GetComponent<TextMeshProUGUI>();
        mTouchImage=Resources.Load<Sprite>("UI/ControlButton/TouchControl");
        mRockerImage=Resources.Load<Sprite>("UI/ControlButton/RockerControl");
        mImage=transform.Find("Image").GetComponent<Image>();
        UpdateState();
        Button button=GetComponent<Button>();
        button.onClick.AddListener(()=>
        {
            AudioManager.GetCurrent().PlayButtonOnClick();
            mIsTouchControl=!mIsTouchControl;
            UpdateState();
        });
    }
    private void UpdateState()
    {
        mText.text=mIsTouchControl?"Touch":"Rocker";
        mImage.sprite=mIsTouchControl?mTouchImage:mRockerImage;
        MainScene.GetCurrent().GetJoystick().gameObject.SetActive(!mIsTouchControl);
    }

    public bool IsTouchControl(){return mIsTouchControl;}
}