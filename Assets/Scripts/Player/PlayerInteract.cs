using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera playerCamera;
    private float distance = 5.0f;

    private LayerMask mask;
    private PlayerUI playerUI;

    void Start()
    {
        playerCamera = Camera.main;
        playerUI = GetComponent<PlayerUI>();
        mask = LayerMask.GetMask("Interactables");
    }

    void Update()
    {
        playerUI.UpdatePromptText(string.Empty);

        playerUI.UpdateTimerText(
            StatTracker.Instance.GetFormattedTime(StatTracker.Instance.elapsedTime)
        );

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            Interactables interactable = hitInfo.collider.GetComponent<Interactables>();

            if (interactable != null)
            {
                playerUI.UpdatePersistentText(string.Empty);
                playerUI.UpdatePromptText(string.Empty);

                playerUI.UpdatePromptText(interactable.promptmessage);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.baseInteract();
                }
            }
        }
    }
}