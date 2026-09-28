using UnityEngine;

public class InstantKeyTarget : Interactables
{
    public string keyID;
    public int keyUses = 1;

    protected override void Interact()
    {
        Inventory.Instance.AddKey(keyID, keyUses);
        PlayerUI.Instance.ShowAchievement("*Key Added to Inventory*");
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            Interact();
        }
    }
}