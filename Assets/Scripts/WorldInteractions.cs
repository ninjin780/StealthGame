using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldInteractions : MonoBehaviour
{
    private void OnExit()
    {
       Application.Quit();
    }

    private void OnStartGame()
    {
        if (PlayerPrefs.GetFloat("Time") <= 2)
        {
            PlayerPrefs.SetFloat("Time", 1300000000);
        }
        SceneManager.LoadScene("Gameplay");
    }
}