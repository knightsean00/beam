using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public float speed = 5f;
    public float jumpSpeed = 8f;
    private float direction = 0f; 
    public float jumpForce = 4f;
    private Rigidbody2D player; 

    public Transform groundCheck; 
    public float groundCheckRadius; 
    public LayerMask groundLayer; 
    private bool isTouchingGround; 

    private float jumpTimeCounter; 
    public float jumpTime; 
    private bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");

        if (direction > 0f) {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
        }
        else if (direction < 0f) {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
        } else {
            player.velocity = new Vector2(0, player.velocity.y);
        }

        if((Input.GetButtonDown("Jump")) && isTouchingGround) {
            isJumping = true;
            jumpTimeCounter = jumpTime; 
            //player.velocity = new Vector2(player.velocity.x, jumpSpeed);
            // player.velocity = new Vector2(player.velocity.x, player.velocity.y * jumpForce);
            player.velocity = Vector2.up * jumpForce;   
        }

        if (Input.GetButtonDown("Jump") && isJumping == true) {
            if (jumpTimeCounter > 0) {
                // player.velocity = new Vector2(player.velocity.x, player.velocity.y * jumpForce); 
                player.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime; 
            } else {
                isJumping = false;
            }
        }

        if (Input.GetButtonDown("Jump")) {
            isJumping = false;
        }
    }
}
