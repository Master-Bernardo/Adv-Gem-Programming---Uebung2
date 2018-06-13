using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //Input Keys
    public string jump_KEY;
    public string runLeft_KEY;
    public string runRight_KEY;


    //Movement
    public float maxSpeed;
    public float speed;
    public float jumpForce;
    public Rigidbody2D rb;

    private bool wantToJump = false;
    private bool grounded = false;
    private float lastVelocity;

    //Gameplay
    public int currentHealth;
    public RectTransform healthbar;
    public int demageBreakpoint = 24;//ab wieviel Kraft bekommt man Falldamage? 24 sollte gut sein

    //Appearance
    public GameObject playerModel;
    public Animator playerAnimator;
    private State state;
    private State previousState; // for stopping when moving

    private enum State
    {
        IDLE,
        JUMPING,
        MOVINGR,
        MOVINGL,
        DEAD,
    }

    void Setup()
    {
        state = State.IDLE;
    }

  

    void Update()
    {
        Debug.Log(state);
        if (Input.GetButtonDown(jump_KEY) && grounded && state!=State.DEAD)
        {
            wantToJump = true;
        }
        else if (Input.GetButton(runRight_KEY) && grounded && state != State.DEAD)
        {
            state = State.MOVINGR; 
        }
        else if (Input.GetButton(runLeft_KEY) && grounded && state != State.DEAD)
        { 
            state = State.MOVINGL;
        }
        else if(state == State.DEAD)
        {
            //just so we dont change back to idle
        }
        else if (Input.GetKey(KeyCode.R))
        {
            getDamage(50);
        }
        else if(state != State.DEAD)
        {
            state = State.IDLE;
        }

        lastVelocity = rb.velocity.magnitude;



    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if(collision.transform.tag == "Environment" || collision.transform.tag == "Player")
        {
            grounded = true;
         
           
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Environment")
        {
            if (lastVelocity > demageBreakpoint)
            {
                Debug.Log("demage taken ");
                getDamage((int)lastVelocity / 1);

            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Environment" || collision.transform.tag =="Player")
        {
            grounded = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {

        if (wantToJump)
        {
            jump();
            wantToJump = false;
            state = State.JUMPING;
        }

        switch (state)
        {
            case State.JUMPING:
                playerAnimator.SetTrigger("evade_1");
                break;
           case State.MOVINGR:
                moveRight();
                playerAnimator.SetTrigger("run");
                playerModel.transform.localScale = new Vector3(1, 1, 1); //turns the player right
                break;
            case State.MOVINGL:
                moveLeft();
                Debug.Log("movingLeft");
                playerAnimator.SetTrigger("run");
                playerModel.transform.localScale = new Vector3(-1, 1, 1);
                break;
            case State.DEAD:
                playerAnimator.SetTrigger("death");
                break;
            case State.IDLE: // some error here
                             
                playerAnimator.ResetTrigger("run");
                playerAnimator.ResetTrigger("evade_1");
                playerAnimator.SetTrigger("idle_1");
                break;
             
            default:
                Debug.Log("no state :O");
                break;
        }


        
    }

    private void jump()
    {
        rb.AddForce(Vector2.up * jumpForce*1000);
        wantToJump = false;
    }

    private void moveRight()
    {
        if (rb.velocity.magnitude < maxSpeed) rb.AddForce(Vector2.right * speed * 1000);
    }

    private void moveLeft()
    {
        if(rb.velocity.magnitude<maxSpeed)rb.AddForce((-Vector2.right) * speed * 1000);
    }

    public void getDamage(int damage)

    {
        currentHealth -= damage;
        healthbar.sizeDelta = new Vector2(currentHealth, healthbar.sizeDelta.y);
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("dead");
            state = State.DEAD;
        }
    }

    public void ApplyPhysicsBoost()
    {
        maxSpeed *= 2;
        speed *= 2;
        jumpForce *= 2;
        StartCoroutine("GoBackToNormal");

    }

    IEnumerator GoBackToNormal()
    {
        yield return new WaitForSeconds(3f);
        maxSpeed /= 2;
        speed /= 2;
        jumpForce /= 2;
    }
}
