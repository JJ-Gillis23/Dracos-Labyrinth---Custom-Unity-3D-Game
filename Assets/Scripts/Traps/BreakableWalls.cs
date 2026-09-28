using UnityEngine;
using System.Collections;

public class BreakableWall : MonoBehaviour
{
    public float respawnDelay = 5f;

    private Collider[] wallColliders;
    private Renderer[] wallRenderers;
    private bool broken;

    void Awake()
    {
        wallColliders = GetComponentsInChildren<Collider>(true);
        wallRenderers = GetComponentsInChildren<Renderer>(true);
        SetWallActive(true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!broken && collision.gameObject.CompareTag("Projectile"))
        {
            StartCoroutine(BreakAndRespawn());
        }
    }

    IEnumerator BreakAndRespawn()
    {
        broken = true;
        SetWallActive(false);
        yield return new WaitForSeconds(respawnDelay);
        SetWallActive(true);
        broken = false;
    }

    void SetWallActive(bool active)
    {
        foreach (var col in wallColliders)
            col.enabled = active;

        foreach (var rend in wallRenderers)
            rend.enabled = active;
    }
}