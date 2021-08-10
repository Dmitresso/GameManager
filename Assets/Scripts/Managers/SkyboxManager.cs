using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class SkyboxManager : Singleton<SkyboxManager> {
    private const float minSwapRate = 10f, maxSwapRate = 120f;
    [SerializeField, Range(minSwapRate, maxSwapRate)] private float swapRate = minSwapRate;
    private const float minFadeTime = 0.25f, maxFadeTime = 2f;
    [SerializeField, Range(minFadeTime, maxFadeTime)] private float fadeTime = 1f;
    [SerializeField] private Skybox selectedSkyboxType;
    [SerializeField] private Material selectedSkyboxMaterial;
    [SerializeField] private List<Material> lightSkyboxes;
    [SerializeField] private List<Material> darkSkyboxes;


    private static readonly int exposure = Shader.PropertyToID("_Exposure");
    private Skybox currentSkyboxType, nextSkyboxType, iSelectedSkyboxType;
    private List<Material> skyboxes;
    private Material currentSkyboxMaterial, nextSkyboxMaterial, iSelectedSkyboxMaterial;
    private int selectedSkyboxIndex;
    public bool SkyboxFadeCompleted { get; set; }


    private float CurrentExposure { get; set; }

    public List<Material> Skyboxes {
        get => skyboxes;
        set => skyboxes = value;
    }

    public List<Material> LightSkyboxes {
        get => lightSkyboxes;
        set => lightSkyboxes = value;
    }

    public List<Material> DarkSkyboxes {
        get => darkSkyboxes;
        set => darkSkyboxes = value;
    }

    public Skybox SelectedSkyboxType {
        get => selectedSkyboxType;
        set {
            iSelectedSkyboxType = selectedSkyboxType = value;
            
            skyboxes.Clear();
            switch (value) {
                case Skybox.Light: skyboxes.AddRange(LightSkyboxes); break;
                case Skybox.Dark: skyboxes.AddRange(DarkSkyboxes); break;                
                case Skybox.Random: skyboxes.AddRange(LightSkyboxes); skyboxes.AddRange(DarkSkyboxes); break;
            }
        }
    }
    
    public Material SelectedSkyboxMaterial {
        get => selectedSkyboxMaterial;
        private set {
            iSelectedSkyboxMaterial = selectedSkyboxMaterial = value;
            RenderSettings.skybox = value;
        }
    }

    public int SelectedSkyboxIndex {
        get => selectedSkyboxIndex;
        set => selectedSkyboxIndex = value;
    }

    public float SwapRate {
        get => swapRate;
        set => swapRate = value;
    }

    
    
    
    private void Awake() {
        Init();
    }

    private void OnDisable() {
        StopAllCoroutines();
    }


    private void OnValidate() {
        //if (UIManager.IsInit) ResetAllSkyboxesList();
        if (iSelectedSkyboxType != selectedSkyboxType) SelectedSkyboxType = selectedSkyboxType;
        if (iSelectedSkyboxMaterial != selectedSkyboxMaterial) SelectedSkyboxMaterial = selectedSkyboxMaterial;

        if (!Application.isPlaying) return;
        var opts = new List<Material>(Skyboxes);
        opts.RemoveAll(skybox => skybox is null);
        if (UIManager.IsInit) UIManager.Instance.menu.SettingsMenu.SkyboxMaterialDropdownInteractable = opts.Count > 1;
    }


    private void Init() {
        skyboxes = new List<Material>();
        SelectedSkyboxType = selectedSkyboxType;
        var randomMaterial = GetRandomSkyboxMaterial(SelectedSkyboxType);
        randomMaterial.SetFloat(exposure, 0);
        SelectedSkyboxMaterial = randomMaterial;
        StartCoroutine(FadeSkybox(Fade.Out, 3));
        
        Skyboxes = new List<Material>();
        switch (SelectedSkyboxType) {
            case Skybox.Light: Skyboxes.AddRange(LightSkyboxes); break;
            case Skybox.Dark: Skyboxes.AddRange(DarkSkyboxes); break;
            case Skybox.Random: Skyboxes.AddRange(LightSkyboxes); Skyboxes.AddRange(DarkSkyboxes); break;
        }
    }

    private Material GetRandomSkyboxMaterial(Skybox skybox = Skybox.Random) {
        do {
            switch (skybox) {
                case Skybox.Light:
                    SelectedSkyboxIndex = Random.Range(0, lightSkyboxes.Capacity - 1);
                    nextSkyboxMaterial = lightSkyboxes[SelectedSkyboxIndex];
                    break;
                case Skybox.Dark:
                    SelectedSkyboxIndex = Random.Range(0, darkSkyboxes.Capacity - 1);
                    nextSkyboxMaterial = darkSkyboxes[SelectedSkyboxIndex];
                    break;
                case Skybox.Random:
                    nextSkyboxMaterial = Random.Range(0, 2) * 2 - 1 > 0
                        ? lightSkyboxes[SelectedSkyboxIndex = Random.Range(0, lightSkyboxes.Capacity - 1)]
                        : darkSkyboxes[SelectedSkyboxIndex = Random.Range(0, darkSkyboxes.Capacity - 1)];
                    break;
            }
        } while (nextSkyboxMaterial == currentSkyboxMaterial);
        return nextSkyboxMaterial;
    }

    public IEnumerator ChangeSkyboxWithFade(Skybox skyboxType) {
        StartCoroutine(FadeSkybox(Fade.In));
        yield return new WaitUntil(() => SkyboxFadeCompleted);
        SkyboxFadeCompleted = false;
        
        ChangeSkybox(skyboxType, skyboxes[Random.Range(0, skyboxes.Capacity - 1)]);
        StartCoroutine(FadeSkybox(Fade.Out));
        yield return new WaitUntil(() => SkyboxFadeCompleted);
        SkyboxFadeCompleted = false;
    }
    
    public IEnumerator ChangeSkyboxWithFade(Skybox skyboxType, int nextSkyboxIndex) {
        SkyboxFadeCompleted = false;
        StartCoroutine(FadeSkybox(Fade.In));
        yield return new WaitUntil(() => SkyboxFadeCompleted);
        SkyboxFadeCompleted = false;
        if (nextSkyboxIndex < 0 || nextSkyboxIndex >= skyboxes.Capacity) nextSkyboxIndex = Random.Range(0, skyboxes.Capacity - 1);
        var nextSkybox = skyboxes[nextSkyboxIndex];
        
        ChangeSkybox(skyboxType, nextSkybox);
        StartCoroutine(FadeSkybox(Fade.Out));
        yield return new WaitUntil(() => SkyboxFadeCompleted);
        SkyboxFadeCompleted = false;
    }
    
    
    public void ChangeSkybox(Skybox skyboxType = Skybox.Random, Material nextSkyboxMaterial = null) {
        currentSkyboxMaterial = SelectedSkyboxMaterial;

        var nextSkybox = nextSkyboxMaterial is null ? GetRandomSkyboxMaterial(skyboxType) : nextSkyboxMaterial;

        nextSkybox.SetFloat(exposure, 0);
        SelectedSkyboxMaterial = nextSkybox;
        currentSkyboxMaterial.SetFloat(exposure, 1);
    }

    public IEnumerator FadeSkybox(Fade fade, float fadeTime) {
        float startAlpha = 0f, endAlpha = 0f;
        
        switch (fade) {
            case Fade.In:
                startAlpha = 1;
                endAlpha = 0;
                break;
            case Fade.Out:
                startAlpha = 0;
                endAlpha = 1;
                break;
        }

        var elapsedTime = 0f;
        while (elapsedTime < fadeTime) {
            elapsedTime += Time.deltaTime;
            CurrentExposure = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / fadeTime));
            RenderSettings.skybox.SetFloat(exposure, CurrentExposure);
            yield return new WaitForEndOfFrame();
        }
        SkyboxFadeCompleted = true;
    }

    public IEnumerator FadeSkybox(Fade fade) {
        float startAlpha = 0f, endAlpha = 0f;
        var trigger = 0;
        
        switch (fade) {
            case Fade.In:
                startAlpha = 1;
                endAlpha = 0;
                trigger = Data.Animator.Parameters.Id.Trigger.SkyboxFadeInCompleted;
                break;
            case Fade.Out:
                startAlpha = 0;
                endAlpha = 1;
                trigger = Data.Animator.Parameters.Id.Trigger.SkyboxFadeOutCompleted;
                break;
        }

        var elapsedTime = 0f;
        while (elapsedTime < fadeTime) {
            elapsedTime += Time.deltaTime;
            CurrentExposure = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / fadeTime));
            RenderSettings.skybox.SetFloat(exposure, CurrentExposure);
            yield return new WaitForEndOfFrame();
        }
        UIManager.Instance.SetTrigger(trigger);
        SkyboxFadeCompleted = true;
    }
    
    public void ResetSkyboxMaterialsExposure() {
        foreach (var material in skyboxes) material.SetFloat(exposure, 1);
    }
}