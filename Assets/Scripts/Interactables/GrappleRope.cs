using UnityEngine;

public class GrappleRope : Interactables
{
    private bool isPickedUp = false;

    void Start()
    {
        promptmessage = "Press E to Pick Up the Grappling Rope";
    }

    protected override void Interact()
    {
        if (!isPickedUp)
            PickUp();
    }

    private void PickUp()
    {
        isPickedUp = true;
        PlayerUI.Instance.ShowAchievement("Grappling Rope Equipped");

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        promptmessage = "";

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Enable the grappling hook script on the player
        GrapplingHook grapple = GameObject.FindGameObjectWithTag("Player").GetComponent<GrapplingHook>();
        if (grapple != null)
            grapple.enabled = true;
        Destroy(gameObject);
    }
}