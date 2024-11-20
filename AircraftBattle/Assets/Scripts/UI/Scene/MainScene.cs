using UnityEngine;

public class MainScene :MonoBehaviour
{
    private FightManager mFightManager;
    private ScoreText mScoreText;
    private EnergyBar mEnergyBar;
    private ControlButton mControlButton;
    private Joystick mJoystick;
    private static MainScene sMainScene;
    public void Awake()
    {
        sMainScene=this;
        AudioManager.AudioManagerInit(GameObject.Find("AudioManager").AddComponent<AudioManager>());
        Utility.InitScreen();
        GameObject uiCanvas=GameObject.Find("UICanvas");
        if(!Utility.IsPC)
        {
            mJoystick=Joystick.Create();
            InputManager inputManager=uiCanvas.AddComponent<InputManager>();
            inputManager.Init(mJoystick);
        }
        Background.Create();
        Application.targetFrameRate=60;
        GameManager.GetInstance();
        mFightManager=new FightManager();
        mEnergyBar=EnergyBar.Create();
        mScoreText=ScoreText.Create();
        mControlButton=ControlButton.Create();


        Camera mainCamera = Camera.main;
        Canvas[] canvases=FindObjectsOfType<Canvas>();
        foreach(Canvas canvas in canvases)
        {
            RectTransform rectTransform=canvas.GetComponent<RectTransform>();
            rectTransform.sizeDelta=new Vector2(Utility.WindowWidth, Utility.WindowHeight); 
        } 
        mainCamera.orthographicSize=Utility.WindowHeight/2;
    }

    public void Update()
    {
        mFightManager.OnUpdate();
    }

    public EnergyBar GetEnergyBar(){return mEnergyBar;}
    public static MainScene GetCurrent(){return sMainScene;}
    public ScoreText GetScoreText(){return mScoreText;}
    public ControlButton GetControlButton(){return mControlButton;}
    public Joystick GetJoystick(){return mJoystick;}

}