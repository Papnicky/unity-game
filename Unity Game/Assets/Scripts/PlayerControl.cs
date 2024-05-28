using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    // Start() variables
    private Rigidbody2D rb; 
    private Collider2D coll;
    private Animator anim;
    private Vector3 respawnPoint;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public int health;
    public int numOfHearts;
    public GameObject fallDetector;


    // FSM
    private enum State {idle, running, jumping, falling, hurt, attacking} // Player states
    private State state = State.idle; // Default state


    // Inspector variables
    [SerializeField] private LayerMask Ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int stars = 0;
    [SerializeField] private Text starText;
    [SerializeField] private float hurtForce = 10f;


 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();


        respawnPoint = transform.position;
    }
    
    void Update()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

       

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
        else if (collision.tag == "NextLevel")
        {
            if (stars >= 60)
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            { 
              // print message
            }
        }
        else if (collision.tag == "PreviousLevel")
        {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else if (collision.tag == "Spikes")
        {
            health -= 1;
            numOfHearts -= 1;
            transform.position = respawnPoint;
        }
    } // Detects collision with tag for fall, respawn, etc.

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
                health -= 1;
                numOfHearts -= 1;
                if (numOfHearts == 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

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