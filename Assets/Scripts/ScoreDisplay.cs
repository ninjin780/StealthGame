using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI textBox;
    private float highestScoreTime;
    private float highestScoreDistance;

    void Start()
    {
        textBox = GetComponent<TextMeshProUGUI>();

        highestScoreTime = PlayerPrefs.GetFloat("Time") > TimeDisplay.CurrentTime ? TimeDisplay.CurrentTime : PlayerPrefs.GetFloat("Time");
        PlayerPrefs.SetFloat("Time", highestScoreTime);

        highestScoreDistance = PlayerPrefs.GetFloat("Distance") > DistanceDisplay.CurrentDistance ? DistanceDisplay.CurrentDistance : PlayerPrefs.GetFloat("Distance");
        PlayerPrefs.SetFloat("Distance", highestScoreDistance);
        ChangeText();
    }

    void ChangeText()
    {
        if (textBox != null)
        {
            textBox.text = "Your highest score was " + highestScoreTime.ToString() + " seconds with a distance of " + highestScoreDistance.ToString() + " meters, keep going!";
        }
    }
}
