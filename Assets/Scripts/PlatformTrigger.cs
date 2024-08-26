using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    private bool playerOnPlatform = false;

    void Start()
    {
        playerOnPlatform = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerOnPlatform)
        {
            playerOnPlatform = true;

            ScoreManager.instance.IncrementScore();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }
}
