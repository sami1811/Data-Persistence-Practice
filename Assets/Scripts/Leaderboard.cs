using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetRecord()
    {
        string dirToFile = GameManager.instance.customDir;

        File.Delete(dirToFile);

        GameManager.instance.bestScore = 0;
        GameManager.instance.bestPlayerName = null;

        Debug.Log("Leaderboard has been reset.");
    }
}
