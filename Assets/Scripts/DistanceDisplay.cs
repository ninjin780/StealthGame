using UnityEngine;
using UnityEngine.UI;

public class DistanceDisplay : MonoBehaviour
{
    public float StartDistance = 0f;
    private Text distanceText;
    private static float CurrentTime { get; set; }

    void Start()
    {
        distanceText = GetComponent<Text>();
    }

    void FixedUpdate()
    {

    }
}

