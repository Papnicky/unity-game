using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private LayerMask Ground;
    private Collider2D coll;
    private Rigidbody2D rb;

    private bool facingLeft = true;

    
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (facingLeft)
        {
            //Test to see if we are beyond the left cap, if not, face right
            if (transform.position.x  > leftCap)
            {   
                // Make sure sprite is facing in the right direction
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                if (coll.IsTouchingLayers(Ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            //Test to see if we are beyond the right cap, if not, face left
            if (transform.position.x < rightCap)
            {
                // Make sure sprite is facing in the right direction
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                if (coll.IsTouchingLayers(Ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }
}

