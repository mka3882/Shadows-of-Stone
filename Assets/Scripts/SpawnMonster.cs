using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : Monster
{
    public Overseer seer;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        mass = 1;
        maxSpeed = 10f;
        seer = GameObject.FindGameObjectWithTag("overseer").GetComponent<Overseer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("pause").GetComponent<PauseMenu>().paused) {
            PlayerSpotted();
            if (stunFrames >= 60)
            {
                stunned = false;
                stunFrames = 0;
            }
            if (!stunned)
            {
                if (playerFound)
                {
                    maxSpeed = 10f;
                    position = transform.position;
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    Vector3 seekSteerForce = Seek(player.GetComponent<Player>().futurePos);
                    ApplyForce(seekSteerForce);
                    var q = Quaternion.LookRotation(player.transform.position - transform.position);
                    transform.rotation = q;
                    velocity += acceleration * Time.deltaTime;
                    position += velocity * Time.deltaTime;
                    if (!WouldCollide(position))
                    {
                        transform.position = position;
                    }
                    acceleration = Vector3.zero;
                }
                else
                {
                    seer.RemoveMonster(gameObject);
                }
            }
            else
            {
                stunFrames++;
            }
        }
    }
}
