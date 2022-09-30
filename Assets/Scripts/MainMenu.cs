using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
   //     Camera.main.orthographicSize = (20.0f / Screen.width * Screen.height / 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        
    }

    void OnMouseDown()
    {
        if (gameObject.name == "start")
        {
            SceneManager.LoadScene("Intro");
        }
        else if (gameObject.name == "levelSelect")
        {
            SceneManager.LoadScene("LevelSelect");
        }
        else if(gameObject.name == "quit")
        {
            Application.Quit();
        }else if (gameObject.name == "tutorial")
        {
            GameObject.FindGameObjectWithTag("tutorial").GetComponent<Tutorial>().StartTutorial();
        }else if (gameObject.name == "main")
        {
            SceneManager.LoadScene("TitleScreen");
        }
        else
        {
            SceneManager.LoadScene(gameObject.name);
        }
    }
}
