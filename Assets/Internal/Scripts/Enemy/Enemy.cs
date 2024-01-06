using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : NetworkBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float changeTargetDistance = 10f;
    private NavMeshAgent agent;
    private Animator animator;

    Transform target = null;
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            agent = GetComponent<NavMeshAgent>();

            agent.enabled = true;

            agent.updateRotation = false;

            agent.updateUpAxis = false;

            agent.speed = moveSpeed;

            animator = GetComponent<Animator>();
        }
    }
    private void Update()
    {
        if (IsServer)
        {
            EnemyTargetObject();
            ChasePlayer();
        }
    }
    private void EnemyTargetObject()
    {
        if (target != null)
        {
            float distance = Vector2.Distance(target.position, transform.position);
            if (distance <= changeTargetDistance)
            {
                return;
            }
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag(CommonController.PLAYER_TAG);

        if (players.Length > 0)
        {
            float tempDistance = Vector2.Distance(transform.position, players[0].transform.position);
            Transform nextTarget = players[0].transform;
            for (int i = 0; i < players.Length; i++)
            {
                Transform tempTarget = players[i].transform;
                float distance = Vector2.Distance(tempTarget.position, transform.position);
                if (tempDistance > distance)
                {
                    tempDistance = distance;
                    nextTarget = tempTarget;
                }
            }
            target = nextTarget;
        }
    }
    private void ChasePlayer()
    {
        if (target != null)
        {
            float xAxis = target.position.x - transform.position.x;
            transform.rotation = Quaternion.Euler(new(0f, xAxis < 0f ? 180f : 0f, 0f));
            if (agent != null)
            {
                agent.SetDestination(target.transform.position);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(damage);
        }
    }
    public void EnemyDie()
    {
        if (IsServer)
        {
            if (animator != null)
            {
                animator.SetBool("Dead", true);
                agent.isStopped = true;
                agent.enabled = false;
                agent = null;
            }
        }
    }
    public void EnemyGetHit()
    {
        if (IsServer)
        {
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }
        }
    }
}
