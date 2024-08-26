using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    private bool playerOnPlatform = false;

    void Start()
    {
        // Reset the flag when the platform is spawned
        playerOnPlatform = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerOnPlatform)
        {
            // The player successfully reached the platform
            playerOnPlatform = true;

            // Increment the score only once when the player reaches the platform
            ScoreManager.instance.IncrementScore();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Allow scoring again when the player leaves the platform (for future platforms)
            playerOnPlatform = false;
        }
    }
}
