using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed = 5f; // player speed
    private float direction = 0f; 
    private Rigidbody2D rb; // player rigidbody 

    private Animator anim;
    private enum State {idle,running,jumping,falling} // player states
    private State state = State.idle; // default state

    private Collider2D coll;
    [SerializeField]private LayerMask Ground;

    private Vector3 respawnPoint; // records the position of player start position
    public GameObject fallDetector; // 


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        respawnPoint = transform.position;
    }
    
    void Update()
    {

        direction = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(Ground)) 
        {
            rb.velocity = new Vector2(rb.velocity.x, 10f);
            state = State.jumping;
        }// jump
        if (direction < 0f)
        {
            transform.localScale = new Vector2(-1, 1);
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        } // move left
        else if (direction > 0f)
        {
            transform.localScale = new Vector2(1, 1);
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        } // move right
        else 
        {

        }
        VelocityState();
        anim.SetInteger("state",(int) state);

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallDetector") 
        {
            transform.position = respawnPoint;
        }
    } // detects collision with tag for fall and respawns

    private void VelocityState() // check if jumping
    {
        if (state == State.jumping) // detecting jump
        {
            if (rb.velocity.y < .1f) // detecting fall
            {
                state = State.falling; 
            }
        }
        else if (state == State.falling)  // check if falling
        {
            if (coll.IsTouchingLayers(Ground)) 
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > .1f) // detecting movement
        {
            // moving
            state = State.running;
        }
        else
        {  // stopping
            state = State.idle;
        }
    }
}