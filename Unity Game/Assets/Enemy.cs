using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    public int maxHealth = 40;
    int currentHealth;

    private enum State { idle, dead } // Player states
    private State state = State.idle; // Default state

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetInteger("state", (int)state);
        Debug.Log("enemy died");

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
