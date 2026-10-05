using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{

    // movement
    public float moveSpeed = 6f;
    public float jumpForce = 5f;

    // ground check
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    private bool isGrounded;

    // camera look
    public Transform cameraTransform;
    public float mouseSensitivity = 0.1f;
    public float smoothness = 20f;
    private float xRotation = 0f;

    private float smoothMouseX;
    private float smoothMouseY;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            // float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
            // float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;
            
            smoothMouseX = Mathf.Lerp(smoothMouseX, mouseDelta.x * mouseSensitivity, smoothness * Time.deltaTime);
            smoothMouseY = Mathf.Lerp(smoothMouseY, mouseDelta.y * mouseSensitivity, smoothness * Time.deltaTime);

            xRotation -= smoothMouseY;
            xRotation = Mathf.Clamp(xRotation, -85f, 85f);

            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * smoothMouseX);
        }


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float x = 0f;
        float z = 0f;

        if (Keyboard.current != null)
        {
            if(Keyboard.current.wKey.isPressed) z += 1f;
            if(Keyboard.current.aKey.isPressed) x -= 1f;
            if(Keyboard.current.sKey.isPressed) z -= 1f;
            if(Keyboard.current.dKey.isPressed) x += 1f;
        }
        Vector3 moveInput = (transform.right * x + transform.forward * z).normalized;
        Vector3 moveVelocity = moveInput * moveSpeed;

        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
    }
}
