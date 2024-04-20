using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Sensitivities")]
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;

    [Header("Orientation")]
    [SerializeField] private Transform orientation;

    private float rotateX;
    private float rotateY;

    // Start is called before the first frame update
    void Start()
    {
        // Makes cursor disappear and locks its place
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Gets frame by frame input from the mouse
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        // Due to Unity physics, add x input to y rotation, and subtract y input from x rotation
        rotateY += mouseX;
        rotateX -= mouseY;

        // Ensures player cannot look up or down more than 180 degrees
        rotateX = Mathf.Clamp(rotateX, -90f, 90f);

        // Applies the rotation of the camera and the player orientation
        transform.rotation = Quaternion.Euler(rotateX, rotateY, 0);
        orientation.rotation = Quaternion.Euler(0, rotateY, 0);
    }
}
