using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask obstacles_layer, player_layer;



    // Attacking
    [Header("Enemy attributes")]
    [SerializeField] float time_between_attacks;
    [SerializeField] float attack_damage;
    [SerializeField] float attack_range;
    [SerializeField] float sight_range;
    [SerializeField] float health = 5;
    bool already_attacked = false;

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

        if (player_in_sight_range && !player_in_attack_range)
        {
            Chase_player();
        }
        if (player_in_sight_range && player_in_attack_range)
        {
            Attack_player();
        }


    }
    private void Chase_player()
    {
        agent.SetDestination(player.position);
    }
    private void Attack_player()
    {
        // stop enemy from moving when reaching attack range
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!already_attacked)
        {
            already_attacked = true;
            Invoke(nameof(Reset_attack), time_between_attacks);
        }
    }

    public void Reset_attack()
    {
        already_attacked = false;
    }

    public void Take_damage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Invoke(nameof(Destroy_enemy), 5f);
        }
    }

    public void Destroy_enemy()
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
