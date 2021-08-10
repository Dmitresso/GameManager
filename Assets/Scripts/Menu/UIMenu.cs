using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CanvasGroup),
                 typeof(Animation))]
public class UIMenu : MonoBehaviour {
    private CanvasGroup canvasGroup;
    private Animation animation;
    private List<AnimationState> states;
    private Dictionary<string, AudioClip> buttonsSounds;
    
    protected List<dynamic> controlsText;
    protected List<dynamic> controls;
    protected List<dynamic> texts;

    protected bool initCompleted;

    public float Visibility {
        get => canvasGroup.alpha;
        set => canvasGroup.alpha = value;
    }

    public int AnimationSpeed {
        set { foreach (var state in states) state.speed = value; }
    }

    protected void Awake() {
        Init();
        
        UIManager.Instance.SkinChange += SetSkin;
        UIManager.Instance.FontChange += SetFont;
    }

    private void Start() {
       
    }

    private void Init() {
        StartCoroutine(InitAwaitingResources());

        animation = GetComponent<Animation>();
        states = new List<AnimationState>(animation.Cast<AnimationState>());
        if (Tracker.IsInit) Tracker.Instance.Log("UIMenu is init.");
    }

    private IEnumerator InitAwaitingResources() {
        yield return new WaitUntil(() => DataInit.IsInit);
        buttonsSounds = new Dictionary<string, AudioClip>(StringComparer.OrdinalIgnoreCase) {
            {"enter", Res.Music.ButtonOnHoverSound},
            {"click", Res.Music.ButtonOnClickSound}
        };
        if (Tracker.IsInit) Tracker.Instance.Log("All data was successfully initialized.");
    }


    protected void InitControlsTexts(List<dynamic> controls, List<dynamic> controlsText) {
        foreach (var control in controls) {
            switch (control) {
                case Button button:
                    controlsText.Add(button.GetComponentInChildren<TextMeshProUGUI>());
                    break;
                case TMP_Dropdown dropdown:
                    controlsText.AddRange(new[] {dropdown.itemText, dropdown.captionText});
                    break;
                case Toggle toggle:
                    if (toggle.GetComponentInChildren<TextMeshProUGUI>() is null)
                        Debug.LogWarning("There's no label with TextMeshProUGUI on toggle.");
                    break;
            }
        }
    }
    
    protected virtual IEnumerator SetMenuSkin(UISkin skin) {
        yield return null;
        // foreach (var container in containers) {
        //     switch (container) {
        //         case ButtonContainer buttonsContainer:
        //             foreach (var (button, _) in buttons) {
        //                 switch (button.transition) {
        //                     case Selectable.Transition.ColorTint:
        //                         button.colors = buttonsContainer.colors.colors;
        //                         break;
        //                     case Selectable.Transition.SpriteSwap:
        //                         button.image.sprite = buttonsContainer.sprites.sprite;
        //                         button.spriteState = buttonsContainer.sprites.spriteState;
        //                         break;
        //                 }
        //             }
        //             break;
        //         case DropdownContainer dropdownContainer:
        //             foreach (var (dropdown, images) in dropdowns) {
        //                 dropdown.colors = dropdownContainer.colors.colors;
        //                 images.arrow.sprite = dropdownContainer.arrow;
        //                 images.templateBackground.color = dropdownContainer.templateBackground;
        //                 images.itemBackground.color = dropdownContainer.itemBackground;
        //                 images.checkmark.sprite = dropdownContainer.checkmark;
        //             }
        //             break;
        //         case SliderContainer sliderContainer:
        //             foreach (var (slider, (background, fill, handle)) in sliders) {
        //                 slider.colors = sliderContainer.colors.colors;
        //                 background.color = sliderContainer.background;
        //                 fill.color = sliderContainer.fill;
        //                 handle.color = sliderContainer.handle;
        //             }
        //             break;
        //         case ToggleContainer toggleContainer:
        //             foreach (var (toggle, checkmark) in toggles) {
        //                 toggle.colors = toggleContainer.colors.colors;
        //                 checkmark.sprite = toggleContainer.checkmark;
        //             }
        //             break;
        //     }
        // }
    }
    
    protected IEnumerator SetMenuFont(TMP_FontAsset font) {
        yield return new WaitUntil(() => initCompleted);
        foreach (var text in texts) text.font = font;
        foreach (var text in controlsText) text.font = font;
    }

    protected virtual void SetSkin(UISkin skin) {
        if (Tracker.IsInit) Tracker.Instance.Log($"{skin.name} was set.");
    }

    protected virtual void SetFont(TMP_FontAsset font) {
        if (Tracker.IsInit) Tracker.Instance.Log($"{font.name} was set.");
    }
    
    public void PlayButtonAnimation(string name) {
        animation.Play(name);
    }

    public void PlayButtonSound(string name) {
        if (buttonsSounds.TryGetValue(name, out var sound)) UIManager.Instance.Audio.PlayOneShot(sound);
    }
}