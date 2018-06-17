using UnityEngine;

public class Camera2PersonMain : MonoBehaviour {


    private Transform target1;
    private Transform target2;
    [SerializeField]
    [Tooltip("the higher, the faster the camera will follow")]
    private float smoothSpeed ;
    private float offset = -1f;
    [SerializeField]
    private float offsetY;
    [SerializeField]
    private Camera camMain;

    [Space(20)]
    [Header("SplitscreenCams")]
    [SerializeField]
    private Camera cam1;
    [SerializeField]
    private Camera cam2;

    [SerializeField]
    private float distanceAbWannSplitscreen = 15f;

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

    public void SetPlayer1Cam(GameObject player1Camera, GameObject currentPlayer1Object)
    {
        target1 = currentPlayer1Object.transform;
        cam1.GetComponent<CameraSmoothFollow>().SetTarget(currentPlayer1Object);
    }

    public void SetPlayer2Cam(GameObject player2Camera, GameObject currentPlayer2Object)
    {
        target2 = currentPlayer2Object.transform;
        cam2.GetComponent<CameraSmoothFollow>().SetTarget(currentPlayer2Object);
    }
}
