using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text bestScore;

    public Text nameText;
    private string m_Name;
    private bool m_Started = false;
    int m_Points = 0;
    
    private bool m_GameOver = false;

    static ScoreBoard m_Board;

 

    // Start is called before the first frame update
    void Awake()
    {
        m_Board = new ScoreBoard();
        
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;

        LoadScore();
        SaveScore();


    }

    void GenerateBlock()
    {
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
            }
        }
    }

    public void SetCurrentName(string name)
    {
        m_Name = name;
    }

    private void Update()
    {
        
    }

    void InitializeObjects()
    {
        Ball = GameObject.Find("Ball").GetComponent<Rigidbody>();
        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        bestScore = GameObject.Find("bestScore").GetComponent<Text>();
        if (GameOverText = GameObject.Find("GameoverText"))
            GameOverText.active = false;
        bestScore.text = "HIGH SCORE: " + m_Board.getName(0) + " - " + m_Board.getScore(0);
    }

    public static void GameSetup()
    {

        if (!instance.m_Started)
        {

            instance.InitializeObjects();
            instance.GenerateBlock();
                instance.m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                instance.Ball.transform.SetParent(null);
                instance.Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);

        }
        else if (instance.m_GameOver)
        {

                SceneManager.LoadScene(0);
                instance.m_Started=false;

                instance.m_GameOver = false;
        }
    }

    public static MainManager GetInstance()
    {
        return instance;
    }

    public int AddPoint(int point)
    {
        m_Points += point;
        Debug.Log("As Added: " + m_Points);
        ScoreText.text = $"Score : {m_Points}";
        return point;
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        m_Board.DebugLog();
        SaveScore();
    }

    public static string[] GetScore()
    {
        string[] scores = new string[5];
        for(int i = 0; i < scores.Length; i++)
            scores[i] = (m_Board.getName(i) + " - " + m_Board.getScore(i));
        return scores;
    }

    public void AddToScoreboard()
    {
        m_Board.AddToScoreBoard(m_Points, m_Name);
    }


    class ScoreBoard
    {
        static int[] scores = new int[5] {0,0,0,0,0};
        static string[] playerNames = new string[5] {"Unknown", "Unknown", "Unknown", "Unknown", "Unknown" };

        public void DebugLog()
        {
            for(int i = 0; i < scores.Length; i++)
            {
                Debug.Log("Name: " + playerNames[i] + " - Score: " +scores[i]);
            }
        }
        public void AddToScoreBoard(int scoreInput, string playerNameInput)
        {
            Debug.Log("Called Points:" + scoreInput);
            for (int i = 0; i <scores.Length;i++)
            {
                if(scoreInput > scores[i])
                {
                    for(int j = 3; j > i-1;j--)
                    {
                        scores[j+1] = scores[j];
                        playerNames[j+1] = playerNames[j];
                    }
                    scores[i] = scoreInput;
                    playerNames[i] = playerNameInput;
                    Debug.Log("Added");
                    return;

                }
            }
        }

        public string getName(int index)
        {
            return playerNames[index];
        }

        public int getScore(int index)
        {
            return scores[index];
        }
    }


    [System.Serializable]
    class SaveData
    {
        public string first;
        public string second;
        public string third;
        public string fourth;
        public string fifth;
        public int firstScore;
        public int secondScore;
        public int thirdScore;
        public int fourthScore;
        public int fifthScore;

    }

    void SaveScore()
    {
        SaveData data = new SaveData();
        data.firstScore = m_Board.getScore(0);
        data.first = m_Board.getName(0);
        data.secondScore = m_Board.getScore(1);
        data.second = m_Board.getName(1);
        data.thirdScore = m_Board.getScore(2);
        data.third = m_Board.getName(2);
        data.fourthScore = m_Board.getScore(3);
        data.fourth = m_Board.getName(3);
        data.fifthScore = m_Board.getScore(4);
        data.fifth = m_Board.getName(4);
        Debug.Log(Application.persistentDataPath + "/savedata.json");
        File.WriteAllText(Application.persistentDataPath + "/savedata.json",JsonUtility.ToJson(data));
        Debug.Log("Scores Saved");

    }


    void LoadScore()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if(File.Exists(path))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
            Debug.Log(saveData.first);
            m_Board.AddToScoreBoard(saveData.firstScore, saveData.first);
            m_Board.AddToScoreBoard(saveData.secondScore, saveData.second);
            m_Board.AddToScoreBoard(saveData.thirdScore, saveData.third);
            m_Board.AddToScoreBoard(saveData.fourthScore, saveData.fourth);
            m_Board.AddToScoreBoard(saveData.fifthScore, saveData.fifth);
            Debug.Log("Loaded Scoreboard"); 
            return;
        }
    }

}
