using UnityEngine;

public class Chest : Interactables
{
    private Animator animator;
    private bool isOpen = false;
    public GameObject objectPrefab;
    public GameObject chestparticles;
    private AudioSource audioSource;
    public float rotationx = 0f;

    public float rotationy = 0f;

    public float rotationz = 0f;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        promptmessage = "Press E to Open Chest";
    }

    protected override void Interact()
    {
        if (!isOpen)
        {
            audioSource.Play();
            animator.SetTrigger("Open");
            isOpen = true;
            GameObject particles = Instantiate(chestparticles, transform.position + Vector3.up, Quaternion.identity);
            Destroy(particles, 2.0f);
            GameObject tool = Instantiate(objectPrefab, transform.position + Vector3.up, Quaternion.Euler(rotationx, rotationy, rotationz));
            if(objectPrefab.CompareTag("Gauntlet"))
            {
            tool.transform.localScale = Vector3.one * 0.5f;
            }
            GetComponent<BoxCollider>().enabled = false;
            promptmessage = "";
            Destroy(gameObject, 2.0f);
        }
    }
}