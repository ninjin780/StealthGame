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

        highestScoreDistance = DistanceDisplay.CurrentDistance;
        ChangeText();
    }

    void ChangeText()
    {
        if (textBox != null)
        {
            textBox.text = "Your highest score was " + Mathf.CeilToInt(highestScoreTime).ToString() + " seconds, this run was " + Mathf.CeilToInt(TimeDisplay.CurrentTime).ToString() + " seconds with a distance of " + Mathf.CeilToInt(highestScoreDistance).ToString() + " meters, keep going!";
        }
    }
}
