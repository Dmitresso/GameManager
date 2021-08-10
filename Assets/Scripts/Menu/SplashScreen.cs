using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SplashScreen : UIMenu {
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text taglineText;

    
    private void Awake() {
        Init();
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) UIManager.Instance.SetTrigger(Data.Animator.Parameters.Id.Trigger.SpaceBarClick);
    }

    private void Init() {
        controls = new List<dynamic>();
        texts = new List<dynamic>();
        controlsText = new List<dynamic>();
        initCompleted = true;
        if (Tracker.IsInit) Tracker.Instance.Log("SplashScreen is init.");
    }
    
    
    protected override void SetFont(TMP_FontAsset font) {
        StartCoroutine(SetMenuFont(font));
    }

    protected override void SetSkin(UISkin skin) {
        StartCoroutine(SetMenuSkin(skin));
    }

    protected override IEnumerator SetMenuSkin(UISkin skin) {
        yield return new WaitUntil(() => initCompleted);

        background.color = skin.splashScreenBackgroundColor;
        titleText.color = skin.titleTextColor;
        taglineText.color = skin.taglineTextColor;
    }
}