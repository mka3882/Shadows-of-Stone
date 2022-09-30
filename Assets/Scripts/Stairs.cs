    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stairs : MonoBehaviour
{
    public string SceneToLoad;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneToLoad == "")
        {
            //comment out when more scenes
            SceneToLoad = SceneManager.GetActiveScene().name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCollide())
        {
            SceneManager.LoadScene(SceneToLoad);
        }
    }

    private bool PlayerCollide()
    {
        if (gameObject.GetComponent<BoxCollider>().bounds.Intersects(GameObject.FindGameObjectWithTag("Player").GetComponent<SphereCollider>().bounds)){
            return true;
        }
        return false;
    }
}
