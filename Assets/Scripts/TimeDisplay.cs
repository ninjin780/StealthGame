using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    public float startTime = 0f;
    private Text timeText;
    public static float CurrentTime { get; set; }

    void Start()
    {
        timeText = GetComponent<Text>();
        CurrentTime = startTime;
        UpdateTimerText();
    }

    void FixedUpdate()
    {
        CurrentTime += Time.deltaTime;
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        timeText.text = "Time: " + Mathf.CeilToInt(CurrentTime) + " sec";
    }
}

