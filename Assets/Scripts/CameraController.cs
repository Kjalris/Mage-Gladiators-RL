using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    [SerializeField] Transform follow_target;

    [SerializeField] float distance = 7;

    [SerializeField] float min_vertical_angle = -45;
    [SerializeField] float max_vertical_angle = 45;

    [SerializeField] float camera_speed = 2f;

    [SerializeField] Vector2 framing_offset;

    float rotationY;
    float rotationX;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        rotationY += Input.GetAxis("Mouse X") * camera_speed;

        rotationX += Input.GetAxis("Mouse Y") * camera_speed;
        rotationX = Mathf.Clamp(rotationX, min_vertical_angle, max_vertical_angle);

        var target_rotation = Quaternion.Euler(rotationX, rotationY, 0);

        var focus_position = follow_target.position + new Vector3(framing_offset.x, framing_offset.y);

        transform.position = focus_position - target_rotation * new Vector3(0, 0, distance);
        transform.rotation = target_rotation;
    }

}
