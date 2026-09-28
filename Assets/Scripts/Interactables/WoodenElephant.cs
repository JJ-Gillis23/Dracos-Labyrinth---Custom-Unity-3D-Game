using UnityEngine;
public class WoodenElephant : Interactables
{
    private bool isPickedUp = false;

    void Start()
    {
        promptmessage = "Press E to Pick Up The Secret Item";
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
        StatTracker.Instance.ItemCollected();
        Destroy(gameObject);
    }
}