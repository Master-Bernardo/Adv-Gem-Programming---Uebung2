using UnityEngine;
using UnityEngine.Audio;

public class Projectile : MonoBehaviour {

    public float speed;
    public float maxSpeed;
    public Rigidbody2D rb;
    public int damage = 10;

    public GameObject explosionPrefab;
    public GameObject trace;

    [SerializeField]
    private AudioClip spellSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    private void Start()
    {
        audioSource.clip = spellSound;
        audioSource.Play();
    }

    private void FixedUpdate()
    {
       
        if (rb.velocity.magnitude<maxSpeed) rb.AddForce(transform.right *speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        if ( collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().GetDamage(damage);
        }
        Destroy(gameObject);
    }
}
