using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect2 : MonoBehaviour {

    [SerializeField]
    [Tooltip("when something is nearer than the player its negative, its positive when something is farer")]
    private float speed;  //- 

    [SerializeField]
    private Transform mainCam;
    private float previousCamPosX;

    // Use this for initialization
    void Start () {
        previousCamPosX = mainCam.position.x;
    }
	
	// Update is called once per frame
	void Update () {
        float parallaxMovement = mainCam.position.x - previousCamPosX;  //wieviel hat sich die Kamera bewegt?

        transform.position = new Vector3(transform.position.x + parallaxMovement * speed * Time.deltaTime, transform.position.y, transform.position.z);
        previousCamPosX = mainCam.position.x;
    }
}
