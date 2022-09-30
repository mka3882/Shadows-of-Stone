using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemies;
    public GameObject[] overseer;
    public GameObject player;
    public GameObject[] keys;
    //BGM Clips
    public AudioSource BGM_T1;
    public AudioSource BGM_T2;
    public AudioSource BGM_Choir;
    public AudioSource BGM_Chased;
    //Event Clips
    public AudioSource keyPickup;
    public AudioSource enemySpot;
    public AudioSource spotlightHit;
    public AudioSource playerBurst;

    //I have these volumes here so the track looping works nicely, and the chasing and choir music can fade in/out insted of 
    float choirVolume = 0.0f;
    float chasedVolume = 0.0f;

    bool PlayerSpotted = false;
    bool PlayerSpottedLastFrame = false;
    bool overSeenIt = false;
    bool overSeenItLastFrame = false;

    void Start()
    {
        BGM_T1.Play();
        BGM_T2.Play();
        BGM_Choir.Play();
        BGM_Chased.Play();



        enemies = GameObject.FindGameObjectsWithTag("enemy");
        
        overseer = GameObject.FindGameObjectsWithTag("overseer");
        /*
        if (keys == null)//and finally the key(s)
        {
            keys = GameObject.FindGameObjectsWithTag("key");
        }
        */

    }

    // Update is called once per frame
    void Update()
    {
        //update interlude music
        BGM_Choir.volume = choirVolume;
        BGM_Chased.volume = chasedVolume;
        //Player burst sound effect
        if(player.GetComponent<Player>().burstCalled)
        {
            AudioClip AClip = playerBurst.clip;
            playerBurst.PlayOneShot(AClip, 0.75f);
        }
        //key pickup currently disabled

        //Plays Stinger for enemy spotting player and ramps up chase music
        PlayerSpotted = false;
        overSeenIt = false;
        foreach(GameObject enmy in enemies)
        {
            if(enmy.GetComponent<Monster>().playerFound)
            {
                PlayerSpotted = true;
            }
        }
        if(PlayerSpotted && !PlayerSpottedLastFrame)
        {
            AudioClip AClip = enemySpot.clip;
            enemySpot.PlayOneShot(AClip, 0.75f);
        }
        if(PlayerSpotted)
        {
            if(chasedVolume < 1.0f)
            {
                chasedVolume += 0.01f;
            }
        }
        else
        {
            if(chasedVolume > 0f)
            {
                chasedVolume -= 0.01f;
            }
        }
        //deal with overseer spotting player
        foreach (GameObject ovs in overseer)
        {
            if (ovs.GetComponent<Overseer>().spawned)
            {
                overSeenIt = true;
            }
        }
        if (overSeenIt && !overSeenItLastFrame)
        {
            AudioClip AClip = spotlightHit.clip;
            spotlightHit.PlayOneShot(AClip, 0.75f);
            chasedVolume = 8.0f;
        }
        if (overSeenIt)
        {
            if (choirVolume < 1.0f)
            {
                choirVolume += 0.2f;
            }
        }
        else
        {
            if (choirVolume > 0f)
            {
                choirVolume -= 0.01f;
            }
        }

        overSeenItLastFrame = overSeenIt;
        PlayerSpottedLastFrame = PlayerSpotted;
    }
}
