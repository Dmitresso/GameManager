using System.Collections;
using System.Collections.Generic;
using System.IO;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEditor;


public class GameManager : Singleton<GameManager> {
    [SerializeField, ReadOnly] public string state;
    [SerializeField] private bool pausable = true;
    [SerializeField] private GameObject[] SystemPrefabs;

    private string currentLevel = string.Empty;
    private List<GameObject> instancedSystemPrefabs;
    private List<AsyncOperation> loadOperations;
    
    private static string currentGameState;
    private static string currentUIState;
    
    private static string GameState {
        get => currentGameState;
        set => currentGameState = GameManager.Instance.state = value;
    }

    private static string UIState {
        get => currentUIState;
        set => currentUIState = UIManager.Instance.state = value;
    }


    public static bool InGame {
        get;
        set;
    }
    
    public static bool Loading {
        get;
        set;
    }

    public static bool Waiting {
        get;
        set;
    }

    public static bool Running {
        get;
        set;
    }
    
    
    
    private void OnDisable() {
        StopAllCoroutines();
    }

    protected void Awake() {
        Init();
    }

    private void Start() {
        UpdateState(State.UI.InSplashscreen);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) PressPause();
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        foreach (var go in instancedSystemPrefabs) Destroy(go);
        instancedSystemPrefabs.Clear();
    }
    
    
    
    private void Init() {
        instancedSystemPrefabs = new List<GameObject>();
        loadOperations = new List<AsyncOperation>();
        InstantiateSystemPrefabs();

        DataInit.Init();
    }

    private void PauseGame() {
        UpdateState(State.UI.InPause);
    }
    
    private void ResumeGame() {
        UpdateState(State.UI.Disabled);
    }

    private void InstantiateSystemPrefabs() {
        foreach (var go in SystemPrefabs) {
            var prefabInstance = Instantiate(go);
            instancedSystemPrefabs.Add(prefabInstance);
        }
        UpdateState(State.Game.Running);
    }


    private void OnLoadOperationComplete(AsyncOperation ao) {
        if (loadOperations.Contains(ao)) {
            loadOperations.Remove(ao);
            if (loadOperations.Count == 0) UpdateState(State.Game.Running);
        }
        Debug.Log("Loading operation completed.");
    }

    private void OnUnloadOperationComplete(AsyncOperation ao) {
        Debug.Log("Unloading operation completed.");
    }

    private IEnumerator ExitApp() {
        SkyboxManager.Instance.ResetSkyboxMaterialsExposure();
        yield return new WaitForSeconds(1);

        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
        Application.Quit();
    }
    
    private  void UpdateState(int i) {
        switch (i) {
            case 0:
                GameState = State.Game.Loading.ToString();
                break;
            case 1:
                GameState = State.Game.Waiting.ToString();
                break;
            case 2:
                GameState = State.Game.Running.ToString();
                break;
            case 3:
                UIState = State.UI.Disabled.ToString();
                UIManager.State.InMenu = false;
                UIManager.State.InSplashScreen = false;
                UIManager.State.InMain = false;
                UIManager.State.InSettings = false;
                UIManager.State.InPause = false;
                break;
            case 4:
                UIState = State.UI.InSplashscreen.ToString();
                UIManager.State.InMenu = true;
                UIManager.State.InSplashScreen = true;
                UIManager.State.InMain = false;
                UIManager.State.InSettings = false;
                UIManager.State.InPause = false;
                break;
            case 5:
                UIState = State.UI.InMain.ToString();
                UIManager.State.InMenu = true;
                UIManager.State.InSplashScreen = false;
                UIManager.State.InMain = true;
                UIManager.State.InSettings = false;
                UIManager.State.InPause = false;
                break;
            case 6:
                UIState = State.UI.InSettings.ToString();
                UIManager.State.InMenu = true;
                UIManager.State.InSplashScreen = false;
                UIManager.State.InMain = false;
                UIManager.State.InSettings = true;
                UIManager.State.InPause = false;
                break;
            case 7:
                UIState = State.UI.InPause.ToString();
                UIManager.State.InMenu = true;
                UIManager.State.InSplashScreen = false;
                UIManager.State.InMain = false;
                UIManager.State.InSettings = false;
                UIManager.State.InPause = true;
                break;
        }
    }
    
    public void UpdateState(State.Game gameState) {
        UpdateState((int) gameState);
    }

    public void UpdateState(State.UI uiState) {
        UpdateState((int) uiState);
    } 
    
    
    
    public void PressPause() {
        if (!pausable) return;
        if (UIState == State.UI.InPause.ToString()) ResumeGame();
        else PauseGame();
    }

    public string GetSceneNameByIndex(int index) {
        return Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(index));
    }

    public void LoadScene(string sceneName) {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (ao == null) {
            Debug.LogError("[GameManager] Unable to load scene \"" + sceneName + "\".");
            return;
        }
        ao.completed += OnLoadOperationComplete;
        loadOperations.Add(ao);
        currentLevel = sceneName;
    }

    public void UnloadScene(string sceneName) {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(sceneName);
        if (ao == null) {
            Debug.LogError("[GameManager] Unable to unload scene \"" + sceneName + "\".");
            return;
        }
        ao.completed += OnUnloadOperationComplete;
    }

    public IEnumerator StartGame() {
        LoadScene(Scenes.Main);
        yield return new WaitForSeconds(1);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(Scenes.Main));
        UnloadScene(Scenes.Boot);
    }

    public IEnumerator RestartGame() {
        Debug.Log("Main unloaded.");
        if (GetComponent<DontDestroyOnLoad>().IsDontDestroyOnLoad) {
            foreach (var go in gameObject.scene.GetRootGameObjects()) {
                Debug.Log($"[GameManager] Destroying object {go.name}");
                if (go.name != gameObject.name) Destroy(go);
            }
        }

        LoadScene(Scenes.Boot);
        yield return new WaitForSeconds(0.5f);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(Scenes.Boot));
        yield return new WaitForSeconds(0.5f);
        UnloadScene(Scenes.Main);
        Destroy(gameObject);
    }

    public void ExitGame() {
        StartCoroutine(ExitApp());
    }
}