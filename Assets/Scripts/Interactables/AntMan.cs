using UnityEngine;

public class AntMan : MonoBehaviour
{
    public KeyCode shrinkKey = KeyCode.Q;
    public KeyCode growKey = KeyCode.F;
    public KeyCode hintKey = KeyCode.H;
    public Vector3 cameraShrinkOffset = new Vector3(0f, -0.35f, 0f);

    private const float shrinkFactor = 0.3f;

    private Vector3 originalScale;
    private bool isShrunk = false;
    private bool isReady = false;
    private Collider col;
    private PlayerController playerController;
    private GameObject gauntletObject;
    private Transform cameraTransform;
    private Vector3 cameraOriginalPosition;

    private void Awake()
    {
        originalScale = transform.localScale;
        col = GetComponent<Collider>();
        playerController = GetComponent<PlayerController>();

        Camera cam = GetComponentInChildren<Camera>();
        if (cam != null)
        {
            cameraTransform = cam.transform;
            cameraOriginalPosition = cam.transform.localPosition;
        }
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
        int timesShown = PlayerPrefs.GetInt("AntManHintShown", 0);
        if (timesShown < 2)
        {
            PlayerUI.Instance.ShowPersistentText($"Press {shrinkKey} to shrink, {growKey} to grow back!");
            PlayerPrefs.SetInt("AntManHintShown", timesShown + 1);
            PlayerPrefs.Save();
        }
    }

    void ShowHintManually()
    {
        if (PlayerUI.Instance == null) return;
        PlayerUI.Instance.ShowPersistentText($"Press {shrinkKey} to shrink, {growKey} to grow back!");
    }

    void OnDisable()
    {
        CancelInvoke("ShowHint");
        isReady = false;
    }

    void TryFindGauntlet()
    {
        if (gauntletObject != null) return;
        Gauntlet gauntlet = GetComponentInChildren<Gauntlet>(true);
        if (gauntlet != null)
        {
            gauntletObject = gauntlet.gameObject;
        }
    }

    private void Update()
    {
        if (!isReady) return;

        if (Input.GetKeyDown(hintKey))
            ShowHintManually();

        if (Input.GetKeyDown(shrinkKey))
        {
            if (!isShrunk)
                Shrink();
        }
        else if (Input.GetKeyDown(growKey))
        {
            if (isShrunk)
            {
                if (HasRoomToGrow())
                    Grow();
                else
                    PlayerUI.Instance.ShowPersistentText("Not enough room to grow here!");
            }
        }
    }

    bool HasRoomToGrow()
    {
        float checkHeight = originalScale.y * 2f;
        return !Physics.Raycast(transform.position, Vector3.up, checkHeight);
    }

    void Shrink()
    {
        TryFindGauntlet();
        transform.localScale = originalScale * shrinkFactor;
        isShrunk = true;

        if (cameraTransform != null)
            cameraTransform.localPosition = cameraOriginalPosition + cameraShrinkOffset;

        if (gauntletObject != null)
            gauntletObject.SetActive(false);

        if (playerController != null)
        {
            playerController.jumpForce = 5f;
            playerController.sprintSpeed = 10f;
            playerController.walkSpeed = 7f;
        }
    }

    void Grow()
    {
        transform.localScale = originalScale;
        isShrunk = false;

        if (cameraTransform != null)
            cameraTransform.localPosition = cameraOriginalPosition;

        if (gauntletObject != null)
            gauntletObject.SetActive(true);

        if (playerController != null)
        {
            playerController.jumpForce = 15f;
            playerController.sprintSpeed = 15f;
            playerController.walkSpeed = 10f;
        }
    }

    public void ResetToNormal()
    {
        transform.localScale = originalScale;
        isShrunk = false;

        if (cameraTransform != null)
            cameraTransform.localPosition = cameraOriginalPosition;

        if (gauntletObject != null)
            gauntletObject.SetActive(true);

        if (playerController != null)
        {
            playerController.jumpForce = 15f;
            playerController.sprintSpeed = 15f;
            playerController.walkSpeed = 10f;
        }
    }
}