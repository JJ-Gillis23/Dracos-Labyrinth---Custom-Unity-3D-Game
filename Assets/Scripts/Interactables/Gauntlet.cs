using UnityEngine;

public class Gauntlet : Interactables
{
    private bool isPickedUp = false;
    
    void Start()
    {
        promptmessage = "Press E to Pick Up Telekinetic Gauntlet";
    }

    protected override void Interact()
    {
        if (!isPickedUp)
            PickUp();
    }

    private void PickUp()
    {
        isPickedUp = true;
        Inventory.Instance.hasGauntlet = true;
        PlayerUI.Instance.ShowAchievement("Telekinetic Gauntlet Equipped!");

        // Find GunController on the Player instead of the Gauntlet
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            GunController gun = player.GetComponent<GunController>();
            if (gun != null)
                gun.enabled = true;
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Transform cameraTransform = GameObject.Find("Main Camera").transform;
        transform.SetParent(cameraTransform);
        
        transform.localPosition = new Vector3(0.3f, -0.3f, .7f);
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        promptmessage = "";

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
    }
}