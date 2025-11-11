using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private GameObject player;

    public NavMeshAgent agent;
    public EnemyData data;
    public EnemyHealth health;
    public Animator anim;
    public SoundEffect soundEffect;

    private float minTimeBtwSounds = 1.5f, maxTimeBtwSounds = 3f;
    private float timeBtwSounds, lastTimeSound;

    public LayerMask playerMask;

    public bool canMove = true;

    private float lastTimeAttacked;
    private bool isAttacking;

    public event Action OnRangeAttack;

    private void Awake()
    {
        health.InitailizeHealthValues(anim, soundEffect, data.maxHealth);
        timeBtwSounds = UnityEngine.Random.Range(minTimeBtwSounds, maxTimeBtwSounds);
    }

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health.RestoreHealth();
        SetAgentValues();
    }

    private void SetAgentValues()
    {
        agent.speed = UnityEngine.Random.Range(data.minSpeed, data.maxSpeed);
        agent.stoppingDistance = data.attackRange;
    }

    private void Update()
    {
        PlayRandomSound();
        FollowPlayer();
    }

    private void PlayRandomSound()
    {
        if (Time.time - lastTimeSound < timeBtwSounds) return;

        lastTimeSound = Time.time;
        timeBtwSounds = UnityEngine.Random.Range(minTimeBtwSounds, maxTimeBtwSounds);

        soundEffect.SetRandomPitch();
        soundEffect.GetRandomSoundEffect(0, 4); // Roars
    }

    private void FollowPlayer()
    {
        if (player == null || !canMove)
        {
            agent.isStopped = true;
            return;
        }

        float dist = Vector3.Distance(transform.position, player.transform.position);

        if(dist <= data.attackRange)
        {
            if (!isAttacking && Time.time - lastTimeAttacked >= data.attackCoolDown)
            {
                lastTimeAttacked = Time.time;
                anim.SetTrigger("Attack");
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
    }

    public void ResetAttacking() => isAttacking = false;

    public void IsAttacking() => isAttacking = true;

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
        OnRangeAttack?.Invoke();
    }

    private void OnDrawGizmos()
    {
        if (data == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.stopRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.attackRange);
    }
}