using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    float lightRadius;
    float lightIntent;
    float energy;
    float maxEnergy;
    Light pointLight;
    public Vector3 futurePos;
    Vector3 pos;
    public bool burstCalled = false;

    //props
    public float Energy
    {
        get { return energy; }
    }

    public float MaxEnergy
    {
        get { return maxEnergy; }
    }

    // Start is called before the first frame update
    void Start()
    {
        pos = gameObject.transform.position;

        //so I'm actually making the pointlight here so I can mess with it later
        pointLight = gameObject.AddComponent<Light>();
        pointLight.range = lightRadius = 3.0f;
        pointLight.intensity = lightIntent = 1.0f;
        pointLight.color = new Color(0.45f, 0.22f, 1.0f, 1.0f);
        energy = 50;
        maxEnergy = 50;
        futurePos = pos;
    }

    // Update is called once per frame
    void Update()
    {
        burstCalled = false;
        if (!GameObject.FindGameObjectWithTag("pause").GetComponent<PauseMenu>().paused)
        {
            Vector3 newPos = pos;
            Vector3 newFuturePos = pos;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPos.z += .1f;
                if (WouldCollide(newPos))
                {
                    newPos = pos;
                }
                else
                {
                    newFuturePos.z += 3f;
                    pos = newPos;
                }
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPos.x -= .1f;
                if (WouldCollide(newPos))
                {
                    newPos = pos;
                }
                else
                {
                    newFuturePos.x -= 3f;
                    pos = newPos;
                }
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPos.z -= .1f;
                if (WouldCollide(newPos))
                {
                    newPos = pos;
                }
                else
                {
                    newFuturePos.z -= 3f;
                    pos = newPos;
                }
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPos.x += .1f;
                if (WouldCollide(newPos))
                {
                    newPos = pos;
                }
                else
                {
                    newFuturePos.x += 3f;
                    pos = newPos;
                }
            }
            if (Input.GetKey(KeyCode.Space))
            {
                if (energy > 10)
                {
                    //call burst event here
                    energy -= 10;
                    lightRadius += 3;
                    lightIntent += 30;
                    burstCalled = true;
                    BurstCollide();
                }
            }

            gameObject.transform.position = pos;
            futurePos = newFuturePos;


            //run energy/light restore refresh every frame;
            if (energy < maxEnergy) { energy += 0.1f; }
            if (lightRadius > 3) { lightRadius -= 1; }
            if (lightIntent > 10) { lightIntent -= 3; }
            if (lightIntent > 5) { lightIntent -= 1; }

            pointLight.range = lightRadius;
            pointLight.intensity = lightIntent;

            if (EnemyCollide())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void BurstCollide()
    {
        Collider[] cols = Physics.OverlapSphere(pos, lightRadius);
        foreach(Collider col in cols)
        {
            if(col.gameObject.tag == "torch")
            {
                col.gameObject.GetComponent<Torch>().LightTorch();
            }
            if(col.gameObject.tag == "enemy")
            {

                Debug.Log("enemy hit");
             //   col.gameObject.SetActive(false);
             //   StunEnemy(col.gameObject);
               col.gameObject.GetComponent<Monster>().Stun();
            }
        }
      //  return false;
    }

    private IEnumerator StunEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        yield return new WaitForSeconds(3);
        enemy.SetActive(true);
    }

    private bool WouldCollide(Vector3 newPos)
    {
        Collider[] cols = Physics.OverlapSphere(newPos, .5f);
        foreach(Collider col in cols){
            if(col.gameObject.tag == "wall" || col.gameObject.tag == "door")
            {
                return true;
            }
        }
        return false;
    }

    private bool EnemyCollide()
    {
        Collider[] cols = Physics.OverlapSphere(pos, .5f);
        foreach (Collider col in cols)
        {
            if(col.gameObject.tag == "enemy")
            {
                return true;
            }
        }
        return false;
    }

}
