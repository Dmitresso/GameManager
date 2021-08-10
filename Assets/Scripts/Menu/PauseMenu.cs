using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PauseMenu : UIMenu {
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI pauseText;
    
    [Space]
    [Header("Resume Button")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Image resumeButtonImage;
    
    [Space]
    [Header("Restart Button")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Image restartButtonImage;
    
    [Space]
    [Header("Quit Button")]
    [SerializeField] private Button quitButton;
    [SerializeField] private Image quitButtonImage;
    
    
    private void Awake() {
        base.Awake();
        Init();
        
        resumeButton.onClick.AddListener(ResumeButtonClicked);
        restartButton.onClick.AddListener(RestartButtonClicked);
        quitButton.onClick.AddListener(QuitButtonClicked);
    }
    
    private void Init() {
        controls = new List<dynamic> { resumeButton, restartButton, quitButton };
        texts = new List<dynamic> { pauseText };
        controlsText = new List<dynamic>();
        InitControlsTexts(controls, controlsText);

        initCompleted = true;
        if (Tracker.IsInit) Tracker.Instance.Log("PauseMenu is init.");
    }

    private void ResumeButtonClicked() {
        GameManager.Instance.UpdateState(GameState.Game.Running);
    }

    private void RestartButtonClicked() {
        GameManager.Instance.RestartGame();
    }

    private void QuitButtonClicked() {
        GameManager.Instance.UpdateState(GameState.Menu.InMain);
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
        foreach (var text in texts) text.color = skin.pauseMenuTextColor;
        background.color = skin.pauseMenuBackgroundColor;

        foreach (var b in new []{resumeButton, restartButton, quitButton}) 
            b.colors = skin.pauseMenuButtons.colors.colors;
    }
}