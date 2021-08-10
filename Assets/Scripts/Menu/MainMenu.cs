using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : UIMenu {
    [SerializeField] private Image background;
    
    [Space]
    [Header("Start Button")]
    [SerializeField] private Button startButton;
    [SerializeField] private Image startButtonImage;
    
    [Space]
    [Header("Settings Button")]
    [SerializeField] private Button settingsButton;
    [SerializeField] private Image settingsButtonImage;
    
    [Space]
    [Header("Exit Button")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Image exitButtonImage;
    
    
    private void Awake() {
        base.Awake();
        Init();
        
        startButton.onClick.AddListener(StartButtonClicked);
        settingsButton.onClick.AddListener(SettingsButtonClicked);
        exitButton.onClick.AddListener(ExitButtonClicked);
    }

    private void Start() {
    }


    private void Init() {
        controls = new List<dynamic> { startButton, settingsButton, exitButton };
        texts = new List<dynamic>();
        controlsText = new List<dynamic>();
        InitControlsTexts(controls, controlsText);

        initCompleted = true;
        if (Tracker.IsInit) Tracker.Instance.Log("MainMenu is init.");
    }


    private void StartButtonClicked() {
        GameManager.Instance.UpdateState(GameState.Game.Running);
    }

    private void SettingsButtonClicked() {
        GameManager.Instance.UpdateState(GameState.Menu.InSettings);
    }

    private void ExitButtonClicked() {
        GameManager.Instance.ExitGame();
    }
    

    protected override void SetFont(TMP_FontAsset font) {
        StartCoroutine(SetMenuFont(font));
    }

    protected override void SetSkin(UISkin skin) {
        StartCoroutine(SetMenuSkin(skin));
    }

    protected override IEnumerator SetMenuSkin(UISkin skin) {
        yield return new WaitUntil(() => initCompleted);
        foreach (var text in controlsText) text.color = skin.controlsTextColor;
        background.color = skin.mainMenuBackgroundColor;
        
        foreach (var b in new []{startButton, settingsButton, exitButton}) 
            b.colors = skin.mainMenuButtons.colors.colors;
    }
}