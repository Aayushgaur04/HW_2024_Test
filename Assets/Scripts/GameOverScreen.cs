using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text lastScore;

    public void setup(int score)
    {
        gameObject.SetActive(true);
        lastScore.text = score.ToString();
    }
}
