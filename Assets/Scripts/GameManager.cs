using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public string playerName;
    public string bestPlayerName;
    public int bestScore;

    private string customDir = "E:/Unity/Unity Projects/Unity Version Control/Data-Persistence-Practice/SavedGame/Leaderboard.json";

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public int score;
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
        if(string.IsNullOrEmpty(customDir))
        {
            customDir = Path.Combine(Application.dataPath,"Leaderboard.json");
        }

        LoadData();
    }

    public void SaveData()
    {
        PlayerData saveData = new PlayerData();

        saveData.playerName = bestPlayerName;
        saveData.score = bestScore;

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

            bestPlayerName = loadData.playerName;
            bestScore = loadData.score;

            Debug.Log("Leaderboard Restored");
        }
    }
}
