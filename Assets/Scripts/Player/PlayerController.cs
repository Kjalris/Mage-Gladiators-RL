using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player attributes")]
    [SerializeField] float movement_speed = 5f;
    [SerializeField] float rotation_speed = 700f;
    [SerializeField] float jump_height = 2f;

    [Header("Ground check attributes")]
    [SerializeField] float ground_check_radius = 0.2f;
    [SerializeField] Vector3 ground_check_offset;
    [SerializeField] LayerMask ground_layer;

    private Vector3 player_velocity;

    bool running = false;
    bool is_grounded;

    Quaternion target_rotation;

    CameraController camera_controller;
    Animator animator;
    CharacterController characterController;
    PlayerManagement playerManagement;

    private void Awake()
    {
        camera_controller = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerManagement = GetComponent<PlayerManagement>();
    }
    private void Update()
    {
        // If player reaches 0 hp, destroy player
        if (playerManagement.GetCurrentHP() == 0)
        {
            animator.Play("Death");
            Invoke("Delay", 3.2f);
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        float movement = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        var move_input = (new Vector3(horizontal, 0, vertical)).normalized;

        var move_direction = camera_controller.GetPlanerRotation() * move_input;

        // check if player is grounded before allowing to jump
        if (Input.GetKeyDown(KeyCode.Space) && is_grounded == true)
        {
            animator.Play("Jumping");
            player_velocity.y += Mathf.Sqrt(jump_height * -3.0f * Physics.gravity.y);
        }

        player_velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(player_velocity * Time.deltaTime);

        CheckGrounded();
        if (is_grounded)
        {
            player_velocity.y = -0.5f;
        }
        else
        {
            player_velocity.y += Physics.gravity.y * Time.deltaTime;
        }

        // While player is moving around, play animations accordingly and face the right direction
        if (movement > 0)
        {
            // Press shift to run
            if (Input.GetKey(KeyCode.LeftShift)) {
                running = true;
                if (running)
                {
                    characterController.Move(move_direction * movement_speed * Time.deltaTime);
                    target_rotation = Quaternion.LookRotation(move_direction);
                    animator.SetFloat("Move_amount", movement*10, 0.2f, Time.deltaTime);
                }
                running = false;
            }

            characterController.Move(move_direction * movement_speed/2 * Time.deltaTime);
            target_rotation = Quaternion.LookRotation(move_direction);
            animator.SetFloat("Move_amount", movement/5, 0.2f, Time.deltaTime);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, rotation_speed * Time.deltaTime);

        animator.SetFloat("Move_amount", movement, 0.2f, Time.deltaTime);

    }

    void CheckGrounded()
    {
       is_grounded = Physics.CheckSphere(transform.TransformPoint(ground_check_offset), ground_check_radius, ground_layer);
    }

    // Delay is used when the player hits 0 hp. Then the object will be destroyed
    // TODO: play endgame scene / UI when reaching that point
    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(ground_check_offset), ground_check_radius);
    }
}
