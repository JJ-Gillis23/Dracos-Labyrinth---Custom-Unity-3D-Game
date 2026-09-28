using UnityEngine;
public class Door:Interactables
{
    public string requiredKeyID; // Unique identifier for the key that opens this door
    private bool isOpen = false; // Track whether the door is open or closed
    private Animator animator; // Reference to the Animator component
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        promptmessage = "Press E to Open Door";
    }

    protected override void Interact()
    {
        if (isOpen)
        {
            return;
        }

        if (Inventory.Instance.HasKey(requiredKeyID))
        {
            Inventory.Instance.UseKey(requiredKeyID); // Remove the key from inventory
            OpenDoor();
        }
        else
        {
            promptmessage = "You need the correct key to open this door";
            Invoke("ResetPrompt", 2.0f);
        }
    }

private void ResetPrompt()
{
    promptmessage = "Press E to Open Door";
}
    private void OpenDoor()
    {
        if(audioSource != null)
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        
        isOpen = true;
        Destroy(gameObject);
    }
} 