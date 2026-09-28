using UnityEngine;

public class JumpingBoots : Interactables
{
    public float jumpForce = 30f;
    protected override void Interact()
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if(player != null)
        {
            player.jumpForce = jumpForce;
            PlayerUI.Instance.ShowAchievement("Jumping Boots equipped!");
        }
        Destroy(gameObject);
    }
}