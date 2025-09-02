using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    public TMP_InputField nameInputField;

    public void StartButtonClicked()
    {
        GameManager.instance.playerName = nameInputField.text;
        SceneManager.LoadScene(1);
    }

    public void QuitApp()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif

    }

    public void LeaderboardClicked()
    {
        SceneManager.LoadScene(2);
    }
}