using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private float speed;
    private float walkSpeed = 5f;
    private float runSpeed = 8f;
    private float jumpSpeed = 2f;
    private float direction = 0f; 
    private float jumpForce = 5f;
    private float jumpTime = 0.3f; 
    private float gravityScale = 1f;
    private Rigidbody2D player; 
    private SpriteRenderer playerRenderer;
    private bool isRunning;

    private bool spaceJump;
    private bool upJump;

    public Transform groundCheck; 
    private float groundCheckRadius = 0.1f; 
    public LayerMask groundLayer; 
    private bool isTouchingGround; 

    private float jumpTimeCounter; 
    private bool isJumping;

    public AudioSource death;
    public AudioSource respawn;
    public Vector3 respawnPoint;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        respawnPoint = transform.position;
    }

    void FixedUpdate() {
        if (!isDead) {
            direction = Input.GetAxisRaw("Horizontal");
            player.velocity = new Vector2(direction * speed, player.velocity.y);
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if (!isDead) {
            if (player.position.y <=-10) //fall death
            {
                playerDeath();
            }

            isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                speed = runSpeed;
                isRunning = true;
            } else {
                speed = walkSpeed;
                isRunning = false; 
            }

            if (direction > 0f) {
                player.velocity = new Vector2(direction * speed, player.velocity.y);
            }
            else if (direction < 0f) {
                player.velocity = new Vector2(direction * speed, player.velocity.y);
            } else {
                player.velocity = new Vector2(0, player.velocity.y);
            }


            //space bar jumping
            if(Input.GetKey(KeyCode.Space) && isTouchingGround) {
                spaceJump = true;
                jumpTimeCounter = jumpTime; 
                player.velocity = Vector2.up * jumpForce;
            }

            if (Input.GetKey(KeyCode.Space) && spaceJump) {
                if (jumpTimeCounter > 0) {
                    player.velocity =  Vector2.up * jumpForce;
                    jumpTimeCounter -= Time.deltaTime; 
                } else {
                    spaceJump = false;
                }
            }

            if (!(Input.GetKey(KeyCode.Space))) {
                spaceJump = false;
            }

            //up arrow jumping 
            if(Input.GetKey(KeyCode.UpArrow) && isTouchingGround) {
                upJump = true;
                jumpTimeCounter = jumpTime; 
                player.velocity = Vector2.up * jumpForce;
            }

            if (Input.GetKey(KeyCode.UpArrow) && upJump) {
                if (jumpTimeCounter > 0) {
                    player.velocity =  Vector2.up * jumpForce;
                    jumpTimeCounter -= Time.deltaTime; 
                } else {
                    upJump = false;
                }
            }

            if (!(Input.GetKey(KeyCode.UpArrow))) {
                upJump = false;
            }
        } else {
            player.velocity = Vector2.zero;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
         if (collision.gameObject.tag == "Death" && !isDead)
         {
            playerDeath();
         }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "checkpoint")
        {
            respawnPoint = transform.position;
        }

        if (collision.gameObject.tag == "Win")
         {
            SceneManager.LoadScene("Credits");
         }
    }

    public void playerRespawn() {
        this.GetComponent<LungBarManager>().ResetLung();
        GameObject.Find("EchoResults").GetComponent<EchoResults>().Reset();
        player.position = respawnPoint;//spawnPosition.position;
        respawn.Play();
        isDead = false;
        playerRenderer.enabled = true;
        player.gravityScale = gravityScale;
    }

    public void playerDeath()
    {
        playerRenderer.enabled = false;
        player.gravityScale = 0;
        death.Play();
        GetComponent<ParticleSystem>().Play();
        isDead = true;
        Invoke("playerRespawn", 1.25f);
    }

    public bool getDead() {
        return isDead;
    }

}
