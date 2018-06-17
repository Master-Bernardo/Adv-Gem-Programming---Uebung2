using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour {

    public Transform target;
    public float smoothSpeed ;  // the higher, the faster the camera will follow
    private float offset = -1f;
    public float offsetY;

    void Setup()
    {
        transform.position = target.position;
    }

    void FixedUpdate()
    {

            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y + offsetY, offset);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);
            transform.position = smoothedPosition;

    }
}
