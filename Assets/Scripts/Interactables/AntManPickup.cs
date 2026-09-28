using UnityEngine;

public class AntManPickup : Interactables
{
    private bool isPickedUp = false;

    void Start()
    {
        promptmessage = "Press E to Pick Up The Shrink Formula";
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
        AntMan antMan = GameObject.FindGameObjectWithTag("Player").GetComponent<AntMan>();
        if (antMan != null)
            antMan.enabled = true;
        Destroy(gameObject);
    }
}