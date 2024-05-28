using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : BaseScreen
{
    [SerializeField] Button restartButton;
    [SerializeField] Button homeButton;
    [SerializeField] Button exitButton;


    private void Start()
    {
        restartButton.onClick.AddListener(OnRetry);
        homeButton.onClick.AddListener(OnHome);
        exitButton.onClick.AddListener(OnExit);
    }

    public override void ActivateScreen()
    {
        base.ActivateScreen();
    }

    public override void DeActivateScreen()
    {
        base.DeActivateScreen();
    }

    void OnRetry()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Play);
    }

    void OnHome()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Home);
    }

    void OnExit()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        Application.Quit();
    }

}
