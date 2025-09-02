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

    [System.Serializable]
    public class PlayerData
    {
        public string savedPlayerName;
        public int savedScore;
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

        saveData.savedPlayerName = bestPlayerName;
        saveData.savedScore = bestScore;
        
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(customDir, json);

        Debug.Log("Data saved at: " + customDir);
    }

    public void LoadData()
    {
        string pathToFile = customDir;
        
        if(File.Exists(customDir))
        {
            string json = File.ReadAllText(customDir);
            PlayerData loadData = JsonUtility.FromJson<PlayerData>(json);

            bestPlayerName = loadData.savedPlayerName;
            bestScore = loadData.savedScore;

            Debug.Log("Leaderboard Restored");
        }
    }
}
