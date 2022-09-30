using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public bool lit;

    // Start is called before the first frame update
    void Start()
    {
        if (lit)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LightTorch()
    {
        if (!lit)
        {
            lit = true;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
