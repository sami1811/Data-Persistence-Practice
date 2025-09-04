using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public List<TMP_Text> rankTexts = new List<TMP_Text>();
    public List<TMP_Text> nameTexts = new List<TMP_Text>();
    public List<TMP_Text> scoreTexts = new List<TMP_Text>();

    private void Awake()
    {
        LoadLeaderboard();
    }

    public void LoadLeaderboard()
    {
        string fileDir = GameManager.instance.customDir;

        if (!File.Exists(fileDir))
        {
            Debug.Log("No records found.");
            
            for (int i = 0; i < rankTexts.Count; i++)
            {
                rankTexts[i].text = "";
                nameTexts[i].text = "";
                scoreTexts[i].text = "";
            }

            return;
        }

        string json = File.ReadAllText(fileDir);
        GameManager.PlayerData sortingRanks = JsonUtility.FromJson<GameManager.PlayerData>(json);

        if(sortingRanks.savedPlayerName.Count == 0)
        {
            Debug.Log("Leaderboard is empty.");
            return;
        }

        var playerScore = new List<(string name, int score)>();

        for (int i = 0; i < sortingRanks.savedPlayerName.Count; i++)
        {
            playerScore.Add((sortingRanks.savedPlayerName[i], sortingRanks.savedScore[i]));
        }

        playerScore.Sort((a , b) => b.score.CompareTo(a.score));

        for (int i = 0; i < playerScore.Count && i < rankTexts.Count && i < scoreTexts.Count && i < nameTexts.Count; i++)
        {
            rankTexts[i].text = $"{i + 1}.";
            nameTexts[i].text = playerScore[i].name;
            scoreTexts[i].text = playerScore[i].score.ToString();
        }

        Debug.Log("Leaderboard loaded sucessfully!");
    }

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
        
        LoadLeaderboard();
    }
}
