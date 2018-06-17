using UnityEngine;

public class ProjectileOld : MonoBehaviour {

    public float speed;
    public Rigidbody2D rb;

    private void FixedUpdate()
    {
        rb.AddForce(transform.right *speed);
    }
}
