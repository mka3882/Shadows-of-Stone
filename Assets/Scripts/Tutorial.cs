using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    GameObject[] images;
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AdvanceTutorial();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RetreatTutorial();
        }
    }

    public void AdvanceTutorial()
    {
        gameObject.transform.GetChild(index).gameObject.SetActive(false);
        if (index != 8)
        {
            index++;
            gameObject.transform.GetChild(index).gameObject.SetActive(true);
        }
        else
        {
            index = 0;
        }
    }

    public void RetreatTutorial()
    {
        if (index != 0)
        {
            gameObject.transform.GetChild(index).gameObject.SetActive(false);
            index--;
            gameObject.transform.GetChild(index).gameObject.SetActive(true);
        }
    }

    public void StartTutorial()
    {
        gameObject.transform.GetChild(index).gameObject.SetActive(true);
    }
}
