using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log("Pointe click");
        switch (gameObject.name)
        {
            case "resume":
                gameObject.transform.parent.parent.GetComponent<PauseMenu>().Unpause();
                break;
            case "main":
                SceneManager.LoadScene("TitleScreen");
                break;
            case "quit":
                Application.Quit();
                break;
            default:
                SceneManager.LoadScene(gameObject.name);
                break;
        }
    }

  /*  void OnMouseDown()
    {
        Debug.Log("clicked");
        switch (gameObject.name)
        {
            case "resume":
                gameObject.transform.parent.parent.GetComponent<PauseMenu>().Unpause();
                break;
            case "main":
                SceneManager.LoadScene("MainMenu");
                break;
            case "quit":
                Application.Quit();
                break;
        }
    }*/
}
