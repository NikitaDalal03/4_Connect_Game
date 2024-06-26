using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : BaseScreen
{
    [SerializeField] Button playButton;
    

    private void Start()
    {
        playButton.onClick.AddListener(OnPlay);
    }


    public override void ActivateScreen()
    {
        base.ActivateScreen();
    }

    void OnPlay()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Play);
    }


}

