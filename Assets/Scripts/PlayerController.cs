using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 3f;
    public float gravity = 20f;
    public float jumpCooldown = 0.2f;
    public float airControl = 0.5f;
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private float verticalVelocity;
    private float xRotation = 0f;
    public bool canJump = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Check for sprint input
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        if (controller.isGrounded)
        {
            moveDirection = move * currentSpeed;
            verticalVelocity = -gravity * Time.deltaTime;

            if (Input.GetButtonDown("Jump") && canJump)
            {
                verticalVelocity = Mathf.Sqrt(2 * jumpForce * gravity); // More natural jump curve
                canJump = false;
                Invoke("ResetJump", jumpCooldown);
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            Vector3 airMove = move * currentSpeed * airControl;
            moveDirection = new Vector3(
                Mathf.Lerp(moveDirection.x, airMove.x, Time.deltaTime * 5f),
                verticalVelocity,
                Mathf.Lerp(moveDirection.z, airMove.z, Time.deltaTime * 5f)
            );
        }

        moveDirection.y = verticalVelocity;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void ResetJump()
    {
        canJump = true;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent looking too far up/down

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}