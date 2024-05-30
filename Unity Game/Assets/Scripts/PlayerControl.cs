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
    private enum State {idle, running, jumping, falling, hurt, attacking, climbing} // Player states
    private State state = State.idle; // Default state

    private float vertical;
    private bool isLadder;
    private bool isClimbing;

    // Inspector variables
    [SerializeField] private LayerMask Ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Text scoreText;
    [SerializeField] private float hurtForce = 10f;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        respawnPoint = transform.position;
        scoreText.text = Scoring.totalScore.ToString();
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

        vertical = Input.GetAxis("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f)
        { 
            isClimbing = true;
            
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {   
            state = State.climbing;
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else 
        { 
            rb.gravityScale = 3f; 
        }
    }
    private void Movement()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
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
            audioManager.PlaySFX(audioManager.jump);
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
            audioManager.PlaySFX(audioManager.checkpoint);
            respawnPoint = transform.position;
        }
        else if (collision.tag == "Collectible")
        {
            audioManager.PlaySFX(audioManager.collect);
            Scoring.totalScore += 1;
            scoreText.text = Scoring.totalScore.ToString();
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Chest")
        {
            audioManager.PlaySFX(audioManager.chest);
            Scoring.totalScore += 10;
            scoreText.text = Scoring.totalScore.ToString();
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Spikes")
        {
            health -= 1;
            numOfHearts -= 1;
            transform.position = respawnPoint;
        }
        else if (collision.tag == "Ladder")
        { 
            isLadder = true;    
            isClimbing = true;
        }
    } // Detects collision with tag for fall, respawn, etc.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            isLadder = false;
            isClimbing = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (state == State.falling)
            {
                audioManager.PlaySFX(audioManager.enemydeath);
                Destroy(other.gameObject);
                Scoring.totalScore += 2;
                scoreText.text = Scoring.totalScore.ToString();
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


    private void AnimationState() 
    {


        if (state == State.jumping) 
        {
            if (rb.velocity.y < .1f) // Detecting fall
            {
                state = State.falling; 
            }
        } // Check if jumping
        else if (state == State.falling)  
        {
            if (coll.IsTouchingLayers(Ground)) 
            {
                state = State.idle;
            }
        } // Check if falling
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
        else
        {  
            state = State.idle;
        } // Stopping
    }
}