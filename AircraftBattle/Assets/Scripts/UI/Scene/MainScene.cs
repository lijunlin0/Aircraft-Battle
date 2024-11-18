using UnityEngine;

public class MainScene :MonoBehaviour
{
    private FightManager mFightManager;
    private ScoreText mScoreText;
    private EnergyBar mEnergyBar;
    private static MainScene sMainScene;
    public void Awake()
    {
        sMainScene=this;
        AudioManager.AudioManagerInit(GameObject.Find("AudioManager").AddComponent<AudioManager>());
        Utility.InitScreen();
        Background.Create();
        Application.targetFrameRate=60;
        GameManager.GetInstance();
        mFightManager=new FightManager();
        mEnergyBar=EnergyBar.Create();
        mScoreText=ScoreText.Create();
        GameObject canvas1=GameObject.Find("UICanvas");
        GameObject canvas2=GameObject.Find("BackgroundCanvas");
        if(!Utility.IsPC)
        {
            InputManager inputManager=canvas1.AddComponent<InputManager>();
            inputManager.Init(Joystick.Create());
        }
        Camera mainCamera = Camera.main;
        RectTransform transform=canvas1.GetComponent<RectTransform>();
        transform.sizeDelta = new Vector2(Utility.WindowWidth, Utility.WindowHeight); 
        RectTransform transform2=canvas2.GetComponent<RectTransform>();
        transform2.sizeDelta = new Vector2(Utility.WindowWidth, Utility.WindowHeight); 
        mainCamera.orthographicSize=Utility.WindowHeight/2;
    }

    public void Update()
    {
        mFightManager.OnUpdate();
    }

    public EnergyBar GetEnergyBar(){return mEnergyBar;}
    public static MainScene GetCurrent(){return sMainScene;}
    public ScoreText GetScoreText(){return mScoreText;}

}