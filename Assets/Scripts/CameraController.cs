using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour    
{
    
    [Header("Camera controls")]
    [SerializeField] Transform follow_target;
    [SerializeField] float distance = 7;
    [SerializeField] float min_vertical_angle = -45;
    [SerializeField] float max_vertical_angle = 45;
    [SerializeField] float camera_speed = 2f;
    [SerializeField] Vector2 framing_offset;
    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    float invert_x_value;
    float invert_y_value;

    float rotationY;
    float rotationX;
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        invert_x_value = (invertX) ? -1 : 1;
        invert_y_value = (invertY) ? -1 : 1;

        rotationY += Input.GetAxis("Mouse X") * invert_x_value * camera_speed;
        rotationX += Input.GetAxis("Mouse Y") * invert_y_value * camera_speed;

        rotationX = Mathf.Clamp(rotationX, min_vertical_angle, max_vertical_angle);

        var target_rotation = Quaternion.Euler(rotationX, rotationY, 0);

        var focus_position = follow_target.position + new Vector3(framing_offset.x, framing_offset.y);

        transform.position = focus_position - target_rotation * new Vector3(0, 0, distance);
        transform.rotation = target_rotation;

    }
    public Quaternion GetPlanerRotation()
    {
        return Quaternion.Euler(0, rotationY, 0);
    }
    
}

