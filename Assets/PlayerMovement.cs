using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1f;

    [Header("Camera")]
    public Transform cameraTransform;
    public float mouseSensitivity = 1.5f;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // -------- MOVEMENT --------
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;
        }

        if (input.magnitude > 1f)
            input.Normalize();

        // Move relative to where you're facing (ignore vertical tilt)
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = right * input.x + forward * input.y;
        transform.Translate(move * speed * Time.deltaTime, Space.World);

        // -------- MOUSE LOOK --------
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            float mouseX = mouseDelta.x * mouseSensitivity * 0.1f;
            float mouseY = mouseDelta.y * mouseSensitivity * 0.1f;

            // Vertical rotation (camera only)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -89f, 89f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Horizontal rotation (player body)
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}