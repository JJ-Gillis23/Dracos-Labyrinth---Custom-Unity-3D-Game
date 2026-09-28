using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 10f;
    public float sprintSpeed = 15f;
    public float acceleration = 15f;
    public float deceleration = 20f;
    public float jumpForce = 15f;
    public float gravityMultiplier = 3f;
    public float fallMultiplier = 2.5f;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.2f;
    public LayerMask groundMask;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public float verticalClampAngle = 80f;
    public Camera playerCamera;

    [Header("Step Climbing")]
    public float stepHeight = 2f;
    public float stepSmooth = 0.1f;

    private Rigidbody rb;
    private Collider col;
    private float verticalRotation = 0f;
    private bool isGrounded = false;
    private static bool exists = false;
    private GrapplingHook grapplingHook;

    void Awake()
    {
        if (exists)
        {
            Destroy(gameObject);
            return;
        }
        exists = true;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
        DontDestroyOnLoad(gameObject);
        grapplingHook = GetComponent<GrapplingHook>();
    }

    void Update()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalClampAngle, verticalClampAngle);
        if (playerCamera != null)
            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void FixedUpdate()
    {
        // Ground check synced with physics
        float rayLength = col.bounds.extents.y + groundCheckDistance;
        isGrounded = Physics.SphereCast(
            transform.position,
            0.2f,
            Vector3.down,
            out RaycastHit hit,
            rayLength
        );
        Debug.DrawRay(transform.position, Vector3.down * rayLength, isGrounded ? Color.green : Color.red);

        if (grapplingHook != null && grapplingHook.IsGrappling())
            return;

        StepClimb();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && isGrounded;
        float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        if (move.magnitude > 1f) move.Normalize();

        Vector3 targetVelocity = move * targetSpeed;
        Vector3 currentHorizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 velocityDiff = targetVelocity - currentHorizontal;

        float rate = move.magnitude > 0.1f ? acceleration : deceleration;
        float control = isGrounded ? 1f : 0.3f;
        rb.AddForce(velocityDiff * rate * control, ForceMode.Acceleration);

        // Gravity multipliers
        if (!isGrounded)
        {
            if (rb.linearVelocity.y < 0)
                rb.AddForce(Physics.gravity * fallMultiplier, ForceMode.Acceleration);
            else if (rb.linearVelocity.y > 0)
                rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    void StepClimb()
    {
        // Use actual horizontal velocity to determine approach direction
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Fall back to input direction if velocity is too low (just starting to move)
        Vector3 moveDir;
        if (horizontalVelocity.magnitude > 0.5f)
        {
            moveDir = horizontalVelocity.normalized;
        }
        else
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            moveDir = (transform.right * h + transform.forward * v).normalized;
        }

        if (moveDir.magnitude < 0.1f) return;

        RaycastHit hitLower;
        if (Physics.Raycast(
            transform.position - new Vector3(0, col.bounds.extents.y - 0.1f, 0),
            moveDir, out hitLower, 0.6f, groundMask))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(
                transform.position - new Vector3(0, col.bounds.extents.y - stepHeight, 0),
                moveDir, out hitUpper, 0.7f, groundMask))
            {
                // Scale the nudge by how hard you're pushing into the step
                float pushStrength = Mathf.Clamp01(horizontalVelocity.magnitude / walkSpeed);
                float nudge = Mathf.Lerp(stepSmooth * 0.5f, stepSmooth, pushStrength);
                rb.position += new Vector3(0, nudge, 0);
            }
        }
    }
}