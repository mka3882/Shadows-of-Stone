using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                paused = true;
            }
            else
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                paused = false;
            }
        }
    }

    public void Unpause()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        paused = false;
    }
}
