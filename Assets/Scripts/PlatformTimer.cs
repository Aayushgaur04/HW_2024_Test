using UnityEngine;
using TMPro;

public class PlatformTimer : MonoBehaviour
{
    public float platformDuration;
    private float remainingTime;
    private TextMeshPro timerText;

    void Start()
    {
        remainingTime = platformDuration;
        timerText = GetComponentInChildren<TextMeshPro>();
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = remainingTime.ToString("F2");
        }

        if (remainingTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
