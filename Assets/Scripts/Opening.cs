using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject overseer;
    public GameObject player;
    public GameObject door;
    Camera mainCamera;
    bool enemyReached = false;
    float t = 0;
    float timePerDirection = 6f;
    Vector3[] enemyInitPos;
    Vector3 overseerInitPos;
    bool[] reached;
    bool overseerReached = false;
    int resumeCount = 115;
    Vector3 playerTarget;
    Vector3 playerInitPos = Vector3.zero;

    Vector3 enemyTarget = new Vector3(27, 2.43f, -10);

    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 cameraRotation = new Vector3(45.0f, 0.0f, 0.0f);
        Vector3 newCameraPos = new Vector3(player.transform.position.x,
            //remember this is +
            player.transform.position.y + 20,
            //and this is -
            player.transform.position.z - 20);

        mainCamera.transform.position = newCameraPos;
        mainCamera.transform.eulerAngles = cameraRotation;
        enemyInitPos = new Vector3[enemies.Length];
        reached = new bool[enemies.Length];
        for(int i = 0; i < enemies.Length; i++)
        {
            enemyInitPos[i] = enemies[i].transform.position;
        }
        overseerInitPos = overseer.transform.position;
        playerTarget = GameObject.FindGameObjectWithTag("stairs").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(counter == 20)
        {
            Vector3 newPos = mainCamera.transform.position;
            newPos.y += 3;
            mainCamera.transform.position = newPos;
        }else if(counter == 25)
        {
            Vector3 newPos = mainCamera.transform.position;
            newPos.y -= 6;
            mainCamera.transform.position = newPos;
        }else if(counter == 30)
        {
            Vector3 newPos = mainCamera.transform.position;
            newPos.y += 6;
            mainCamera.transform.position = newPos;
        }else if(counter == 35)
        {
            Vector3 newPos = mainCamera.transform.position;
            newPos.y -= 6;
            mainCamera.transform.position = newPos;
        }else if(counter == 40)
        {
            Vector3 newPos = mainCamera.transform.position;
            newPos.y += 6;
            mainCamera.transform.position = newPos;
        }
        else if(counter == 45)
        {
            Vector3 newPos = mainCamera.transform.position;
            newPos.y -= 6;
            mainCamera.transform.position = newPos;
        }else if(counter == 50)
        {
            Vector3 newPos = mainCamera.transform.position;
            newPos.y += 6;
            mainCamera.transform.position = newPos;
        }else if(counter == 55)
        {
            Vector3 newPos = mainCamera.transform.position;
            newPos.y -= 6;
            mainCamera.transform.position = newPos;
        }else if(counter == 60)
        {
            Vector3 newPos = mainCamera.transform.position;
            newPos.y += 3;
            mainCamera.transform.position = newPos;
        }else if(counter == 90)
        {
            foreach(GameObject enemy in enemies)
            {
                Vector3 newPos = enemy.transform.position;
                newPos.y += 1.5f;
                enemy.transform.position = newPos;
            }
        }else if (counter == 95)
        {
            foreach (GameObject enemy in enemies)
            {
                Vector3 newPos = enemy.transform.position;
                newPos.y += 1.5f;
                enemy.transform.position = newPos;
            }
        }else if (counter == 100)
        {
            foreach (GameObject enemy in enemies)
            {
                Vector3 newPos = enemy.transform.position;
                newPos.y -= 1.5f;
                enemy.transform.position = newPos;
            }
        }else if (counter == 105)
        {
            foreach (GameObject enemy in enemies)
            {
                Vector3 newPos = enemy.transform.position;
                newPos.y -= 1.5f;
                enemy.transform.position = newPos;
            }
        }else if (counter >= 115 && !enemyReached)
        {
            t += Time.deltaTime / timePerDirection;

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].transform.position = Vector3.Lerp(enemyInitPos[i], enemyTarget, t);
                var q = Quaternion.LookRotation(enemyTarget - enemies[i].transform.position);
                enemies[i].transform.rotation = q;
                if(Mathf.Abs(enemies[i].transform.position.x - enemyTarget.x) <= .25 && Mathf.Abs(enemies[i].transform.position.z - enemyTarget.z) <= .25)
                {
                    reached[i] = true;
                }
            }
            overseer.transform.position = Vector3.Lerp(overseerInitPos, enemyTarget, t);
            if (Mathf.Abs(overseer.transform.position.x - enemyTarget.x) <= .25 && Mathf.Abs(overseer.transform.position.z - enemyTarget.z) <= .25)
            {
                overseerReached = true;
            }
            if (AllReached())
            {
                enemyReached = true;
                t = 0;
                resumeCount = counter;
            }

        }else if(counter >= 115 && enemyReached)
        {
            if(counter == resumeCount + 5)
            {
                door.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if(counter == resumeCount + 90)
            {
                Vector3 newPos = player.transform.position;
                newPos.x -= .25f;
                player.transform.position = newPos;
            }
            else if (counter == resumeCount + 95)
            {
                Vector3 newPos = player.transform.position;
                newPos.x -= .25f;
                player.transform.position = newPos;
            }
            else if (counter == resumeCount + 105)
            {
                Vector3 newPos = player.transform.position;
                newPos.x -= .25f;
                player.transform.position = newPos;
            }
            else if (counter == resumeCount + 110)
            {
                Vector3 newPos = player.transform.position;
                newPos.x += .25f;
                player.transform.position = newPos;
            }
            else if (counter == resumeCount + 115)
            {
                Vector3 newPos = player.transform.position;
                newPos.x += .25f;
                player.transform.position = newPos;
            }
            else if (counter == resumeCount + 120)
            {
                Vector3 newPos = player.transform.position;
                newPos.x += .25f;
                player.transform.position = newPos;
            }
            else if (counter == resumeCount + 125)
            {
                Vector3 newPos = player.transform.position;
                newPos.x += .25f;
                player.transform.position = newPos;
            }
            else if (counter == resumeCount + 130)
            {
                Vector3 newPos = player.transform.position;
                newPos.x += .25f;
                player.transform.position = newPos;
            }
            else if (counter == resumeCount + 135)
            {
                Vector3 newPos = player.transform.position;
                newPos.x += .25f;
                player.transform.position = newPos;
                playerInitPos = player.transform.position;
            }else if(counter >= resumeCount + 170)
            {
                t += Time.deltaTime / timePerDirection;
                player.transform.position = Vector3.Lerp(playerInitPos, playerTarget, t);
            }
        }
        counter++;
    }

    bool AllReached()
    {
        foreach(bool b in reached)
        {
            if (!b)
            {
                return false;
            }
        }
        if (!overseerReached)
        {
            return false;
        }

        return true;
    }
}
