using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Arrow : MonoBehaviour, IPointerDownHandler
{
    public bool left;

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
        Debug.Log("Clicked");
        Tutorial tutorial = GameObject.FindGameObjectWithTag("tutorial").GetComponent<Tutorial>();
        if (left)
        {
            tutorial.RetreatTutorial();
        }
        else
        {
            tutorial.AdvanceTutorial();
        }
    }

    void OnMouseDown()
    {
        Debug.Log("mouse clicked");
    }
}
