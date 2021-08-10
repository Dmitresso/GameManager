using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    private static T instance;

    protected void Awake() {
        if (instance != null) Debug.LogError("[Singleton] Trying to instantiate a second instance of " + typeof(T) + " class.");
    }

    protected virtual void OnDestroy() {
        if (instance == this) instance = null;
    }

    public static bool IsInit => instance != null;

    public static T Instance {
        get {
            if (instance == null) {
                instance = (T) FindObjectOfType(typeof(T));

                if (instance == null) {
                    Debug.LogError("[Singleton] An instance of " + typeof(T) + " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
}
