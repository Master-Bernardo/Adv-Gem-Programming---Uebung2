using UnityEngine;

public class Camera2PersonMain : MonoBehaviour {

    public Transform target1;
    public Transform target2;
    public float smoothSpeed ;  // the higher, the faster the camera will follow
    private float offset = -1f;
    public float offsetY;

    //splitscreen
    public Camera cam1;
    public Camera cam2;
    public Camera camMain;

    public float distanceAbWannSplitscreen = 15f;

    void Setup()
    {
        transform.position = (target1.position + target2.position) * 0.5f;
    }

    void FixedUpdate()
    {
        //move the camera
        Vector3 middle = (target1.position + target2.position) * 0.5f;


        Vector3 desiredPosition = new Vector3(middle.x, middle.y + offsetY, offset);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;


        //deside if we should switch to splitscreen
        if (Vector2.Distance(target1.position, target2.position)<distanceAbWannSplitscreen) {
            camMain.enabled = true;
            cam1.enabled = false;
            cam2.enabled = false;
           
        }
        else
        {
            //Splitscrren

            //disable main Camera
            camMain.enabled = false;
            cam1.enabled = true;
            cam2.enabled = true;

            if (target1.position.x < target2.position.x)  //correkte positionierung der Cams
            {
                //wenn player1 links -> cam1 links
                cam1.rect = new Rect(0, 0, 0.5f, 1);
                cam2.rect = new Rect(0.5f, 0, 0.5f, 1);
            }
            else
            {
                cam2.rect = new Rect(0, 0, 0.5f, 1);
                cam1.rect = new Rect(0.5f, 0, 0.5f, 1);
            }

        }

    }
}
