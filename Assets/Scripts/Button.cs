using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject wall;
    bool onTopOf = false;
    bool firstOnTopOf = true;
    bool firstExit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerCollide();
        if(firstOnTopOf && onTopOf)
        {
            wall.transform.Rotate(new Vector3(0, 90, 0));
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
            firstOnTopOf = false;
            firstExit = true;
        }
        if(!onTopOf && firstExit)
        {
           gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
            firstExit = false;
        }
    }

    private bool PlayerCollide()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (gameObject.GetComponent<BoxCollider>().bounds.Intersects(player.GetComponent<SphereCollider>().bounds))
        {
            Debug.Log("Button Collision");
            onTopOf = true;
            return true;
            //  Debug.Log("Key Collision");
        }
        onTopOf = false;
        firstOnTopOf = true;
        return false;
    }
}
