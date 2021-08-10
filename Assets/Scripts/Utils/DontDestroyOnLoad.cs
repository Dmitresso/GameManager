using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
    [SerializeField] private bool dontDestroyOnLoad = true;

    private void Awake() {
        if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
    }
}
