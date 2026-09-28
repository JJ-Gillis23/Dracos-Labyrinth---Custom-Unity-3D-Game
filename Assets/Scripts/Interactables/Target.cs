using UnityEngine;

public class Target : Interactables
{
    public GameObject key; // assign KeyHolder prefab here
    
    public Barrier[] barriers; // assign Barrier here
    protected override void Interact()
    {
        if(key!=null)
        {
        GameObject spawnedKey = Instantiate(key, transform.position, Quaternion.identity);
        Animator keyAnimator = spawnedKey.GetComponentInChildren<Animator>();
            if (keyAnimator != null)
            {
                keyAnimator.SetTrigger("isDropped");
            }
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            Interact();
            if(barriers != null)
            {
                for(int i = 0; i<barriers.Length; i++)
                {
                barriers[i].LowerBarrier();
                }
            }
        }
    }
}