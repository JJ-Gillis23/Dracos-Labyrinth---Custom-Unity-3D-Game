using UnityEngine;

public abstract class Interactables : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string promptmessage;

    public void baseInteract()
    {
        Interact();
    }
    protected virtual void Interact()
    {
        
    }
}
