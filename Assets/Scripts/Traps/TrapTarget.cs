using UnityEngine;

public class TrapTarget : MonoBehaviour
{
    public GameObject Trap;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            SpawnLaunchPlatform();
            Destroy(gameObject);
        }
    }

    void SpawnLaunchPlatform()
    {
        PlayerUI.Instance.UpdatePersistentText(string.Empty);
        PlayerUI.Instance.UpdatePromptText(string.Empty);
        PlayerUI.Instance.ShowPersistentText("Look out! You've set off a trap!");

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 spawnPos = new Vector3(
                player.transform.position.x,
                player.transform.position.y - 1f,
                player.transform.position.z
            );

            // Face the platform in the same direction the player is facing
            // so transform.forward on LaunchPlatform points the same way
            Quaternion spawnRot = Quaternion.Euler(0f, player.transform.eulerAngles.y, 0f);
            GameObject platform = Instantiate(Trap, spawnPos, spawnRot);
            Destroy(platform, 5f);
        }
    }
}