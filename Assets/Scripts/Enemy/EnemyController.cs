using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private GameObject player;

    public NavMeshAgent agent;
    public EnemyData data;
    public EnemyHealth health;
    public Animator anim;

    public LayerMask playerMask;

    public bool canMove = true;

    private float lastTimeAttacked;
    private bool isAttacking;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health.RestoreHealth();
        SetAgentValues();
    }

    private void SetAgentValues()
    {
        agent.speed = Random.Range(data.minSpeed, data.maxSpeed);
        agent.stoppingDistance = data.attackRange;
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (agent.isStopped) return;

        if (player == null || !canMove)
        {
            agent.isStopped = true;
            return;
        }

        float dist = Vector3.Distance(transform.position, player.transform.position);

        if(dist <= data.attackRange)
        {
            agent.isStopped = true;
            TryAttack();
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
    }

    private void TryAttack()
    {
        if(Time.time - lastTimeAttacked >= data.attackCoolDown)
        {
            lastTimeAttacked = Time.time;

            anim.SetTrigger("Attack");
        }
    }

    public void DoMeleeAttack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, data.attackRange, playerMask);
        foreach (var hit in hits)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.GetDamage(data.dmg);
        }
    }

    public void DoRangedAttack()
    {
        /*if (projectilePrefab == null || firePoint == null) return;

        Vector3 targetPos = player.transform.position;
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Projectile>().SetTarget(targetPos);*/
    }
}