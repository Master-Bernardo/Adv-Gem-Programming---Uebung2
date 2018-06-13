using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour {

    //according to Brackeys Tutorial

    public Transform[] backgrounds;
    public float[] parallaxScales;
    public float smoothing = 1f;

    public Transform cam;
    private Vector3 previousCamPos;

    // Use this for initialization
    void Start () {
        previousCamPos = cam.position;

        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = 1/backgrounds[i].position.z * 10;
        }
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < backgrounds.Length; i++) { 
            float _parallax = Mathf.Abs((previousCamPos.x - cam.position.x)) * parallaxScales[i];

            float backgroundTargetPosX = backgrounds[i].position.x + _parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //smoothing, fade between current and target
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
	}
}
