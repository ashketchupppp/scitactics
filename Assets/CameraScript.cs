using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    InputAction iDragAction;
    Camera iCamera;

    void Start() {
        iCamera = GetComponent<Camera>();
        iDragAction = InputSystem.actions.FindAction("DragCamera");
    }

    void Update() {
        if (iDragAction.IsPressed()) {
            Vector2 mouseMove = Pointer.current.delta.ReadValue();
            Vector3 cameraTranslation = new Vector3(mouseMove.x, mouseMove.y, 0);
            iCamera.transform.Translate(cameraTranslation);
        }
    }
}
