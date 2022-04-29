using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] Text[] highScores = new Text[5];
    MainManager mainManager;
    // Start is called before the first frame update
    void Start()
    {
        mainManager = MainManager.GetInstance();
        for (int i = 0; i< 5; i++)
            highScores[i].text = MainManager.GetScore()[i];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
