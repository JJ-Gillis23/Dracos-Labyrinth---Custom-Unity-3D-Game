using UnityEngine;

public class TeleportPickup : Interactables
{
    private bool isPickedUp = false;

    void Start()
    {
        promptmessage = "Press E to Pick Up Teleport Tool";
    }

    protected override void Interact()
    {
        if (!isPickedUp)
            PickUp();
    }

    private void PickUp()
    {
        isPickedUp = true;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        promptmessage = "";

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Enable the teleport tool on the player
        TeleportTool teleport = GameObject.FindGameObjectWithTag("Player").GetComponent<TeleportTool>();
        if (teleport != null)
            teleport.enabled = true;
        Destroy(gameObject);
    }
}