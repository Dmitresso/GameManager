using UnityEngine;

public class CameraRotation : MonoBehaviour {
    [SerializeField] private float speed = 2f;
    private Vector3 rotation;
    private float timer;

    private void Update() {
        timer = Time.deltaTime;
        rotation.x += speed * timer;
        rotation.y += speed * timer;
        rotation.z += speed * timer;
    }

    private void LateUpdate() {
        transform.eulerAngles = new Vector3(rotation.x, rotation.y, rotation.z);
    }
}