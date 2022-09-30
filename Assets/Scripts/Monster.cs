using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Vector3 pos;
    public enum Direction { up, down, left, right};
    public Direction[] patrolPath;
    public float sightDistance = 50f;
    int patrolPathPos = 0;
    public float timePerDirection = 2f;
    Vector3 initialPos;
    float t;
    Vector3 targetPos;
    Vector3 patrolStartPos;
    public bool playerFound = false;
    bool lastIterPlayerFound = false;
    ArrayList stepsFromPatrol = new ArrayList();
    Vector3 posLeft;
    public bool stunned = false;

    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    private Vector3 direction;
    public float maxSpeed;
    public float mass;
   // public Vector3 targetPos;
   // public float startAngle;
    private int counter = 0;
    internal int stunFrames = 0;
    bool reachedPatrolStart = false;


    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        mass = 1;
        maxSpeed = 10f;
        pos = gameObject.transform.position;
        initialPos = gameObject.transform.position;
        patrolStartPos = transform.position;
       // startAngle = gameObject.transform.rotation.y;
        //Debug.Log("Start angle is " + startAngle);
        SetTargetPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("pause").GetComponent<PauseMenu>().paused)
        {
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
                    if (counter >= 10)
                    {
                        stepsFromPatrol.Add(transform.position);
                        counter = 0;
                    }
                    else
                    {
                        counter++;
                    }
                }
                else if (!playerFound && stepsFromPatrol.Count > 1)
                {
                    maxSpeed = 5f;
                    Vector3 currentTarget = (Vector3)stepsFromPatrol[stepsFromPatrol.Count - 1];
                    if (Mathf.Abs(transform.position.x - currentTarget.x) <= .5f && Mathf.Abs(transform.position.z - currentTarget.z) <= .5f)
                    {
                        stepsFromPatrol.RemoveAt(stepsFromPatrol.Count - 1);
                        currentTarget = (Vector3)stepsFromPatrol[stepsFromPatrol.Count - 1];
                        if (stepsFromPatrol.Count == 0)
                        {
                            reachedPatrolStart = true;
                        }
                        //SetTargetPos();
                    }
                    Vector3 seekSteerForce = Seek(currentTarget);
                    ApplyForce(seekSteerForce);
                    var q = Quaternion.LookRotation(currentTarget - transform.position);
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
                    if (Mathf.Abs(transform.position.x - targetPos.x) < .1f && Mathf.Abs(transform.position.z - targetPos.z) < .1f || reachedPatrolStart)
                    {
                        SetTargetPos();
                        reachedPatrolStart = false;
                    }
                    t += Time.deltaTime / timePerDirection;
                    if (!WouldCollide(Vector3.Lerp(initialPos, targetPos, t)))
                    {
                        transform.position = Vector3.Lerp(initialPos, targetPos, t);
                    }
                    else
                    {
                        SetTargetPos();
                    }
                }
            }
            else
            {
                stunFrames++;
            }
        }
    }

    // Todo: move somewhere else so not duplicated from Player script
    internal bool WouldCollide(Vector3 newPos)
    {
        Collider[] cols = Physics.OverlapSphere(newPos, 1f);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "wall")
            {
                return true;
            }
        }
        return false;
    }

    internal void PlayerSpotted()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 target = player.transform.position - transform.position;
        float angle = Vector3.Angle(target, transform.forward);
        //Debug.Log(angle);
        if (playerFound)
        {
            lastIterPlayerFound = true;
        }
        else
        {
            lastIterPlayerFound = false;
        }
        Debug.DrawRay(transform.position, new Vector3(sightDistance, 0, 0), Color.blue, 0.5f);

        if (angle <= 30f && Vector3.Distance(player.transform.position, transform.position) <= sightDistance && !PlayerBehindWall(player))
        {
           //Debug.Log("In line of sight");
            playerFound = true;
        }
        else
        {
           // Debug.Log("Not in line of sight");
            playerFound = false;
        }
    }

    //needs work, not super accurate
    internal bool PlayerBehindWall(GameObject player)
    {
        Collider[] cols;
        RaycastHit hit;
        Debug.DrawLine(transform.position, player.transform.position, Color.green, 0.5f);
        if(Physics.Linecast(transform.position, player.transform.position, out hit))
        {
            if(hit.collider.gameObject.tag == "wall")
            {
              //  Debug.Log("Behind wall");
                return true;
            }
        }
        return false;
    }

    private void SetTargetPos()
    {
        timePerDirection = 2f;
        initialPos = transform.position;
        t = 0;
        Vector3 newPos = transform.position;
        Vector3 rotation = new Vector3(0, 0, 0);
        switch (patrolPath[patrolPathPos])
        {
            case Direction.up:
                newPos.z += 7f;
                rotation.y = 0;
                break;
            case Direction.down:
                newPos.z -= 7f;
                rotation.y = 180f;
                break;
            case Direction.left:
                newPos.x -= 7f;
                rotation.y = 270f;
                break;
            case Direction.right:
                newPos.x += 7f;
                rotation.y = 90f;
                break;
        }

        if (patrolPathPos == patrolPath.Length - 1)
        {
            patrolPathPos = 0;
        }
        else
        {
            patrolPathPos++;
        }

        gameObject.transform.eulerAngles = rotation;
        targetPos = newPos;
    }

 /*   private void PlayerHuntTargetPos(Direction direction)
    {
        timePerDirection = .15f;
        Debug.Log(direction);
        initialPos = transform.position;
        t = 0;
        Vector3 newPos = transform.position;
        Vector3 rotation = new Vector3(0, 0, 0);
        switch (direction)
        {
            case Direction.up:
                newPos.z += 1f;
                rotation.y = 0;
                break;
            case Direction.down:
                newPos.z -= 1f;
                rotation.y = 180f;
                break;
            case Direction.left:
                newPos.x -= 1f;
                rotation.y = 270f;
                break;
            case Direction.right:
                newPos.x += 1f;
                rotation.y = 90f;
                break;
        }

        gameObject.transform.eulerAngles = rotation;
        targetPos = newPos;
    }*/

    public void Stun()
    {
        stunned = true;
    }

   /* Direction FindPlayerDirection()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float xDist = player.transform.position.x - transform.position.x;
        float zDist = player.transform.position.z - transform.position.z;
        if(Mathf.Abs(xDist) - Mathf.Abs(zDist) > 0)
        {
            if(xDist < 0)
            {
                return Direction.left;
            }
            else
            {
                return Direction.right;
            }
        }
        else
        {
            if(zDist < 0)
            {
                return Direction.down;
            }
            else
            {
                return Direction.up;
            }
        }
    }
    */
    internal Vector3 Seek(Vector3 destPos)
    {
        Vector3 desiredVel = destPos - position;    //want to go towards target
        desiredVel.Normalize();
        desiredVel *= maxSpeed;
        Vector3 seekSteerForce = desiredVel - velocity;
        return seekSteerForce;
    }

    internal void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

}
