using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSpell : MonoBehaviour
{
    // Asignables
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask enemies_layer;

    // stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool use_gravity;

    // damage
    public int explosion_damage;
    public int explosion_range;

    // lifetime
    [SerializeField] float max_life_time;
    public int max_collision;
    public bool explode_on_touch = true;

    int collisions = 0;
    PhysicMaterial physics_mat;

    private void Update()
    {
        max_life_time = -Time.deltaTime;

        if (collisions > max_collision)
        {
            Explode();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // count up the amount of collisions
        collisions++;

        // explode if direct hit on an enemy
        if (collision.collider.CompareTag("Enemies") || collision.collider.CompareTag("Obstacles") && explode_on_touch == true)
        {
            Explode();
        }
    }

    private void Start()
    {
        Setup();
    }

    private void Explode()
    {
        // instantiate explosion
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }

        // check for enemies
        // Collider[] enemies = Physics.OverlapSphere(transform.position, explosion_range, enemies_layer);

        /*for (int i; i < enemies.Length; i++)
        {
            // get component of enemies and call take dmg
            
            // example
            // enemies[i].GetComponent<Enemies>().take_damage(explosion_damage);
        } */

        // delay explosion to avoid bugs, is not be visible ingame
        Invoke("Delay", 0.05f);

    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void Setup()
    {
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        // assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

        // set gravity
        rb.useGravity = use_gravity;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosion_range);
    }
}
