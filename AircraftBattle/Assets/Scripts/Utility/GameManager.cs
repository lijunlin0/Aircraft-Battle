
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    //构造
    public void OnSingletonInit()
    {
        DOTween.Init();
    }
}
