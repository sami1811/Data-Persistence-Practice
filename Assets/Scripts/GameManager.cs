using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public string playerName;
    public string bestPlayerName;
    public int bestScore;

    public string customDir = "E:/Unity/Unity Projects/Unity Version Control/Data-Persistence-Practice/SavedGame/Leaderboard.json";

    private int playerListLength = 4;

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

        for (int i = 0; i < playerListLength; i++)
        {
            if (saveData.savedScore.Count > 0)
            {
                return;
            }
            else
            {
                saveData.savedPlayerName[i] = bestPlayerName;
                saveData.savedScore[i] = bestScore;
                
                string json = JsonUtility.ToJson(saveData);
                File.WriteAllText(customDir, json);

                Debug.Log( i+1 + ": " + bestPlayerName + " scored " + bestScore + " and data is saved at " + customDir);
            }
        }
    }

    public void LoadData()
    {
        string pathToFile = customDir;
        
        if(File.Exists(customDir))
        {
            string json = File.ReadAllText(customDir);
            PlayerData loadData = JsonUtility.FromJson<PlayerData>(json);

            if (loadData.savedScore.Count > 0)
            {
                int tempScore = loadData.savedScore[0];
                string tempName = loadData.savedPlayerName[0];

                for (int i = 1; i < loadData.savedScore.Count; i++)
                {
                    if (loadData.savedScore[i] > tempScore)
                    {
                        tempScore = loadData.savedScore[i];
                        tempName = loadData.savedPlayerName[i];
                    }
                }

                bestScore = tempScore;
                bestPlayerName = tempName;

                Debug.Log("Top Player Restored");
            }
        }
    }
}
