using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed;
    public float jumpForce;
    public Rigidbody2D rb;
    bool jump = false;
    bool isJumping = false;
    public int health;
    float lastVelocity;

    // Use this for initialization
    void Start () {
		
	}

    private void Update()
    {
        if (Input.GetButtonDown("Jump1"))
        {
            jump = true;
        }
        lastVelocity = rb.velocity.magnitude;
        


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Environment")
        {
            Debug.Log("jep");
            if (isJumping)
            {
                Debug.Log("back on ground");
                isJumping = false;
                jump = false;
            }
            //Debug.Log(lastVelocity);
            if (lastVelocity > 12)
            {
                Debug.Log("demage taken ");
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        
        if (jump&&!isJumping)
        {
            rb.AddForce(Vector2.up * jumpForce);
            jump = false;
            isJumping = true;
        }

      
        if (Input.GetButton("Right1")&&!isJumping)
        {
            rb.AddForce(Vector2.right * speed);
        }
        if (Input.GetButton("Left1") && !isJumping)
        {
            rb.AddForce((-Vector2.right) * speed);
        }
        /*if (Input.GetButton("Duck1"))
        {

        }*/
    }
}
