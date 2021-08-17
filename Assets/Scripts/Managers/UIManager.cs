using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Animator = UnityEngine.Animator;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Canvas), typeof(StandaloneInputModule), typeof(GraphicRaycaster))]
[RequireComponent(typeof(EventSystem), typeof(AudioSource), typeof(Animator))]
public class UIManager : Singleton<UIManager> {
    public Action<UISkin> SkinChange;
    public Action<TMP_FontAsset> FontChange;
    public Action<List<UISkin>> UpdateSkinDropdownOptions;
    public Action<List<TMP_FontAsset>> UpdateFontDropdownOptions;
    public Action<int> SetSkinDropdownValue; 
    public Action<int> SetFontDropdownValue; 

    [SerializeField, ReadOnly] public string state;
    [SerializeField] public Menu menu;
    [SerializeField] private Camera camera;
    [SerializeField] private AudioSource audio;
    [SerializeField] private Animator animator;
    [SerializeField, Range(1, 3)] private int animatorSpeed = 2;
    [SerializeField] private bool swapSkyboxes;
    [SerializeField] private UISkin selectedSkin;
    [SerializeField] private List<UISkin> skins;
    [SerializeField] private TMP_FontAsset selectedFont;
    [SerializeField] private List<TMP_FontAsset> fonts;

    private CanvasScaler canvasScaler;
    private int inspectorAnimationsSpeed = 2;
    private bool inspectorSwapSkyboxes;
    private UISkin inspectorSkin;
    private TMP_FontAsset inspectorFont;


    
    
    public AudioSource Audio => audio;

    public float Volume {
        get => audio.volume;
        set => audio.volume = value;
        // add inspector/settings menu sync
    }

    public int AnimatorSpeed {
        get => (int) Math.Round(animator.speed);
        set =>
            animator.speed =
                inspectorAnimationsSpeed =
                    menu.MainMenu.AnimationSpeed =
                        menu.SettingsMenu.AnimationSpeed =
                            menu.PauseMenu.AnimationSpeed = value;
    }

    public bool SwapSkyboxes {
        get => animator.GetBool(Data.Animator.Parameters.Id.Bool.SwapSkyboxes);
        set {
            inspectorSwapSkyboxes = swapSkyboxes = value;
            animator.SetBool(Data.Animator.Parameters.Id.Bool.SwapSkyboxes, swapSkyboxes);
        }
    }

    public Vector2 ReferenceResolution {
        get => canvasScaler.referenceResolution;
        set => canvasScaler.referenceResolution = value;
    }
    
    public List<UISkin> Skins {
        get => skins;
        set => skins = value;
    }
    
    public List<TMP_FontAsset> Fonts {
        get => fonts;
        set => fonts = value;
    }
    
    public UISkin SelectedSkin {
        get => selectedSkin;
        set {
            inspectorSkin = selectedSkin = value;
            SkinChange?.Invoke(value);
        }
    }

    public TMP_FontAsset SelectedFont {
        get => selectedFont;
        set {
            inspectorFont = selectedFont = value;
            FontChange?.Invoke(value);
        }
    }


    public static class State {
        public static Animator animator;

        public static bool InMenu {
            get => animator.GetBool(Data.Animator.Parameters.Id.Bool.InMenu);
            set => animator.SetBool(Data.Animator.Parameters.Id.Bool.InMenu, value);
        }
    
        public static bool InSplashScreen {
            get => animator.GetBool(Data.Animator.Parameters.Id.Bool.State.UI.InSplashScreen);
            set => animator.SetBool(Data.Animator.Parameters.Id.Bool.State.UI.InSplashScreen, value);
        }    
    
        public static bool InMain {
            get => animator.GetBool(Data.Animator.Parameters.Id.Bool.State.UI.InMain);
            set => animator.SetBool(Data.Animator.Parameters.Id.Bool.State.UI.InMain, value);
        }
    
        public static bool InPause {
            get => animator.GetBool(Data.Animator.Parameters.Id.Bool.State.UI.InPause);
            set => animator.SetBool(Data.Animator.Parameters.Id.Bool.State.UI.InPause, value);
        }
    
        public static bool InSettings {
            get => animator.GetBool(Data.Animator.Parameters.Id.Bool.State.UI.InSettings);
            set => animator.SetBool(Data.Animator.Parameters.Id.Bool.State.UI.InSettings, value);
        }
    }    



    
    
    private void Awake() {
        Init();

    }

    private void OnDisable() {
        StopAllCoroutines();
        var a = GetComponent<CanvasScaler>().referenceResolution;
    }
    
    
    private void OnValidate() {
        if (inspectorAnimationsSpeed != animatorSpeed) AnimatorSpeed = animatorSpeed;
        if (inspectorSwapSkyboxes != swapSkyboxes) SwapSkyboxes = swapSkyboxes;
        if (inspectorSkin != selectedSkin) SelectedSkin = selectedSkin;
        if (inspectorFont != selectedFont) SelectedFont = selectedFont;
        
        UpdateFontDropdownOptions?.Invoke(Fonts);
        UpdateSkinDropdownOptions?.Invoke(Skins);
    }


    private void Start() {
        AnimatorSpeed = animatorSpeed;
    }

    private void Update() {
        if (!State.InMenu) return;
        if (Input.GetKeyDown(KeyCode.Space)) SetTrigger(Data.Animator.Parameters.Id.Trigger.SpaceBarClick);
    }


    private void Init() {
        State.animator = GetComponent<Animator>();        
        canvasScaler = GetComponent<CanvasScaler>();
        StartCoroutine(InitFonts());
        StartCoroutine(InitSkins());
    }
    

    private IEnumerator InitSkins(UISkin skin = null) {
        yield return new WaitUntil(() => DataInit.IsInit);
        if (skins.Count > 0) {
            var skins = new List<UISkin>(this.skins);
            skins.RemoveAll(skin => skin is null);
            if (skin is null) {
                var index = Random.Range(0, skins.Capacity - 1);
                SelectedSkin = skins[index];
                SetSkinDropdownValue?.Invoke(index);
                if (Tracker.IsInit) Tracker.Instance.Log($"[UIManager] '{SelectedSkin}' skin was automatically selected from 'skins' list.");
            }
            else if (skins.Contains(skin)) {
                SelectedSkin = skin;
                SetSkinDropdownValue?.Invoke(skins.IndexOf(SelectedSkin));
                if (Tracker.IsInit) Tracker.Instance.Log($"[UIManager] '{SelectedSkin.name}' skin is found and selected.");
            }
            else {
                SelectedSkin = skins[0];
                if (Tracker.IsInit) Tracker.Instance.Log($"[UIManager] '{SelectedSkin.name}' was selected as first not null skin in 'skins' list.");
            }
        }
        else {
            skins.AddRange(new []{ Res.Skins.lightSkin, Res.Skins.darkSkin });
            SelectedSkin = skins[0];
            if (Tracker.IsInit) Tracker.Instance.Log($"[UIManager] There's empty 'skins' list. '{Res.Skins.lightSkin.name}' and " +
                                                     $"'{Res.Skins.darkSkin.name}' skins added, '{SelectedSkin.name}' set as default.");
        }
    }
    
    private IEnumerator InitFonts(TMP_FontAsset font = null) {
        yield return new WaitUntil(() => DataInit.IsInit);
        if (fonts.Count > 0) {
            var fonts = new List<TMP_FontAsset>(this.fonts);
            fonts.RemoveAll(font => font is null);
            if (font is null) {
                var index = Random.Range(0, fonts.Capacity - 1);
                SelectedFont = fonts[index];
                SetFontDropdownValue?.Invoke(index);
                if (Tracker.IsInit) Tracker.Instance.Log($"[UIManager] '{SelectedFont.name}' font was automatically selected from 'fonts' list.");
            }
            else if (fonts.Contains(font)) {
                SelectedFont = font;
                SetFontDropdownValue?.Invoke(fonts.IndexOf(SelectedFont));
                if (Tracker.IsInit) Tracker.Instance.Log($"[UIManager] '{SelectedFont.name}' font is found and selected.");
            }
            else {
                SelectedFont = fonts[0];
                if (Tracker.IsInit) Tracker.Instance.Log($"[UIManager] '{SelectedFont.name}' was selected as first font in 'fonts' list.");
            }
        }
        else {
            fonts.Add(Res.Fonts.arialSDF);
            SelectedFont = fonts[0];
            if (Tracker.IsInit) Tracker.Instance.Log($"[UIManager] There's empty 'fonts' list. {Res.Fonts.arialSDF.name} font was added and selected as default.");
        }
    }

    public void SetCameraActive(bool active) {
        camera.gameObject.SetActive(active);
    }
    
    public void SetTrigger(int trigger) {
        animator.SetTrigger(trigger);
        if (Tracker.IsInit) Tracker.Instance.Log($"Animator trigger {trigger} was set.");
    }

    public void ResetTrigger(int trigger) {
        animator.ResetTrigger(trigger);
        if (Tracker.IsInit) Tracker.Instance.Log($"Animator trigger {trigger} was reset.");
    }
    
    


    public void CallAnimationEventFunction(string methodName) {
        Data.Animator.Events.Functions.TryGetValue(methodName, out var method);
        if (method is null) {
            Debug.Log($"[UIManager] There's no '{methodName}' method in Data.Animator.Events.Functions dictionary.");
            return;
        }
        method.Invoke();
        if (Tracker.IsInit) Tracker.Instance.Log($"Animator event function {method} was called.");
    }
}