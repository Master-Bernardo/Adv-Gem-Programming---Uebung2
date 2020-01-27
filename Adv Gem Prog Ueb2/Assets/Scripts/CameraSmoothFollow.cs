using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour {

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float smoothSpeed ;  // the higher, the faster the camera will follow
    private float offset = -1f;
    [SerializeField]
    private float offsetY;

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

    public void SetTarget(GameObject _target)
    {
        target = _target.transform;
    }
}
