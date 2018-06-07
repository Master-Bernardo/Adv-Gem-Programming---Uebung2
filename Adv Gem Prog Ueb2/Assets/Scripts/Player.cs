using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed;
    public float jumpForce;
    public Rigidbody2D rb;

    public int currentHealth;

    private bool wantToJump = false;
    private bool isJumping = false;
    private float lastVelocity;

    public RectTransform healthbar;

    public GameObject playerModel;
    public Animator playerAnimator;
    private string state;

    void Setup()
    {
        state = "idle";
    }

    public void getDamage(int damage)

    
    {
        currentHealth -= damage;
        healthbar.sizeDelta = new Vector2(currentHealth, healthbar.sizeDelta.y);
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("dead");
            state = "dead";
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump1"))
        {
            wantToJump = true;
        }
        lastVelocity = rb.velocity.magnitude;

        //anim controller via states
        switch (state)
        {
            case "idle":
                playerAnimator.SetTrigger("idle_1");
                break;
            case "movingRight":
                playerAnimator.SetTrigger("run");
                playerModel.transform.localScale = new Vector3(1, 1, 1); //turns the player right
                break;
            case "movingLeft":
                Debug.Log("movingLeft");
                playerAnimator.SetTrigger("run");
                playerModel.transform.localScale = new Vector3(-1, 1, 1);
                break;
            default:
                Debug.Log("no state :O");
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Environment")
        {
            //Debug.Log("jep");
            if (isJumping)
            {
                //Debug.Log("back on ground");
                isJumping = false;
                wantToJump = false;
            }
            //Debug.Log(lastVelocity);
            if (lastVelocity > 12)
            {
                Debug.Log("demage taken ");
                getDamage((int)lastVelocity/1);

            }
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        
        if (wantToJump&&!isJumping)
        {
            rb.AddForce(Vector2.up * jumpForce);
            wantToJump = false;
            isJumping = true;
        }
        else if (Input.GetButton("Right1")&&!isJumping)
        {
            state = "movingRight";
            rb.AddForce(Vector2.right * speed);
        }
        else if (Input.GetButton("Left1") && !isJumping)
        {
            state = "movingLeft";
            rb.AddForce((-Vector2.right) * speed);
        }else
        {
            state = "idle";
        }
        /*if (Input.GetButton("Duck1"))
        {

        }*/
    }
}
