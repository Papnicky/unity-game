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
    private enum State {idle,running,jumping,falling} // Player states
    private State state = State.idle; // Default state

    // Inspector variables
    [SerializeField] private LayerMask Ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpforce = 10f;
    [SerializeField] private int stars = 0;
    [SerializeField] private Text starText;

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
        Movement();
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
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            state = State.jumping;
        } // Jumping
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