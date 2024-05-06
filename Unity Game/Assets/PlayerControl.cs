using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    // Start() variables
    private Rigidbody2D rb; 
    private Collider2D coll;
    private Animator anim;
    private Vector3 respawnPoint;

    

    // FSM
    private enum State {idle, running, jumping, falling, hurt} // Player states
    private State state = State.idle; // Default state

    // Inspector variables
    [SerializeField] private LayerMask Ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int stars = 0;
    [SerializeField] private Text starText;
    [SerializeField] private float hurtForce = 10f;

    public GameObject fallDetector; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        respawnPoint = transform.position;
    }
    
    void Update()
    {
        if (state != State.hurt)
        {
            Movement();
        }

        AnimationState();
        anim.SetInteger("state", (int)state); // Sets animation based on enum State

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);

    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        } // Moving left

        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        } // Moving right

        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(Ground))
        {
            Jump();
        } // Jumping

        /*if (Input.GetButtonDown("Attack"))
        {
            state = State.attacking;
        }*/
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
        else if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;        
        }
        else if (collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
            stars += 1;
            starText.text = stars.ToString();
        }

    } // Detects collision with tag for fall, respawn, collectible

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (state == State.falling)
            {
                Destroy(other.gameObject);
                Jump();
            }
            else
            {
                state = State.hurt;
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    // Enemy is to my right so i should be damaged and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                    // Enemy is to my left so i should be damaged and move right

                }
            }

        }
    }


    private void AnimationState() // Check if jumping
    {


        if (state == State.jumping) // Detecting jump
        {
            if (rb.velocity.y < .1f) // Detecting fall
            {
                state = State.falling; 
            }
        }

        else if (state == State.falling)  // Check if falling
        {
            if (coll.IsTouchingLayers(Ground)) 
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        } // Check if hurt

        else if (Mathf.Abs(rb.velocity.x) > .1f) // Detecting movement
        {
            
            state = State.running;
        } // Moving

        /*else if (state == State.running && rb.velocity.y < .1f)
        {
            state = State.falling;
            running to falling check (didnt work)
        }*/
        else
        {  
            state = State.idle;
        } // Stopping
    }
}