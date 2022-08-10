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

    float gravity_value;

    bool running = false;
    bool is_grounded;

    Quaternion target_rotation;

    CameraController camera_controller;
    Animator animator;
    CharacterController characterController;

    private void Awake()
    {
        camera_controller = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        

        float movement = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        var move_input = (new Vector3(horizontal, 0, vertical)).normalized;

        var move_direction = camera_controller.GetPlanerRotation() * move_input;

        if (Input.GetKey(KeyCode.Space))
        {
            animator.Play("Jumping");
            characterController.Move(new Vector3(0, jump_height, 0) * movement_speed * Time.deltaTime);
        }

        CheckGrounded();
        if (is_grounded)
        {
            gravity_value = -0.5f;
        }
        else
        {
            gravity_value += Physics.gravity.y * Time.deltaTime;
        }

        if (movement > 0)
        {
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(ground_check_offset), ground_check_radius);
    }
}
