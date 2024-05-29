using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    AudioManager audioManager;

    public Animator anim;

    public Transform attackPoint;
    public float attackRange = 0.5f;

    public int attackDamage = 40;
    public LayerMask enemyLayer;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            audioManager.PlaySFX(audioManager.attack);
            Attack();
        }
    }

    void Attack()
    {
        // Attack animation
        anim.SetTrigger("attack");

        // Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        // Damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) 
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        
    }
}
