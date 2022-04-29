using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField]Text nameText;

    private void Start()
    {
        
    }

    public void NewGame()
    {
        MainManager.instance.SetCurrentName(nameText.text);
        SceneManager.LoadScene(1);
    }

    public void loadScene(int sceneID)
    {
        //Debug.Log("Loading " + SceneManager.GetSceneByBuildIndex(sceneID).name);
        SceneManager.LoadScene(sceneID);
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
