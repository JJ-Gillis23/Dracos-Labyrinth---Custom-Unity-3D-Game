using UnityEngine;
using System.Collections;

public class Quicksand : MonoBehaviour
{
    public float sinkSpeed = 1f;
    public Vector3 respawnPoint;
    public float sinkDepth = 2f;

    private bool isSinking = false;
    private Rigidbody playerRb;
    private int groundLayer;
    private float sinkStartY;
    private GameObject playerObj;

    void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isSinking)
        {
            playerObj = other.gameObject;
            playerRb = other.GetComponent<Rigidbody>();
            sinkStartY = playerRb.position.y;
            isSinking = true;
            Physics.IgnoreLayerCollision(other.gameObject.layer, groundLayer, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isSinking)
        {
            // Only cancel if they haven't sunk far enough to respawn
            if (playerRb.position.y > sinkStartY - sinkDepth)
            {
                Physics.IgnoreLayerCollision(other.gameObject.layer, groundLayer, false);
                playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);
                isSinking = false;
                playerRb = null;
            }
        }
    }

    void Respawn()
    {
        playerRb.linearVelocity = Vector3.zero;
        playerRb.position = respawnPoint;
        isSinking = false;
        Physics.IgnoreLayerCollision(playerObj.layer, groundLayer, false);
        playerRb = null;
    }

    void Update()
    {
        if (!isSinking || playerRb == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Physics.IgnoreLayerCollision(playerRb.gameObject.layer, groundLayer, false);
            playerRb.linearVelocity = new Vector3(0, 5f, 0);
            isSinking = false;
            playerRb = null;
            return;
        }

        playerRb.linearVelocity = new Vector3(
            playerRb.linearVelocity.x * 0.3f,
            -sinkSpeed,
            playerRb.linearVelocity.z * 0.3f
        );

        if (playerRb.position.y < sinkStartY - sinkDepth)
            Respawn();
    }
}