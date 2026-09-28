using UnityEngine;

public class SprintingBoots : Interactables
{
    protected override void Interact()
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if(player != null)
        {
            player.sprintSpeed = 20;
            PlayerUI.Instance.ShowAchievement("Sprinting Boots equipped!");
        }
        Destroy(gameObject);
    }
}