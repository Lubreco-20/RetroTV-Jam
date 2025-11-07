using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float minEnemySpeed = 4f;
    public float maxEnemySpeed = 5f;

    private GameObject player;
    public NavMeshAgent agent;

    public bool canMove = true;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        SetRandomSpeed();
    }

    private void SetRandomSpeed()
    {
        agent.speed = Random.Range(minEnemySpeed, maxEnemySpeed);
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (player == null || !canMove)
        {
            agent.isStopped = true;
            return;
        }

        agent.SetDestination(player.transform.position);
    }
}
