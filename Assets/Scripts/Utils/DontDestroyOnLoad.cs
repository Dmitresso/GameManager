using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
    [SerializeField] private bool dontDestroyOnLoad = true;

    public bool IsDontDestroyOnLoad => dontDestroyOnLoad;

    private void Awake() {
        if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
    }
}
