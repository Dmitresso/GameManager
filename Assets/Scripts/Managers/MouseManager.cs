using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EventVector3 : UnityEvent<Vector3> { }

public class MouseManager : MonoBehaviour
{
    public LayerMask clickableLayer;

    public Texture2D pointer;
    public Texture2D target;
    public Texture2D doorway;

    public EventVector3 OnClickEnvironment;

    private bool useDefaultCursor;
    
    private void Start() {
    }

    private void Update() {
        if (useDefaultCursor) {
            Cursor.SetCursor(pointer, new Vector2(16, 16), CursorMode.Auto);
        }
        
        // Raycast into scene
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, clickableLayer.value)) {
            bool door = false;
            if (hit.collider.gameObject.tag == "Doorway") {
                Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                door = true;
            }
            else {
                Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
            }

            // If environment surface is clicked, invoke callbacks.
            if (Input.GetMouseButtonDown(0)) {
                if (door) {
                    Transform doorway = hit.collider.gameObject.transform;
                    OnClickEnvironment.Invoke(doorway.position + doorway.forward * 10);
                }
                else {
                    OnClickEnvironment.Invoke(hit.point);
                }
            }
        }
        else {
            Cursor.SetCursor(pointer, Vector2.zero, CursorMode.Auto);
        }
    }
}