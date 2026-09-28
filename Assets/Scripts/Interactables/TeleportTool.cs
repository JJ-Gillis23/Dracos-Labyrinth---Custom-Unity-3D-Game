using UnityEngine;

public class TeleportTool : MonoBehaviour
{
    [Header("Teleport Settings")]
    public KeyCode markKey = KeyCode.Q;
    public KeyCode teleportKey = KeyCode.F;
    public KeyCode hintKey = KeyCode.H;
    public float maxMarkDistance = 5f;
    public LayerMask markMask;
    public GameObject markerPrefab;
    public Camera playerCamera;

    private Vector3 markedPosition;
    private bool hasMarked = false;
    private GameObject currentMarker;
    private Rigidbody rb;
    private bool isReady = false;

    void Start()
    {
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
        rb = GetComponentInParent<Rigidbody>();
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
        int timesShown = PlayerPrefs.GetInt("TeleportHintShown", 0);
        if (timesShown < 2)
        {
            PlayerUI.Instance.ShowPersistentText("Press Q to mark a position, F to teleport!");
            PlayerPrefs.SetInt("TeleportHintShown", timesShown + 1);
            PlayerPrefs.Save();
        }
    }

    void ShowHintManually()
    {
        if (PlayerUI.Instance == null) return;
        PlayerUI.Instance.ShowPersistentText("Press Q to mark a position, F to teleport!");
    }

    void OnDisable()
    {
        CancelInvoke("ShowHint");
        if (currentMarker != null)
            Destroy(currentMarker);
        hasMarked = false;
        isReady = false;
    }

    void Update()
    {
        if (!isReady) return;

        if (Input.GetKeyDown(hintKey))
            ShowHintManually();

        if (Input.GetKeyDown(markKey))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxMarkDistance, markMask))
            {
                if (currentMarker != null)
                    Destroy(currentMarker);

                markedPosition = hit.point + hit.normal * 0.1f;
                hasMarked = true;
                currentMarker = Instantiate(markerPrefab, markedPosition, Quaternion.LookRotation(-hit.normal));
            }
        }

        if (Input.GetKeyDown(teleportKey) && hasMarked)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.position = markedPosition + Vector3.up;
            }
            else
            {
                transform.root.position = markedPosition + Vector3.up;
            }

            Destroy(currentMarker);
            hasMarked = false;
        }
    }
}