using UnityEngine;

public class Key : Interactables
{
    public string keyID;
    private bool canPickup = false;
    private bool isDropped = false;
    private float startY;
    public int uses = 1; // set in Inspector    

    void Start()
    {
        promptmessage = "";
        Invoke("EnablePickup", 0.5f);
        Invoke("StartSpinning", 1f);
    }

    void StartSpinning()
    {
        isDropped = true;
        Animator anim = GetComponentInChildren<Animator>();
        if (anim != null)
            anim.enabled = false;
        startY = transform.position.y;
    }

    void EnablePickup()
    {
        canPickup = true;
    }

    void Update()
    {
        if (isDropped)
        {
            transform.Rotate(0, 0, 90f * Time.deltaTime); // Rotate around Z-axis
            transform.position = new Vector3(transform.position.x, startY + Mathf.Sin(Time.time) * 0.1f, transform.position.z);
        }
    }
    private bool isCollected = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canPickup && !isCollected)
        {
            isCollected = true;
            Interact();
        }
    }

        protected override void Interact()
        {
            if (!canPickup || isCollected == false) return;
            Inventory.Instance.AddKey(keyID, uses);

            if (gameObject.transform.parent.CompareTag("KeyHolder"))
                gameObject.transform.parent.gameObject.SetActive(false); // was Destroy
            else
                gameObject.SetActive(false); // was Destroy

            PlayerUI.Instance.ShowAchievement("*Key Added to Inventory*");
        }
void OnEnable()
{
    isCollected = false;
    canPickup = false;
    Invoke("EnablePickup", 0.5f);
    Invoke("StartSpinning", 1f);
}
    
}