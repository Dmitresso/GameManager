using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

using Data;
using UnityEditor;


public class GameManager : Singleton<GameManager> {
    [SerializeField, ReadOnly] private string state;
    [SerializeField] private bool pausable = true;
    [SerializeField] private GameObject[] SystemPrefabs;

    private string currentLevel = string.Empty;
    private List<GameObject> instancedSystemPrefabs;
    private List<AsyncOperation> loadOperations;
    
    
    private string CurrentGameState {
        get => state;
        set => state = value;
    }

    
    private void OnDisable() {
        StopAllCoroutines();
    }

    protected void Awake() {
        Init();
    }

    private void Start() {
        UpdateState(GameState.Menu.InSplashscreen);
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
        UpdateState(GameState.Menu.InPause);
    }
    
    private void ResumeGame() {
        UpdateState(GameState.Game.Running);
    }

    private void InstantiateSystemPrefabs() {
        foreach (var go in SystemPrefabs) {
            var prefabInstance = Instantiate(go);
            instancedSystemPrefabs.Add(prefabInstance);
        }
    }


    private void OnLoadOperationComplete(AsyncOperation ao) {
        if (loadOperations.Contains(ao)) {
            loadOperations.Remove(ao);
            if (loadOperations.Count == 0) UpdateState(GameState.Game.Running);
        }
        Debug.Log("Loading Completed.");
    }

    private void OnUnloadOperationComplete(AsyncOperation ao) {
        Debug.Log("Unloading Completed.");
    }

    private IEnumerator ExitApp() {
        SkyboxManager.Instance.ResetSkyboxMaterialsExposure();
        yield return new WaitForSeconds(1);

        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
        Application.Quit();
    }


    public void UpdateState(int i) {
        switch (i) {
            case 0: UpdateState(GameState.Game.Loading); break;
            case 1: UpdateState(GameState.Game.Waiting); break;
            case 2: UpdateState(GameState.Game.Running); break;
            case 3: UpdateState(GameState.Menu.InSplashscreen); break;
            case 4: UpdateState(GameState.Menu.InMain); break;
            case 5: UpdateState(GameState.Menu.InSettings); break;
            case 6: UpdateState(GameState.Menu.InPause); break;
        }
    }
    
    public void UpdateState(GameState.Game state) {
        var previousGameState = CurrentGameState;
        CurrentGameState = state.ToString();
        if (Tracker.IsInit) Tracker.Instance.LogStateTransitionOpInfo(previousGameState, CurrentGameState);

        if (CurrentGameState == GameState.Menu.InMain.ToString()) UIManager.Instance.InMain = false;
        else if (CurrentGameState == GameState.Menu.InPause.ToString()) UIManager.Instance.InPause = false;
            
        UIManager.Instance.InGame = true;
        UIManager.Instance.InMenu = false;
        UIManager.Instance.InSplashScreen = false;
        UIManager.Instance.InMain = false;
        UIManager.Instance.InPause = false;
        UIManager.Instance.InSettings = false;
        switch (state) {
            case GameState.Game.Loading:
                break;
            case GameState.Game.Waiting:
                break;
            case GameState.Game.Running:
                UIManager.Instance.Running = true;
                break;
        }
        
        //OnGameStateChanged?.Invoke(CurrentGameState, previousGameState);
    }

    public void UpdateState(GameState.Menu state) {
        var previousGameState = CurrentGameState;
        CurrentGameState = state.ToString();
        if (Tracker.IsInit) Tracker.Instance.LogStateTransitionOpInfo(previousGameState, CurrentGameState);

        UIManager.Instance.InGame = false;
        UIManager.Instance.InMenu = true;
        switch (state) {
            case GameState.Menu.InSplashscreen:
                UIManager.Instance.InSplashScreen = true;
                UIManager.Instance.InMain = false;
                UIManager.Instance.InPause = false;
                UIManager.Instance.InSettings = false;
                break;
            case GameState.Menu.InMain:
                UIManager.Instance.InSplashScreen = false;
                UIManager.Instance.InMain = true;
                UIManager.Instance.InPause = false;
                UIManager.Instance.InSettings = false;
                break;
            case GameState.Menu.InPause:
                UIManager.Instance.InSplashScreen = false;
                UIManager.Instance.InMain = false;
                UIManager.Instance.InPause = true;
                UIManager.Instance.InSettings = false;
                break;
            case GameState.Menu.InSettings:
                UIManager.Instance.InSplashScreen = false;
                UIManager.Instance.InMain = false;
                UIManager.Instance.InPause = false;
                UIManager.Instance.InSettings = true;
                break;
        }
    }
    
    public void PressPause() {
        if (!pausable) return;
        if (CurrentGameState == GameState.Menu.InPause.ToString()) ResumeGame();
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

    public void StartGame() {
        UpdateState(GameState.Game.Running);
        LoadScene(Scenes.Main);
    }

    public void RestartGame() {
        UpdateState(GameState.Game.Loading);
    }

    public void ExitGame() {
        StartCoroutine(ExitApp());
    }
}