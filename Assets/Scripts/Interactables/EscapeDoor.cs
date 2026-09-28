using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class EscapeDoor : Interactables
{
    public List<string> requiredKeyIDs; // All keys needed to open this door
    private bool isOpen = false;
    private Animator animator;
    private AudioSource audioSource;
    private float numofKeys;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        promptmessage = "Press E to Open Door";
        numofKeys = requiredKeyIDs.Count;
    }

    protected override void Interact()
    {
        if (isOpen) return;

        // Check if player has all required keys
        foreach (string keyID in requiredKeyIDs)
        {
            if (!Inventory.Instance.HasKey(keyID))
            {
                promptmessage = "This Door requires " + numofKeys + " keys to open";
                Invoke("ResetPrompt", 2.0f);
                return;
            }
        }

        // All keys found — use them all and open
        foreach (string keyID in requiredKeyIDs)
            Inventory.Instance.UseKey(keyID);

        OpenDoor();
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
        StatTracker.Instance.LevelCompleted();
        SceneManager.LoadScene(StatTracker.Instance.level);
    }
}