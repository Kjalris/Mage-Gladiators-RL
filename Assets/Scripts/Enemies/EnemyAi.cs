using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask obstacles_layer, player_layer;

    // patrolling
    public Vector3 walk_point;
    bool walkpoint_set;
    public float walkpoint_range;

    // Attacking
    [Header("Enemy attributes")]
    [SerializeField] float time_between_attacks;
    [SerializeField] int attack_damage;
    [SerializeField] float attack_range;
    [SerializeField] float sight_range;
    [SerializeField] int health;
    [SerializeField] bool already_attacked = false;

    // States
    [Header("State")]
    public bool player_in_attack_range;
    public bool player_in_sight_range;



    public void Awake()
    {
        player = GameObject.Find("Kachujin G Rosales").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        // check for sight and attack range
        player_in_sight_range = Physics.CheckSphere(transform.position, sight_range, player_layer);
        player_in_attack_range = Physics.CheckSphere(transform.position, attack_range, player_layer);

        if (!player_in_sight_range && !player_in_attack_range)
        {
            Patrolling();
        }
        
        if (player_in_sight_range && !player_in_attack_range)
        {
            ChasePlayer();
        }
        if (player_in_sight_range && player_in_attack_range)
        {
            AttackPlayer();
        }

    }

    private void Patrolling()
    {
        if (!walkpoint_set)
        {
            SearchWalkPoint();
        }

        if (walkpoint_set)
        {
            agent.SetDestination(walk_point);
        }

        Vector3 distance_to_walk_point = transform.position - walk_point;

        // if destination is reached, search for a new walk point
        if (distance_to_walk_point.magnitude < 1f)
        {
            walkpoint_set = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkpoint_range, walkpoint_range);
        float randomX = Random.Range(-walkpoint_range, walkpoint_range);

        walk_point = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);   

        // check if point is inside the map
        if (Physics.Raycast(walk_point, -transform.up, 2f, obstacles_layer))
        {
            walkpoint_set = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        // stop enemy from moving when reaching attack range
        agent.SetDestination(transform.position);

        transform.LookAt(player);
   
        if (!already_attacked)
        {
            // TODO: Make player take dmg
            already_attacked = true;
            Invoke("ResetAttack", time_between_attacks);
        }
    
    }

    public void ResetAttack()
    {
        already_attacked = false;
    }

    public void RecieveDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            // TODO: Insert death animation of enemies
            Invoke(nameof(DestroyEnemy), 0.05f);
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attack_range);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sight_range);
    }
}
