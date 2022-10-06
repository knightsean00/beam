using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed;
    private float walkSpeed = 5f;
    private float runSpeed = 8f;
    private float jumpSpeed = 2f;
    private float direction = 0f; 
    private float jumpForce = 5f;
    private float jumpTime = 0.3f; 
    private Rigidbody2D player; 
    private bool isRunning;

    private bool spaceJump;
    private bool upJump;

    public Transform groundCheck; 
    private float groundCheckRadius = 0.1f; 
    public LayerMask groundLayer; 
    private bool isTouchingGround; 

    private float jumpTimeCounter; 
    private bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        direction = Input.GetAxisRaw("Horizontal");
        player.velocity = new Vector2(direction * speed, player.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
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



    }
}
