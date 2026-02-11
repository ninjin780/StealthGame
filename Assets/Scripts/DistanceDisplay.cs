using UnityEngine;
using UnityEngine.UI;

public class DistanceDisplay : MonoBehaviour
{
    public float StartDistance = 0f;
    private Text distanceText;
    public static float CurrentDistance { get; set; }

    void Start()
    {
        distanceText = GetComponent<Text>();
    }

    void FixedUpdate()
    {
        UpdateDistanceText();
    }

    private void UpdateDistanceText()
    {
        distanceText.text = "Distance: " + Mathf.CeilToInt(CurrentDistance) + " m";
    }
}

