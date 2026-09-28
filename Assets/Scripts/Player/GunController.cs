using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public static GunController Instance;
    public GameObject projectilePrefab;
    public Vector3 offset = new Vector3(0.6f, 0.3f, 0);
    public float maxDistance = 500f;
    public Image crosshair;
    private Camera playerCamera;
    private AudioSource audioSource;
    private GameObject currentProjectile;
    private int layerMask;

    private GrapplingHook grapplingHook;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        if (playerCamera == null)
            playerCamera = Camera.main;
        crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        layerMask = ~LayerMask.GetMask("Player");
        grapplingHook = GetComponent<GrapplingHook>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return; // paused, ignore all input
        RaycastHit hit;
        bool gunHit = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore);

        // Crosshair priority: blue (grapple in range) > green (gun hit) > red (nothing)
        if (grapplingHook != null && grapplingHook.CanGrapple())
            crosshair.color = Color.purple;
        else if (gunHit)
            crosshair.color = Color.green;
        else
            crosshair.color = Color.red;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (gunHit)
            {
                if (audioSource != null)
                    audioSource.Play();

                if (currentProjectile != null)
                    Destroy(currentProjectile);

                Vector3 spawnPos = hit.point + hit.normal * 0.1f;
                Quaternion spawnRot = Quaternion.LookRotation(-hit.normal);
                currentProjectile = Instantiate(projectilePrefab, spawnPos, spawnRot);
                currentProjectile.tag = "Projectile";
                currentProjectile.transform.SetParent(hit.collider.transform);

                Rigidbody rb = currentProjectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    StartCoroutine(DelayedHit(rb));
                }

                Destroy(currentProjectile, 5.0f);
            }
        }
    }

    IEnumerator DelayedHit(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.25f);
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = -rb.transform.forward * 0.1f;
        }
    }
}