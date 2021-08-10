using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Animator = UnityEngine.Animator;


public class SettingsMenu : UIMenu {
    [Header("Go Back")]
    [SerializeField] private Button goBackButton;
    [SerializeField] private Image goBackButtonImage;
    
    [Space]
    [Header("Transitions Type")]
    [SerializeField] private TextMeshProUGUI transitionsTypeText;
    [SerializeField] private TMP_Dropdown transitionsTypeDropdown;
    [SerializeField] private Image transitionsTypeDropdownArrow;
    [SerializeField] private Image transitionsTypeDropdownTemplateBackground;
    [SerializeField] private Image transitionsTypeDropdownItemBackground;
    [SerializeField] private Image transitionsTypeDropdownCheckmark;

    [Space]
    [Header("Animation Speed")]
    [SerializeField] private TextMeshProUGUI animatorSpeedText;
    [SerializeField] private Slider animatorSpeedSlider;
    [SerializeField] private Image animatorSpeedSliderBackground;
    [SerializeField] private Image animatorSpeedSliderHandle;
    [SerializeField] private Image animatorSpeedSliderFill;
    
    [Space]
    [Header("Volume")]
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image volumeSliderBackground;
    [SerializeField] private Image volumeSliderHandle;
    [SerializeField] private Image volumeSliderFill;
    
    [Space]
    [Header("Screen Resolution")]
    [SerializeField] private TextMeshProUGUI screenResText;
    [SerializeField] private TMP_Dropdown screenResDropdown;
    [SerializeField] private Image screenResDropdownArrow;
    [SerializeField] private Image screenResDropdownTemplateBackground;
    [SerializeField] private Image screenResDropdownItemBackground;
    [SerializeField] private Image screenResDropdownCheckmark;
    
    [Space]
    [Header("Skin")]
    [SerializeField] private TextMeshProUGUI skinText;
    [SerializeField] private TMP_Dropdown skinDropdown;
    [SerializeField] private Image skinDropdownArrow;
    [SerializeField] private Image skinDropdownTemplateBackground;
    [SerializeField] private Image skinDropdownItemBackground;
    [SerializeField] private Image skinDropdownCheckmark;
    
    [Space]
    [Header("Font")]
    [SerializeField] private TextMeshProUGUI fontText;
    [SerializeField] private TMP_Dropdown fontDropdown;
    [SerializeField] private Image fontDropdownArrow;
    [SerializeField] private Image fontDropdownTemplateBackground;
    [SerializeField] private Image fontDropdownItemBackground;
    [SerializeField] private Image fontDropdownCheckmark;
    
    [Space]
    [Header("Skybox Material")]
    [SerializeField] private TextMeshProUGUI skyboxMaterialText;
    [SerializeField] private TMP_Dropdown skyboxMaterialDropdown;
    [SerializeField] private Image skyboxMaterialDropdownArrow;
    [SerializeField] private Image skyboxMaterialDropdownTemplateBackground;
    [SerializeField] private Image skyboxMaterialDropdownItemBackground;
    [SerializeField] private Image skyboxMaterialDropdownCheckmark;
    
    [Space]
    [Header("Skybox Type")]
    [SerializeField] private TextMeshProUGUI skyboxTypeText;
    [SerializeField] private TMP_Dropdown skyboxTypeDropdown;
    [SerializeField] private Image skyboxTypeDropdownArrow;
    [SerializeField] private Image skyboxTypeDropdownTemplateBackground;
    [SerializeField] private Image skyboxTypeDropdownItemBackground;
    [SerializeField] private Image skyboxTypeDropdownCheckmark;
    
    [Space]
    [Header("Swap Skyboxes")]
    [SerializeField] private TextMeshProUGUI swapSkyboxesText;
    [SerializeField] private Toggle swapSkyboxesToggle;
    [SerializeField] private Image swapSkyboxesToggleBackground;
    [SerializeField] private Image swapSkyboxesToggleCheckmark;
    
    
    private Animator animator;
    private MenuTransitions[] menuTransitionsBehaviors;
    private List<string> resolutions = new List<string>();


    public bool SkinDropdownInteractable {
        get => skinDropdown.interactable;
        set => skinDropdown.interactable = value;
    }

    public bool FontDropdownInteractable {
        get => fontDropdown.interactable;
        set => fontDropdown.interactable = value;
    }

    public bool SkyboxMaterialDropdownInteractable {
        get => skyboxMaterialDropdown.interactable;
        set => skyboxMaterialDropdown.interactable = value;
    }


    private void OnDisable() {
        StopAllCoroutines();
    }

    private void Awake() {
        base.Awake();
        Init();
        
        UIManager.Instance.UpdateFontDropdownOptions += UpdateFontDropdownOptions;
        UIManager.Instance.UpdateSkinDropdownOptions += UpdateSkinDropdownOptions;

        UIManager.Instance.SetFontDropdownValue += SetFontDropdownValue;
        UIManager.Instance.SetSkinDropdownValue += SetSkinDropdownValue;
        
        goBackButton.onClick.AddListener(GoBackButtonClicked);
        transitionsTypeDropdown.onValueChanged.AddListener(TransitionsTypeDropdownValueChanged);
        animatorSpeedSlider.onValueChanged.AddListener(AnimatorSpeedSliderValueChanged);
        volumeSlider.onValueChanged.AddListener(VolumeSliderValueChanged);
        screenResDropdown.onValueChanged.AddListener(ScreenResolutionDropdownValueChanged);
        skinDropdown.onValueChanged.AddListener(SkinDropdownValueChanged);
        fontDropdown.onValueChanged.AddListener(FontDropdownValueChanged);
        skyboxMaterialDropdown.onValueChanged.AddListener(SkyboxMaterialDropdownValueChanged);
        skyboxTypeDropdown.onValueChanged.AddListener(SkyboxTypeDropdownValueChanged);
        swapSkyboxesToggle.onValueChanged.AddListener(SkyboxSwapToggleValueChanged);
        
        
        transitionsTypeDropdown.ClearOptions();
        transitionsTypeDropdown.AddOptions(new List<string>{"Fade", "Slide"});

        skinDropdown.ClearOptions();
        skinDropdown.AddOptions(UIManager.Instance.Skins.Where(skin => skin != null).Select(skin => skin.name).ToList());

        fontDropdown.ClearOptions();
        fontDropdown.AddOptions(UIManager.Instance.Fonts.Where(font => font != null).Select(font => font.name).ToList());
        
        skyboxTypeDropdown.ClearOptions();
        skyboxTypeDropdown.AddOptions(new List<string>{Skybox.Light.ToString(), Skybox.Dark.ToString(), Skybox.Random.ToString()});
        skyboxTypeDropdown.SetValueWithoutNotify((int) SkyboxManager.Instance.SelectedSkyboxType);

        skyboxMaterialDropdown.ClearOptions();
        skyboxMaterialDropdown.AddOptions(SkyboxManager.Instance.Skyboxes.ConvertAll(material => material.name));
        skyboxMaterialDropdown.SetValueWithoutNotify(SkyboxManager.Instance.SelectedSkyboxIndex);
    }

    private void Start() {
    }

    private void Update() { }

    private void Init() {
        volumeSlider.value = UIManager.Instance.Volume;
        
        controls = new List<dynamic> {
            transitionsTypeDropdown,
            animatorSpeedSlider,
            volumeSlider,
            screenResDropdown,
            skinDropdown, 
            fontDropdown,
            skyboxMaterialDropdown,
            skyboxTypeDropdown,
            swapSkyboxesToggle
        };

        texts = new List<dynamic> {
            transitionsTypeText,
            animatorSpeedText,
            volumeText,
            screenResText,
            skinText,
            fontText,
            skyboxMaterialText,
            skyboxTypeText,
            swapSkyboxesText
        };
        
        controlsText = new List<dynamic>();
        InitControlsTexts(controls, controlsText);

        animator = GetComponentInParent<Animator>();
        menuTransitionsBehaviors = animator.GetBehaviours<MenuTransitions>();
        
        StartCoroutine(nameof(InitResolutions));

        initCompleted = true;
        if (Tracker.IsInit) Tracker.Instance.Log("SettingsMenu is init.");
    }
    
    
    private IEnumerator InitResolutions() {
        yield return new WaitUntil(() => ResolutionManager.Instance.ResolutionsInit);
        foreach (var r in ResolutionManager.Instance.windowedResolutions) resolutions.Add(r.x + "×" + r.y);
        
        if (ResolutionManager.Instance.fullscreenResolutions.Count >= 2) {
            resolutions.Add(ResolutionManager.Instance.fullscreenResolutions[ResolutionManager.Instance.fullscreenResolutions.Count - 2].x +
                            "×" + ResolutionManager.Instance.fullscreenResolutions[ResolutionManager.Instance.fullscreenResolutions.Count - 2].y +
                            " (Fullscreen)");
        }
        resolutions.Add(ResolutionManager.Instance.fullscreenResolutions[ResolutionManager.Instance.fullscreenResolutions.Count - 1].x +
                        "×" + ResolutionManager.Instance.fullscreenResolutions[ResolutionManager.Instance.fullscreenResolutions.Count - 1].y +
                        " (Fullscreen)");
        
        screenResDropdown.ClearOptions();
        screenResDropdown.AddOptions(resolutions);
    }

    private void GoBackButtonClicked() {
        if (Tracker.IsInit) Tracker.Instance.Log("GoBackButtonClicked event was called.");
        GameManager.Instance.UpdateState(GameState.Menu.InMain);
    }

    private void TransitionsTypeDropdownValueChanged(int value) {
        if (Tracker.IsInit) Tracker.Instance.Log($"transitionTypeDropdown value was changed to {value}.");
        switch (value) {
            case 0: foreach (MenuTransitions b in menuTransitionsBehaviors) b.Transitions = new[] { Transition.Fade, Transition.Fade }; break;
            case 1: foreach (MenuTransitions b in menuTransitionsBehaviors) b.Transitions = new[] { Transition.Slide, Transition.Slide }; break;
        }
    }

    private void AnimatorSpeedSliderValueChanged(float value) {
        var v = (int) Math.Round(value);
        UIManager.Instance.AnimatorSpeed = v;
        if (Tracker.IsInit) Tracker.Instance.Log($"animatorSpeedSlider value was changed to {v}.");
    }

    private void VolumeSliderValueChanged(float value) {
        UIManager.Instance.Volume = value;
    }

    private void ScreenResolutionDropdownValueChanged(int value) {
        var fullscreen = value > ResolutionManager.Instance.windowedResolutions.Count - 1;
        ResolutionManager.Instance.SetResolution(value, fullscreen);
        if (Tracker.IsInit) Tracker.Instance.Log($"screenResDropdown value was changed to {value}.");
    }

    private void SkinDropdownValueChanged(int value) {
        UIManager.Instance.SelectedSkin = UIManager.Instance.Skins[value];
        if (Tracker.IsInit) Tracker.Instance.Log($"skinDropdown value was changed to {value}.");
    }

    private void FontDropdownValueChanged(int value) {
        UIManager.Instance.SelectedFont = UIManager.Instance.Fonts[value];
        if (Tracker.IsInit) Tracker.Instance.Log($"fontDropdown value was changed to {value}.");
    }
    
    private void SkyboxMaterialDropdownValueChanged(int value) {
        if (Tracker.IsInit) Tracker.Instance.Log($"skyboxMaterialDropdown value was changed to {value}.");
        switch (SkyboxManager.Instance.SelectedSkyboxType) {
            case Skybox.Light: StartCoroutine(SkyboxManager.Instance.ChangeSkyboxWithFade(Skybox.Light, value)); break;            
            case Skybox.Dark: StartCoroutine(SkyboxManager.Instance.ChangeSkyboxWithFade(Skybox.Dark, value)); break;
            case Skybox.Random: StartCoroutine(SkyboxManager.Instance.ChangeSkyboxWithFade(Skybox.Random, value)); break;
        }
    }

    private void SkyboxTypeDropdownValueChanged(int value) {
        SkyboxManager.Instance.SelectedSkyboxType = (Skybox) value;
        StartCoroutine(SkyboxManager.Instance.ChangeSkyboxWithFade(SkyboxManager.Instance.SelectedSkyboxType, 0));
        skyboxMaterialDropdown.ClearOptions();
        skyboxMaterialDropdown.AddOptions(SkyboxManager.Instance.Skyboxes.ConvertAll(material => material.name));
        if (Tracker.IsInit) Tracker.Instance.Log($"{skyboxMaterialDropdown} value was changed.");
    }

    private void SkyboxSwapToggleValueChanged(bool value) {
        UIManager.Instance.SwapSkyboxes = value;
        if (Tracker.IsInit) Tracker.Instance.Log($"SwapSkyboxes value was changed to {value}.");
    }

    
    
    
    private void UpdateFontDropdownOptions(List<TMP_FontAsset> options) {
        var opts = new List<TMP_FontAsset>(options);
        opts.RemoveAll(font => font is null);
        fontDropdown.options.Clear();
        fontDropdown.AddOptions(opts.Select(option => option.name).ToList());
        var option = opts.IndexOf(UIManager.Instance.SelectedFont);
        skinDropdown.SetValueWithoutNotify(option);
        FontDropdownInteractable = opts.Count > 1;
        if (Tracker.IsInit) Tracker.Instance.Log($"{fontDropdown} value was changed to {option}.");
    }

    private void UpdateSkinDropdownOptions(List<UISkin> options) {
        var opts = new List<UISkin>(options);
        opts.RemoveAll(skin => skin is null);
        skinDropdown.options.Clear();
        skinDropdown.AddOptions(opts.Select(option => option.name).ToList());
        var option = opts.IndexOf(UIManager.Instance.SelectedSkin);
        skinDropdown.SetValueWithoutNotify(option);
        SkinDropdownInteractable = opts.Count > 1;
        if (Tracker.IsInit) Tracker.Instance.Log($"{skinDropdown} value was changed to {option}.");
    }

    private void SetFontDropdownValue(int value) {
        fontDropdown.SetValueWithoutNotify(value);
    }

    private void SetSkinDropdownValue(int value) {
        skinDropdown.SetValueWithoutNotify(value);
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
        foreach (var text in texts) {
            text.color = skin.settingsMenuTextColor;
            text.outlineColor = skin.settingsMenuTextOutlineColor;
            text.outlineWidth = skin.settingsMenuTextOutlineWidth;
        }

        goBackButton.spriteState = skin.goBackButton.sprites.spriteState;
        goBackButtonImage.sprite = skin.goBackButton.sprites.sprite;
        
        transitionsTypeDropdown.colors = skin.settingsMenuDropdowns.colors.colors;
        transitionsTypeDropdownArrow.sprite = skin.settingsMenuDropdowns.arrow;
        transitionsTypeDropdownCheckmark.sprite = skin.settingsMenuDropdowns.checkmark;
        transitionsTypeDropdownTemplateBackground.color = skin.settingsMenuDropdowns.templateBackgroundColor;
        transitionsTypeDropdownItemBackground.color = skin.settingsMenuDropdowns.itemBackgroundColor;
        
        animatorSpeedSlider.colors = skin.settingsMenuSliders.colors.colors;
        animatorSpeedSliderBackground.color = skin.settingsMenuSliders.backgroundColor;
        animatorSpeedSliderHandle.color = skin.settingsMenuSliders.handleColor;
        animatorSpeedSliderFill.color = skin.settingsMenuSliders.fillColor;
        
        volumeSlider.colors = skin.settingsMenuSliders.colors.colors;
        volumeSliderBackground.color = skin.settingsMenuSliders.backgroundColor;
        volumeSliderHandle.color = skin.settingsMenuSliders.handleColor;
        volumeSliderFill.color = skin.settingsMenuSliders.fillColor;

        screenResDropdown.colors = skin.settingsMenuDropdowns.colors.colors;
        screenResDropdownArrow.sprite = skin.settingsMenuDropdowns.arrow;
        screenResDropdownCheckmark.sprite = skin.settingsMenuDropdowns.checkmark;
        screenResDropdownTemplateBackground.color = skin.settingsMenuDropdowns.templateBackgroundColor;
        screenResDropdownItemBackground.color = skin.settingsMenuDropdowns.itemBackgroundColor;
        
        skinDropdown.colors = skin.settingsMenuDropdowns.colors.colors;
        skinDropdownArrow.sprite = skin.settingsMenuDropdowns.arrow;
        skinDropdownCheckmark.sprite = skin.settingsMenuDropdowns.checkmark;
        skinDropdownTemplateBackground.color = skin.settingsMenuDropdowns.templateBackgroundColor;
        skinDropdownItemBackground.color = skin.settingsMenuDropdowns.itemBackgroundColor;

        fontDropdown.colors = skin.settingsMenuDropdowns.colors.colors;
        fontDropdownArrow.sprite = skin.settingsMenuDropdowns.arrow;
        fontDropdownCheckmark.sprite = skin.settingsMenuDropdowns.checkmark;
        fontDropdownTemplateBackground.color = skin.settingsMenuDropdowns.templateBackgroundColor;
        fontDropdownItemBackground.color = skin.settingsMenuDropdowns.itemBackgroundColor;
        
        skyboxMaterialDropdown.colors = skin.settingsMenuDropdowns.colors.colors;
        skyboxMaterialDropdownArrow.sprite = skin.settingsMenuDropdowns.arrow;
        skyboxMaterialDropdownCheckmark.sprite = skin.settingsMenuDropdowns.checkmark;
        skyboxMaterialDropdownTemplateBackground.color = skin.settingsMenuDropdowns.templateBackgroundColor;
        skyboxMaterialDropdownItemBackground.color = skin.settingsMenuDropdowns.itemBackgroundColor;
        
        skyboxTypeDropdown.colors = skin.settingsMenuDropdowns.colors.colors;
        skyboxTypeDropdownArrow.sprite = skin.settingsMenuDropdowns.arrow;
        skyboxTypeDropdownCheckmark.sprite = skin.settingsMenuDropdowns.checkmark;
        skyboxTypeDropdownTemplateBackground.color = skin.settingsMenuDropdowns.templateBackgroundColor;
        skyboxTypeDropdownItemBackground.color = skin.settingsMenuDropdowns.itemBackgroundColor;

        swapSkyboxesToggle.colors = skin.settingsMenuToggles.colors.colors;
        swapSkyboxesToggleBackground.color = skin.settingsMenuToggles.backgroundColor;
        swapSkyboxesToggleCheckmark.sprite = skin.settingsMenuToggles.checkmark;
    }
}