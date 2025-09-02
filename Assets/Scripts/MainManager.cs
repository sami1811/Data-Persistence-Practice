using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public Rigidbody Ball;
    public Text ScoreText;
    public Text bestScoreText;
    public GameObject GameOverText;
    public GameObject backButton;
    
    private bool m_Started = false;
    private int m_Points;
    private string currentPlayerName;
    private string bestPlayerName;

    public int currentBestScore = 0;
    public int LineCount = 6;

    private bool m_GameOver = false;

    private void Awake()
    {
        backButton.SetActive(false);
        currentBestScore = GameManager.instance.bestScore;
        bestPlayerName = GameManager.instance.bestPlayerName;
        currentPlayerName = GameManager.instance.playerName;


    }

    // Start is called before the first frame update
    public void Start()
    {
        if(currentBestScore <= 0)
        {
            bestPlayerName = currentPlayerName;
        }

        bestScoreText.text = $"Best Score : {bestPlayerName} : {currentBestScore}";
        ScoreText.text = $"{currentPlayerName} : {m_Points}";

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            GameManager.instance.LoadData();

            currentBestScore = GameManager.instance.bestScore;
            bestPlayerName = GameManager.instance.bestPlayerName;

            bestScoreText.text = $"Best Score : {bestPlayerName} : {currentBestScore}";

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{currentPlayerName} : {m_Points}";

        if(m_Points > currentBestScore)
        {
            currentBestScore = m_Points;
            GameManager.instance.bestScore = currentBestScore;
            GameManager.instance.bestPlayerName = currentPlayerName;
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        backButton.SetActive(true);
        GameManager.instance.SaveData();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
