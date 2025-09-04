using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public string playerName;
    public int playerScore;
    public string bestPlayerName;
    public int bestScore;

    public string customDir = "E:/Unity/Unity Projects/Unity Version Control/Data-Persistence-Practice/SavedGame/Leaderboard.json";

    private int playerListLength = 5;

    [System.Serializable]
    public class PlayerData
    {
        public List<string> savedPlayerName = new List<string>();
        public List<int> savedScore = new List<int>();
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Directory.CreateDirectory(Path.GetDirectoryName(customDir));

        if (string.IsNullOrEmpty(customDir))
        {
            customDir = Path.Combine(Application.dataPath, "Leaderboard.json");
        }

        LoadData();
    }

    public void SaveData()
    {
        PlayerData saveData = new PlayerData();

        if(File.Exists(customDir))
        {
            string existingJson = File.ReadAllText(customDir);
            PlayerData existingData = JsonUtility.FromJson<PlayerData>(existingJson);

            if(existingData.savedPlayerName != null && existingData.savedScore != null)
            {
                saveData.savedPlayerName = existingData.savedPlayerName;
                saveData.savedScore = existingData.savedScore;
            }
        }

        int playerExists = saveData.savedPlayerName.IndexOf(playerName);

        if(playerExists >= 0)
        {
            if(playerScore > saveData.savedScore[playerExists])
            {
                saveData.savedScore[playerExists] = playerScore;
                Debug.Log(playerName + " got the new record: " + playerScore);
            }
        }

        else
        {
            if(saveData.savedScore.Count < playerListLength)
            {
                saveData.savedPlayerName.Add(playerName);
                saveData.savedScore.Add(playerScore);
                Debug.Log(playerName + " is added to the list with the score of " + playerScore);
            }
            else
            {
                int minScore = saveData.savedScore[0];
                int minIndex = 0;

                for(int i = 1; i < saveData.savedScore.Count; i++)
                {
                    if (saveData.savedScore[i] < minScore)
                    {
                        minScore = saveData.savedScore[i];
                        minIndex = i;
                    }
                }

                if(playerScore <= minScore)
                {
                    Debug.Log("Score is too low to be updated");
                    return;
                }

                else
                {
                    string removedPlayerName = saveData.savedPlayerName[minIndex];

                    saveData.savedScore.RemoveAt(minIndex);
                    saveData.savedPlayerName.RemoveAt(minIndex);

                    saveData.savedPlayerName.Add(playerName);
                    saveData.savedScore.Add(playerScore);

                    Debug.Log(playerName + " took " + removedPlayerName + "'s place and listed is updated");
                }
            }
        }

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(customDir, json);

        Debug.Log("leaderboard is updated and saved at :" + customDir);
    }

    public void LoadData()
    {
        if(File.Exists(customDir))
        {
            string json = File.ReadAllText(customDir);
            PlayerData loadData = JsonUtility.FromJson<PlayerData>(json);

            if (loadData.savedScore.Count > 0)
            {
                int maxScore = loadData.savedScore[0];
                int maxIndex = 0;

                for (int i = 1; i < loadData.savedScore.Count; i++)
                {
                    if (loadData.savedScore[i] > maxScore)
                    {
                        maxScore = loadData.savedScore[i];
                        maxIndex = i;
                    }
                }

                bestScore = maxScore;
                bestPlayerName = loadData.savedPlayerName[maxIndex];

                Debug.Log("Top Player Restored");
            }
        }
    }
}
