using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    [Header("Settings")]
    public float attackRange = 2f;
    public float damage = 10f;
    public float attackCooldown = 1.5f;

    private NavMeshAgent agent;
    private Animator animator;

    private float attackTimer;

    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null || isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            ChasePlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }

    void AttackPlayer()
    {
        // STOP movement when attacking
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        transform.LookAt(player);

        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            DealDamage();
            attackTimer = 0f;
        }
    }

    public void Death()
    {
        if (isDead) return;

        isDead = true;

        // FULL STOP movement
        agent.isStopped = true;
        agent.enabled = false; // important!


        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("Dead", true);
    }

    void DealDamage()
    {
        PlayerHealth ph = player.GetComponent<PlayerHealth>();

        if (ph != null)
        {
            ph.TakeDamage(damage);
        }
    }
}