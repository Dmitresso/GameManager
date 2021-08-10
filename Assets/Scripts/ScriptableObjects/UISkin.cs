using UnityEngine;


[CreateAssetMenu(fileName = "New UISkin", menuName = "ScriptableObjects/UISkin/Simple Skin", order = 2)]
public class UISkin : ScriptableObject {
    [Header("Common")]
    public Color controlsTextColor;
    
    [Space][Space]
    [Header("Main Menu")]
    public Color mainMenuBackgroundColor;
    public ButtonContainer mainMenuButtons;
    
    [Space][Space]
    [Header("Settings Menu")]
    public Color settingsMenuTextColor;
    public Color32 settingsMenuTextOutlineColor;
    [Range(0, 1)] public float settingsMenuTextOutlineWidth = 0.1f;

    public ButtonContainer goBackButton;
    public ButtonContainer settingsMenuButtons;
    public DropdownContainer settingsMenuDropdowns;
    public SliderContainer settingsMenuSliders;
    public ToggleContainer settingsMenuToggles;

    [Space][Space]
    [Header("Pause Menu")]
    public Color pauseMenuBackgroundColor;
    public Color pauseMenuTextColor;
    public ButtonContainer pauseMenuButtons;

    [Space][Space]
    [Header("Splash Screen")]
    public Color splashScreenBackgroundColor;
    public Color titleTextColor;
    public Color taglineTextColor;

    

    private void Awake() {
    }

    private void OnValidate() {
    }
    
    
    
}
