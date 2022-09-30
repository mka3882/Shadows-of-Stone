using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessLayer))]
public class CameraScript : MonoBehaviour
{
    protected GameObject playerObject; //holds player, but should be able to find parent whenever attached to another object
    protected float distanceFromPlayer; //this is what our scroll wheel will mess with
    protected Vector3 cameraRotation; //holds camera Rotation, pretty simple, allows us to angle it down like the 2.5D

    // Post Processing and Skybox Connection
    protected PostProcessLayer postProcessLayer;
    protected PostProcessVolume postProcessVolume;

    // Start is called before the first frame update
    void Start()
    {
        //*NOTE* this will always pick the first object in the scene that it finds with the player tag, if you have multiple player it will only find one
        //this may get wonky with multiple levels but we'll cross that bridge when we get there
        GameObject[] players; 
        
         players = GameObject.FindGameObjectsWithTag("Player");
   
        if(players.Length < 1)
        {
            Debug.Log("Can't find objects");
        }
        else
        {
            playerObject = players[0];
        }

        distanceFromPlayer = 20.0f;
        //not the cleanest but it works
        cameraRotation = new Vector3(45.0f, 0.0f, 0.0f);

        GetPostProcessingInformation();
    }

    // Update is called once per frame
    void Update()
    {
        //setting our camera position based on the player
        Vector3 newCameraPos = new Vector3(playerObject.transform.position.x,
            //remember this is +
            playerObject.transform.position.y + distanceFromPlayer,
            //and this is -
            playerObject.transform.position.z - distanceFromPlayer);

        transform.position = newCameraPos;
        transform.eulerAngles = cameraRotation;


        //handeling camera movmenet
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)//scroll up
        {
            //I'm using scroll up to zoom in
            distanceFromPlayer -= 2.0f;
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)//scroll down
        {
            //and down to scroll out
            distanceFromPlayer += 2.0f;
        }
        
    }

    /// <summary>
    /// Instantiates PPStack and Skybox
    /// </summary>
    void GetPostProcessingInformation()
    {
        // Set Skybox Material
        RenderSettings.skybox = Resources.Load<Material>("darkSkybox");

        postProcessLayer = GetComponent<PostProcessLayer>();
        postProcessVolume = GetComponent<PostProcessVolume>();
        if (postProcessVolume != null) return;

        // Create Volume and Get Main Camera Profile
        postProcessVolume = gameObject.AddComponent<PostProcessVolume>();
        postProcessVolume.profile = Resources.Load<PostProcessProfile>("MainCameraProfile");
        postProcessVolume.weight = 1f;
        postProcessVolume.isGlobal = true;

        
    }
}
