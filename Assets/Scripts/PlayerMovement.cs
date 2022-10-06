using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 5f;
    public float jumpSpeed = 8f;
    private float direction = 0f; 
    public float jumpForce = 4f;
    public float jumpTime = 0.15f; 
    private Rigidbody2D player; 

    public Transform groundCheck; 
    public float groundCheckRadius; 
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
        // direction = Input.GetAxis("Horizontal");

        if (direction > 0f) {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
        }
        else if (direction < 0f) {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
        } else {
            player.velocity = new Vector2(0, player.velocity.y);
        }

        if(Input.GetKey(KeyCode.Space) && isTouchingGround == true) {
            isJumping = true;
            jumpTimeCounter = jumpTime; 
            //player.velocity = new Vector2(player.velocity.x, jumpSpeed);
            // player.velocity = new Vector2(player.velocity.x, player.velocity.y * jumpForce);
            player.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true) {
            if (jumpTimeCounter > 0) {
                player.velocity =  Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime; 
            } else {
                isJumping = false;
            }
        }

        if (!Input.GetKey(KeyCode.Space)) {
            isJumping = false;
        }
    }
}
