using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCollide())
        {
            GameObject.FindGameObjectWithTag("hud").GetComponent<HUDManager>().AcquireKey();
            gameObject.SetActive(false);
        }
    }

    private bool PlayerCollide()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (gameObject.GetComponent<BoxCollider>().bounds.Intersects(player.GetComponent<SphereCollider>().bounds)){
            return true;
          //  Debug.Log("Key Collision");
        }
        return false;
    }
}
