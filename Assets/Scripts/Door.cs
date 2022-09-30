using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private HUDManager hud;
    // Start is called before the first frame update
    void Start()
    {
        hud = GameObject.FindGameObjectWithTag("hud").GetComponent<HUDManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCollide() && hud.HasKey())
        {
            hud.RemoveKey();
            gameObject.SetActive(false);
        }
    }

    private bool PlayerCollide()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Collider[] cols = Physics.OverlapSphere(player.transform.position, .7f);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "door")
            {
                Debug.Log("DoorHit");
                return true;
            }
        }
        return false;
    }
}
