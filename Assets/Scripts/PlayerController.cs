using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movement_speed = 5f;
    [SerializeField] float rotation_speed = 500f;

    Quaternion target_rotation;

    CameraController camera_controller;
    private void Awake()
    {
        camera_controller = Camera.main.GetComponent<CameraController>();
    }
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float movement = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

        var move_input = (new Vector3(horizontal, 0, vertical)).normalized;

        var move_direction = camera_controller.GetPlanerRotation() * move_input;

        if (movement > 0)
        {
            transform.position += move_direction * movement_speed * Time.deltaTime;
            target_rotation = Quaternion.LookRotation(move_direction);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, rotation_speed * Time.deltaTime);
    }
}
