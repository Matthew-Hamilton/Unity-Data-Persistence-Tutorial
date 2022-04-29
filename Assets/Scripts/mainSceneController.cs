using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainSceneController : MonoBehaviour
{
    MainManager mainManager;
    // Start is called before the first frame update
    void Start()
    {
        mainManager = MainManager.instance;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MainManager.GameSetup();
            Debug.Log("Setup");
        }
    }
}
