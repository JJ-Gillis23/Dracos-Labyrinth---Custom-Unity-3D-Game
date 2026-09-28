using UnityEngine;

/// <summary>
/// FPS Camera Controller
/// 
/// Setup:
///   1. Attach this script to your Player GameObject.
///   2. Assign your Camera (child of Player) to the 'playerCamera' field in the Inspector.
///   3. Add a CharacterController component to the Player GameObject.
///   4. Set the Player's capsule collider/CharacterController height to match your character.
///
/// The camera handles vertical (pitch) rotation.
/// The player body handles horizontal (yaw) rotation.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class FPSCameraController : MonoBehaviour
{
    [Header("Mouse Look")]
    [Tooltip("Mouse sensitivity for horizontal and vertical look.")]
    public float mouseSensitivity = 100f;

    [Tooltip("Clamp the vertical look angle (degrees up/down).")]
    public float verticalClampAngle = 80f;

    [Tooltip("Invert the vertical axis.")]
    public bool invertY = false;

    [Header("Gravity")]
    [Tooltip("Gravity strength applied to the player. Unity default is -9.81.")]
    public float gravity = -9.81f;

    [Header("References")]
    [Tooltip("The Camera child object that will pitch up/down.")]
    public Camera playerCamera;

    // Accumulated vertical rotation (pitch)
    private float verticalRotation = 0f;

    // Vertical velocity accumulator for gravity
    private float verticalVelocity = 0f;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock and hide the cursor for FPS feel
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerCamera == null)
        {
            // Fallback: try to find the camera in children
            playerCamera = GetComponentInChildren<Camera>();
        }
    }

    void Update()
    {
        HandleGravity();
        HandleMouseLook();
        HandleCursorToggle();
    }

    /// <summary>
    /// Applies gravity via the CharacterController each frame.
    /// </summary>
    private void HandleGravity()
    {
        if (characterController.isGrounded)
        {
            // Small constant downward force keeps isGrounded reliable on slopes/steps
            verticalVelocity = -2f;
        }
        else
        {
            // Accumulate gravity over time (v = v0 + a*t)
            verticalVelocity += gravity * Time.deltaTime;
        }

        characterController.Move(new Vector3(0f, verticalVelocity * Time.deltaTime, 0f));
    }

    /// <summary>
    /// Rotates the player body left/right and the camera up/down based on mouse input.
    /// </summary>
    private void HandleMouseLook()
    {
        // Read raw mouse delta (Unity scales by Time.deltaTime internally for Input.GetAxis)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (invertY) mouseY = -mouseY;

        // Horizontal rotation: rotate the entire player body (yaw)
        transform.Rotate(Vector3.up * mouseX);

        // Vertical rotation: rotate only the camera (pitch), clamped to avoid over-rotation
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalClampAngle, verticalClampAngle);

        if (playerCamera != null)
            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    /// <summary>
    /// Toggle cursor lock with Escape key (useful during development/menus).
    /// </summary>
    private void HandleCursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}