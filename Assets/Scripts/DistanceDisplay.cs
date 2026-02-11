using UnityEngine;
using UnityEngine.UI;

public class DistanceDisplay : MonoBehaviour
{
    public float StartDistance = 0f;
    private Text distanceText;
    public static float CurrentDistance { get; set; }
    private Vector3 ultimaPosicion;
    [SerializeField]
    private GameObject player;

    void Start()
    {
        distanceText = GetComponent<Text>();
        CurrentDistance = StartDistance;
        ultimaPosicion = player.transform.position;
    }

    void FixedUpdate()
    {
        float distanciaFrame = Vector3.Distance(player.transform.position, ultimaPosicion);

        CurrentDistance += distanciaFrame;
        ultimaPosicion = player.transform.position;
        PlayerPrefs.SetFloat("Distance", CurrentDistance);

        UpdateDistanceText();
    }

    private void UpdateDistanceText()
    {
        if (distanceText != null)
        {
            distanceText.text = "Distance: " + CurrentDistance.ToString("F2") + " m";
        }
    }
}

