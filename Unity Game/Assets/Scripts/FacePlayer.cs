using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private readonly string playerTag = "Player";

    public void Start()
    {
        player = GameObject.FindWithTag(playerTag);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LookAtPlayer()
    {
        // What is the difference in position?
        float diff = player.transform.position.x - transform.position.x;

        spriteRenderer.flipX = diff > 1;
    }

    private void Update()
    {
        LookAtPlayer();
    }
}
