using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class GrapplingHook : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Transform grappleMuzzle;
    public LayerMask grappleMask;

    [Header("Grapple Settings")]
    public float maxGrappleDistance = 35f;
    public float grappleAssistRadius = 0.75f;

    [Header("Swing Physics")]
    public float swingForce = 35f;
    public float climbSpeed = 18f;
    public float gravityScale = 1.5f;

    [Header("Momentum Control")]
    public float airControl = 10f;
    public float maxSwingSpeed = 25f;

    [Header("Auto Aim Feel")]
    public float snapSpeed = 20f;

    [Header("Hint")]
    public KeyCode hintKey = KeyCode.H;

    private Rigidbody rb;
    private LineRenderer lr;

    private bool isGrappling;
    private bool isReady = false;

    private Vector3 grapplePoint;
    private Vector3 ropeDir;
    private float ropeLength;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
    }

    void OnEnable()
    {
        isReady = false;
        Invoke("ShowHint", 0.5f);
    }

    void ShowHint()
    {
        isReady = true;
        if (PlayerUI.Instance == null) return;
        int timesShown = PlayerPrefs.GetInt("GrappleHintShown", 0);
        if (timesShown < 2)
        {
            PlayerUI.Instance.ShowPersistentText("Hold Right click to grapple, Only On Certain Surfaces though!");
            PlayerPrefs.SetInt("GrappleHintShown", timesShown + 1);
            PlayerPrefs.Save();
        }
    }

    void ShowHintManually()
    {
        if (PlayerUI.Instance == null) return;
        PlayerUI.Instance.ShowPersistentText("Hold Right click to grapple, Only On Certain Surfaces though!");
    }

    void OnDisable()
    {
        CancelInvoke("ShowHint");
        isReady = false;
        StopGrapple();
    }

    void Update()
    {
        if (!isReady) return;

        if (Input.GetKeyDown(hintKey))
            ShowHintManually();

        if (Input.GetMouseButtonDown(1))
            TryStartGrapple();

        if (Input.GetMouseButtonUp(1))
            StopGrapple();

        if (isGrappling)
        {
            HandleRopeInput();
            DrawRope();
        }
    }

    public bool CanGrapple()
    {
        if (!enabled) return false;

        RaycastHit hit;
        return Physics.SphereCast(
            playerCamera.transform.position,
            grappleAssistRadius,
            playerCamera.transform.forward,
            out hit,
            maxGrappleDistance,
            grappleMask
        );
    }

    void FixedUpdate()
    {
        if (!isGrappling) return;

        ApplySwingPhysics();
        LimitSwingSpeed();
    }

    void TryStartGrapple()
    {
        RaycastHit hit;

        Vector3 origin = playerCamera.transform.position;
        Vector3 dir = playerCamera.transform.forward;

        if (Physics.SphereCast(origin, grappleAssistRadius, dir, out hit, maxGrappleDistance, grappleMask))
        {
            isGrappling = true;
            grapplePoint = hit.point;
            ropeLength = Vector3.Distance(transform.position, grapplePoint);
            lr.positionCount = 2;
        }
    }

    void HandleRopeInput()
    {
        ropeDir = (transform.position - grapplePoint).normalized;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = playerCamera.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = playerCamera.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 inputDir = (camRight * h + camForward * v);

        rb.AddForce(inputDir * swingForce, ForceMode.Acceleration);
        rb.AddForce(Vector3.down * gravityScale, ForceMode.Acceleration);
    }

    void ApplySwingPhysics()
    {
        ropeDir = (transform.position - grapplePoint).normalized;

        Vector3 radialVel = Vector3.Project(rb.linearVelocity, ropeDir);
        rb.linearVelocity -= radialVel;

        float currentLength = Vector3.Distance(transform.position, grapplePoint);
        if (currentLength > ropeLength)
            transform.position = grapplePoint + ropeDir * ropeLength;

        if (transform.position.y >= grapplePoint.y)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                Mathf.Min(rb.linearVelocity.y, 0f),
                rb.linearVelocity.z
            );
        }
    }

    void LimitSwingSpeed()
    {
        Vector3 flatVel = rb.linearVelocity;
        if (flatVel.magnitude > maxSwingSpeed)
            rb.linearVelocity = flatVel.normalized * maxSwingSpeed;
    }

    void DrawRope()
    {
        if (!isGrappling || grappleMuzzle == null) return;

        lr.SetPosition(0, grappleMuzzle.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple()
    {
        isGrappling = false;
        lr.positionCount = 0;
    }

    public bool IsGrappling() => isGrappling;
    public Vector3 GetGrapplePoint() => grapplePoint;
}