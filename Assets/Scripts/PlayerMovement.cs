using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpSpeed = 4f;
    private float direction = 0f; 
    public float jumpForce = 5f;
    public float jumpTime = 0.3f; 
    private Rigidbody2D player; 
    private bool isRunning;

    public Transform groundCheck; 
    public float groundCheckRadius = 0.3f; 
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


        if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) && isTouchingGround == true) {
            isJumping = true;
            jumpTimeCounter = jumpTime; 
            player.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) && isJumping == true) {
            if (jumpTimeCounter > 0) {
                player.velocity =  Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime; 
            } else {
                isJumping = false;
            }
        }

        if (!(Input.GetKey(KeyCode.Space) || (Input.GetKey(KeyCode.UpArrow)))) {
            isJumping = false;
        }
    }
}
