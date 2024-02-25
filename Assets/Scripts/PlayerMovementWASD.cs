using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementWASD : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [Header("Grounded")]
    [SerializeField] private float height;
    [SerializeField] private float drag;
    [SerializeField] private LayerMask ground;
    [SerializeField]  bool grounded;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float cooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] bool canJump;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Miscellaneous")]
    [SerializeField] private Transform orientation;

    float horizontalInput;
    float verticalInput;
    Vector3 direction;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ResetJump();
    }

    // Update is called once per frame
    void Update()
    {
        // this.gameObject.transform.position = new Vector3(Camera.main.transform.position.x, this.gameObject.transform.position.y, Camera.main.transform.position.z);

        // Raycast checks if grounded or not for drag and jump mechanics
        grounded = Physics.Raycast(transform.position, Vector3.down, height + 0.1f, ground);

        // Input and speed checks are done every frame
        GetInput();
        SpeedControl();

        // If grounded, applies higher drag
        if (grounded)
        {
            rb.drag = drag;
        }
        else
        {
            rb.drag = 1;
        }
    }

    private void FixedUpdate()
    {
        // playerMove called every FixedUpdate for physics
        PlayerMove();
    }

    private void GetInput()
    {
        // Input is taken from both horizontal and vertical inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // If conditions are satisfied, player jumps and cooldown is invoked
        if (Input.GetKey(jumpKey) && canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), cooldown);
        }
    }

    private void PlayerMove()
    {
        // Movement direction is calculated, then applied as a force on the player
        direction = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        // If grounded, apply the force in the direction, otherwise apply an air multiplier
        if (grounded)
        {
            rb.AddForce(direction * moveSpeed, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(direction * moveSpeed * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        // Horizontal velocity is calculated, and if the magnitude is larger than moveSpeed, it is normalized and reapplied
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 cappedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(cappedVelocity.x, rb.velocity.y, cappedVelocity.z);
        }
    }

    private void Jump()
    {
        // Resets y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }
}
