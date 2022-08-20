using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public PlayerManagement playerManagement;
    public Transform player;

    public LayerMask obstacles_layer, player_layer;

    // Attacking
    [Header("Enemy attributes")]
    [SerializeField] float time_between_attacks;
    [SerializeField] int attack_damage;
    [SerializeField] float attack_range;
    [SerializeField] float sight_range;
    [SerializeField] int health;
    bool already_attacked = false;

    // States
    [Header("State")]
    public bool player_in_attack_range;
    public bool player_in_sight_range;



    public void Awake()
    {
        player = GameObject.Find("Kachujin G Rosales").transform;
        agent = GetComponent<NavMeshAgent>();
        playerManagement = GetComponent<PlayerManagement>();
    }

    public void Update()
    {
        // check for sight and attack range
        player_in_sight_range = Physics.CheckSphere(transform.position, sight_range, player_layer);
        player_in_attack_range = Physics.CheckSphere(transform.position, attack_range, player_layer);

        if (player_in_sight_range && !player_in_attack_range)
        {
            ChasePlayer();
        }
        if (player_in_sight_range && player_in_attack_range)
        {
            AttackPlayer();
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
            // TODO: player does not take dmg, needs fix
            playerManagement.TakeDamage(attack_damage);
            already_attacked = true;
            Invoke(nameof(ResetAttack), time_between_attacks);
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
            Invoke(nameof(DestroyEnemy), 0.5f);
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
