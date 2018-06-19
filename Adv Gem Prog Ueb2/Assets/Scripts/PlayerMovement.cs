using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour {

    public int playerNumber;

    //Keys
    [Space(10)]
    [Header("Keys")]
    public string jump_KEY;
    public string runLeft_KEY;
    public string runRight_KEY;

    public string attack_a_KEY;
    public string steerAimLeft_KEY;
    public string steerAimRight_KEY;

    //Movemetn
    [Space(10)]
    [Header("Movement Values")]
    public float maxSpeed;
    [Tooltip("Speed with which we accelerate to the sides")]
    public float movementForce;
    [Tooltip("Speed with which we accelerate upwards")]
    public float jumpForce;
    public Rigidbody2D rb;

    private bool wantToJump = false;
    private bool moveR = false;
    private bool moveL = false;
    private bool grounded = true;
    private float lastVelocity;

    //Gameplay
    [Space(10)]
    [Header("Gameplay")]
    private int currentHealth = 100;
    public int maxHealth = 100;

    public int maxManna = 100;
    public float currentManna = 50;
    public float mannaRegeneration = 20f;
    private bool physicsBoostActive = false;

    [Space(10)]
    [Tooltip("how much force is needed to give FallDamage")]
    [Range(6,60)]
    public int demageBreakpoint = 24;//ab wieviel Kraft bekommt man Falldamage? 24 sollte gut sein

    //basic attack
    [Space(10)]
    [Header("Basic Attack")]
    public GameObject basicProjectilePrefab;
    public int basicAttackMannaCost = 12;

    //aimer
    [Space(10)]
    [Header("Aimer")]
    public GameObject aimObject; //has a aparticle system as children
    public float aimerRotationSpeed = 20f;
    public float aimerRotationLimit = 85f;
    private float timeSinceNodAimerKeyWasPressed = 0f;
    public float timeAfterWhichAimerDissapears = 2f;


    //Appearance
    [Space(10)]
    [Header("Appearance")]
    public GameObject playerModel;
    public Animator playerAnimator;
    private State state;
    //private State previousState; // for stopping when moving

    //sound 
    [Space(10)]
    [Header("Sound")]
    [SerializeField]
    private AudioClip dieSound;
    [SerializeField]
    private AudioClip damageSound;
    [SerializeField]
    private AudioClip[] jumpSound;
    [SerializeField]
    private AudioClip drinkPotionSound;

    private AudioSource audioSource;

    private enum State
    {
        IDLE,
        STUNNED,
        DEAD,
    }

    void Awake()
    {
        state = State.IDLE;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

  

    void Update()
    {
        UpdateManna();
        UpdateHealth();


        if (state != State.DEAD)
        {

            if (!grounded)
            {
                playerAnimator.SetBool("isInAir", true);
            }
            else
            {
                playerAnimator.SetBool("isInAir", false);
            }

            //get KEy inputs for movement and Gameplay
            if (Input.GetButtonDown(jump_KEY) && grounded)
            {
                wantToJump = true;
            }
            else if (Input.GetButton(runRight_KEY) && grounded)
            {
                moveR = true;
                playerAnimator.SetBool("isRunning", true);
                transform.localScale = new Vector3(1, 1, 1); //turns the player right
            }
            else if (Input.GetButton(runRight_KEY)) // movement in air
            {
                moveR = true;
                transform.localScale = new Vector3(1, 1, 1); //turns the player right
                playerAnimator.SetBool("isRunning", false);
            }
            else if (Input.GetButton(runLeft_KEY) && grounded)
            {
                moveL = true;
                playerAnimator.SetBool("isRunning", true);
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (Input.GetButton(runLeft_KEY)) // movement in air
            {
                moveL = true;
                transform.localScale = new Vector3(-1, 1, 1); //turns the player right
                playerAnimator.SetBool("isRunning", false);
            }
            else
            {
                playerAnimator.SetBool("isRunning", false);
            }

            //Check facing for aimer object

            //check facing
            /*if (playerModel.transform.localScale == new Vector3(1, 1, 1))
            {
                aimObject.transform.position = transform.position + new Vector3(1f, 1.5f, 0f);
                aimObject.transform.localScale = new Vector3(1f, 1f, 1f);
                //aimObject.transform.eulerAngles = new Vector3(0f, 0f, 0f); 
                //Debug.Log(aimObject.transform.localEulerAngles);
            }
            else
            {
                aimObject.transform.position = transform.position + new Vector3(-1f, 1.5f, 0f);
                aimObject.transform.localScale = new Vector3(-1f, 1f, 1f);
                //aimObject.transform.eulerAngles = new Vector3(0f, 0f, 180f);
               // Debug.Log(aimObject.transform.localEulerAngles);
            }*/

            //Attack and aiming
            if (Input.GetButtonDown(attack_a_KEY)&&currentManna>basicAttackMannaCost)
            {
                playerAnimator.SetTrigger("castSpell");
                currentManna -= basicAttackMannaCost;
                AttackA();
            }

            //Debug.Log(aimObject.transform.rotation.eulerAngles);
            if (Input.GetButton(steerAimLeft_KEY))
            {
                if (transform.localScale == new Vector3(1, 1, 1)) AimLeft();
                else AimRight();

                timeSinceNodAimerKeyWasPressed = 0f;
            }
            else if (Input.GetButton(steerAimRight_KEY))
            {
                if(transform.localScale == new Vector3(1, 1, 1)) AimRight();
                else AimLeft();
                timeSinceNodAimerKeyWasPressed = 0f;
            }
            else
            {
                timeSinceNodAimerKeyWasPressed += Time.deltaTime;
                if (timeSinceNodAimerKeyWasPressed > timeAfterWhichAimerDissapears)
                {
                    HideAimer();
                }
            }

            //for Debugging
            if (Input.GetKeyDown(KeyCode.R))
            {
                GetDamage(50);
            }
        }else
        {
            
            GameManager.Instance.playerDied(playerNumber);
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

    //for the wind objects
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.transform.tag == "WindsBlock")
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
                GetDamage((int)lastVelocity / 1);

            }
        }
        
        if (collision.transform.tag == "Player" && physicsBoostActive)
        {
            collision.gameObject.GetComponent<PlayerMovement>().GetDamage(30);
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
            Jump();
            wantToJump = false;
        }
        else if (moveR)
        {
            MoveRight();
            moveR = false;
        }

        else if (moveL)
        {
            MoveLeft();
            moveL = false;
        }  
    }

    private void Jump()
    {
        audioSource.clip = jumpSound[Random.Range(0, jumpSound.Length)];
        audioSource.Play();
        rb.AddForce(Vector2.up * jumpForce*1000);
        wantToJump = false;
    }

    private void MoveRight()
    {
        if (grounded)
        {
            if (rb.velocity.magnitude < maxSpeed) rb.AddForce(Vector2.right * movementForce * 1000);
        }else// in air the movement speed is only half
        {
            if (rb.velocity.magnitude < maxSpeed) rb.AddForce(Vector2.right * movementForce/2 * 1000);
        }
    }

    private void MoveLeft()
    {
        if (grounded) {
            if(rb.velocity.magnitude<maxSpeed)rb.AddForce((-Vector2.right) * movementForce * 1000);
        }else
        {
            if (rb.velocity.magnitude < maxSpeed) rb.AddForce(-Vector2.right * movementForce / 2 * 1000);
        }
    }

    public void GetDamage(int damage)

    {
        currentHealth -= damage;
        
        
        if (currentHealth <= 0)
        {
            playerAnimator.SetTrigger("Die");
            state = State.DEAD;
            audioSource.clip = dieSound;
            audioSource.Play();
        }
        else
        {
            playerAnimator.SetTrigger("getDamage");
            audioSource.clip = damageSound;
            audioSource.Play();
        }
        
    }

    private void UpdateManna()
    {
        currentManna = Mathf.Clamp(currentManna + mannaRegeneration * Time.deltaTime, 0f, (float)maxManna);
        UIController.Instance.UpdateMannaBar(playerNumber, (int)currentManna);
    }
    private void UpdateHealth()
    {
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UIController.Instance.UpdateHealthBar(playerNumber, (int)currentHealth);
    }


    private void AttackA()
    {
        //Instantiate(basicProjectilePrefab, aimObject.transform.position, aimObject.transform.rotation);
        //Debug.Log(Quaternion.LookRotation(aimObject.transform.GetChild(0).gameObject.transform.forward).eulerAngles);
        if (transform.localScale == new Vector3(1, 1, 1))
        {
            Instantiate(basicProjectilePrefab, aimObject.transform.position, aimObject.transform.rotation);
            //Debug.Log("Inverse" + Quaternion.Inverse(aimObject.transform.rotation).eulerAngles);
            //Debug.Log(aimObject.transform.rotation.eulerAngles);
        }
        else //Instantiate(basicProjectilePrefab, aimObject.transform.position, Quaternion.Inverse(aimObject.transform.rotation));
        {
            Debug.Log(aimObject.transform.rotation.eulerAngles.z);
            if(aimObject.transform.rotation.eulerAngles.z >= 90)
            {

                Instantiate(basicProjectilePrefab, aimObject.transform.position, Quaternion.Euler(new Vector3(0f, 0f, aimObject.transform.rotation.eulerAngles.z +180f)));
            }
            else
            {
                Instantiate(basicProjectilePrefab, aimObject.transform.position, Quaternion.Euler(new Vector3(0f, 0f, aimObject.transform.rotation.eulerAngles.z - 180f)));
            }
            //Instantiate(basicProjectilePrefab, aimObject.transform.position, Quaternion.Euler(new Vector3(0f,0f, aimObject.transform.rotation.eulerAngles.z+180f)));
            //Debug.Log("Inverse" + Quaternion.Inverse(aimObject.transform.rotation).eulerAngles);
            //Debug.Log(aimObject.transform.rotation.eulerAngles);
        }

    }

    private void AimLeft()
    {
        

        //enable effect
        aimObject.transform.GetChild(0).gameObject.SetActive(true);
        //rotate aimer empthy Object
        aimObject.transform.Rotate(new Vector3(0f, 0f, aimerRotationSpeed * Time.deltaTime));
        //clamp
        if (aimObject.transform.localEulerAngles.z < aimerRotationLimit*2)
        {
            aimObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Clamp(aimObject.transform.localRotation.eulerAngles.z, 0f, aimerRotationLimit)));
        }else
        {
            aimObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Clamp(aimObject.transform.localRotation.eulerAngles.z, 360f - aimerRotationLimit, 360f)));
        }
        //disable effect after 1 second
        //StartCoroutine("MakeAimHelperDissapear");
    }

    private void AimRight()
    {
       

        //enable effect
        aimObject.transform.GetChild(0).gameObject.SetActive(true);
        //rotate aimer empthy Object
        aimObject.transform.Rotate(new Vector3(0f, 0f, - aimerRotationSpeed * Time.deltaTime));
        //clamp
        if (aimObject.transform.localEulerAngles.z < aimerRotationLimit*2)
        {
            aimObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Clamp(aimObject.transform.localRotation.eulerAngles.z, 0f, aimerRotationLimit)));
        }
        else
        {
            aimObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Clamp(aimObject.transform.localRotation.eulerAngles.z, 360f - aimerRotationLimit, 360f)));
        }

        //disable effect after 1 second
        //StartCoroutine("MakeAimHelperDissapear");
    }

   private void HideAimer()
    {
        aimObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ApplyPhysicsBoost()
    {
        audioSource.clip = drinkPotionSound;
        audioSource.Play();
        maxSpeed *= 2;
        movementForce *= 2;
        playerAnimator.SetTrigger("getPowerUp");
        StartCoroutine("GoBackToNormal");
        physicsBoostActive = true;

    }
    IEnumerator GoBackToNormal()
    {
        yield return new WaitForSeconds(7f);
        maxSpeed /= 2;
        movementForce /= 2;
        physicsBoostActive = false;
    }

    public void RegenerateManna(int manna)
    {
        audioSource.clip = drinkPotionSound;
        audioSource.Play();
        currentManna += manna;
        playerAnimator.SetTrigger("getPowerUp");
    }

    public void RegenerateHealth(int health)
    {
        audioSource.clip = drinkPotionSound;
        audioSource.Play();
        currentHealth += health;
        playerAnimator.SetTrigger("getPowerUp");
    }
}
