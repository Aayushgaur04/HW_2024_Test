using System.Collections;
using UnityEngine;
using TMPro;

public class PlatformTimer : MonoBehaviour
{
    public float platformDuration;  // Set this when the platform is spawned
    private float remainingTime;
    private TextMeshPro timerText;

    void Start()
    {
        remainingTime = platformDuration;

        // Get the TextMeshPro component in the platform
        timerText = GetComponentInChildren<TextMeshPro>();
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;

        // Update the timer text
        if (timerText != null)
        {
            timerText.text = remainingTime.ToString("F2");
        }

        // Destroy the platform when the timer reaches zero
        if (remainingTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
