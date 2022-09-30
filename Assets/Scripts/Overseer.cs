using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overseer : MonoBehaviour
{
    // SPOTLIGHT AND LERPING
    public GameObject spotlightGObj;
    public Transform startingPosition;
    public Transform endPosition;
    public List<Transform> possibleEndPositions;
    Light spotlight;
    float startTime;
    float journeyLength;
    float speed = 1.0f;
    float yFollow = 47;
    bool completedPath = false;

    // PHYSICS CAST
    public Vector3 origin;
    public Vector3 direction;
    public RaycastHit hitInfo;
    public float maxDistance = 47;
    public float currentHitDistance;
    public float radius = 10;
    public LayerMask layerMask;
    public Transform floorArea;
    public string SceneToLoad;

    // ENEMY CREATION
    public GameObject tempMonster;
    public List<GameObject> monstersSpawned;
    public GameObject player;
    public bool spawned = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get Vars
        SceneToLoad = SceneManager.GetActiveScene().name;
        startTime = Time.time;
        spotlight = spotlightGObj.GetComponent<Light>();

        // Set Positions
        endPosition = possibleEndPositions[0];
        startingPosition.position = new Vector3(startingPosition.transform.position.x, startingPosition.transform.position.y, startingPosition.transform.position.z);
        endPosition.position = new Vector3(Vector3.zero.x, startingPosition.transform.position.y, Vector3.zero.z);

        // find distance from last spot
        journeyLength = Vector3.Distance(startingPosition.position, endPosition.position);

        // initialize monsterList
        monstersSpawned = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("pause").GetComponent<PauseMenu>().paused)
        {

            // Set PhysicsSphere Origin and Direction
            origin = transform.position;
            direction = transform.forward;

            // Random Pathing
            if (!completedPath)
            {
                endPosition.position = endPosition.GetComponentInParent<Transform>().position;
                float distanceCovered = (Time.time - startTime) * speed;
                float fractionOfInterpalation = distanceCovered / journeyLength;
                transform.position = Vector3.Lerp(startingPosition.position, endPosition.position, fractionOfInterpalation);

                float calculatedDistance = Vector3.Distance(startingPosition.position, endPosition.position);
                if (calculatedDistance <= .01f)
                {
                    // Complete Path
                    completedPath = true;

                    // Get Random New Position
                    System.Random rand = new System.Random();
                    int num = rand.Next(0, possibleEndPositions.Count);
                    endPosition.position = new Vector3(possibleEndPositions[num].position.x, yFollow, possibleEndPositions[num].position.z);

                    // Randomize End Positions
                    for (int i = 0; i < possibleEndPositions.Count; i++)
                    {
                        if (i != num)
                        {
                            possibleEndPositions[i].transform.position = new Vector3(rand.Next((int)-50, (int)50), yFollow, rand.Next((int)-50, (int)50));
                        }
                    }
                }
            }

            if (Time.time > 3f && completedPath)
            {
                startTime = Time.time;
                journeyLength = Vector3.Distance(startingPosition.position, endPosition.position);
                completedPath = false;

                // Check if Monsters are already spawned
                if (spawned)
                {
                    if (monstersSpawned.Count <= 0)
                    {
                        spawned = false;
                    }
                }
            }

            /*  if (spawned)
              {
                  ArrayList toRemove = new ArrayList();
                  foreach (var monster in monstersSpawned)
                  {
                      if (!monster.GetComponent<Monster>().playerFound)
                      {
                          toRemove.Add(monster);
                      }
                  }
                  foreach (var monster in toRemove)
                  {
                      monstersSpawned.Remove((GameObject)monster);
                  }
                  if (monstersSpawned.Count == 0)
                  {
                      spawned = false;
                  }
              }*/

            //COLLISION CHECK
            if (Physics.SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                Debug.Log(hitInfo.transform.gameObject.name);
                currentHitDistance = hitInfo.distance;

                if (hitInfo.transform.gameObject.name == "Player3D" && !spawned)
                {
                    spawned = true;
                    SpawnMonstersByPlayer();
                }
            }
            else
            {
                currentHitDistance = maxDistance;
            }

            // Check if Monsters Should Look at Player
            //  if (monstersSpawned != null)
            // {
            //   foreach (var item in monstersSpawned)
            //  {
            //      item.transform.LookAt(player.transform.right);
            //   }
            // }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, radius);
    }

    public void SpawnMonstersByPlayer()
    {
        System.Random rand = new System.Random();
        int numOfMonsters = rand.Next(1, 6);
        for (int i = 0; i < numOfMonsters; i++)
        {
            GameObject monster = Instantiate(tempMonster);
            SpawnAroundPlayer(monster.transform);
            monstersSpawned.Add(monster);
        }
    }
    System.Random rand = new System.Random();
    public Transform SpawnAroundPlayer(Transform _transform)
    {
        bool success = false;
        int xDif = 0;
        int zDif = 0;
        while (!success)
        {
            int randRadius = rand.Next(-10, 10);
            if ((int)player.transform.position.x < 0)
            {
                xDif = (int)player.transform.position.x + -randRadius;
            }
            else
            {
                xDif = (int)player.transform.position.x - randRadius;
            }

            if ((int)player.transform.position.z < 0)
            {
                zDif = (int)player.transform.position.z + -randRadius;
            }
            else
            {
                zDif = (int)player.transform.position.z - randRadius;
            }
            Vector3 tempPosition = new Vector3(xDif, player.transform.position.y, zDif);
            if (!SpawnPosOnWallOrPlayer(tempPosition))
            {
                success = true;
            }
        }
        // Set New Position
        Vector3 position = new Vector3(xDif,player.transform.position.y,zDif);
        _transform.position = position;
        var q = Quaternion.LookRotation(player.transform.position - _transform.position);
        _transform.rotation = q;
        // Get Bones
        // var bones = _transform.gameObject.GetComponentsInChildren<Transform>()[0];
        // bones.transform.LookAt(player.transform.position);
        return _transform;
    }

    bool SpawnPosOnWallOrPlayer(Vector3 pos)
    {
        Collider[] wallCols = Physics.OverlapSphere(pos, .5f);
        foreach(Collider col in wallCols)
        {
            if(col.gameObject.tag == "wall")
            {
                return true;
            }
        }
        Collider[] playerCols = Physics.OverlapSphere(pos, 3f);
        foreach (Collider col in playerCols)
        {
            if (col.gameObject.tag == "Player" || col.gameObject.tag == "enemy")
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveMonster(GameObject monster)
    {
        monstersSpawned.Remove(monster);
        Destroy(monster);
    }
}
