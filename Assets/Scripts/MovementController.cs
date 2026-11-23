using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 4f;

    [Header("Crouch")]
    public CapsuleCollider playerCollider;
    public float crouchSpeed = 3f;
    public float crouchHeight = 1f;
    public float crouchCameraY = -0.3f; // Negative because pivot is at the waist (Y=0)
    public float crouchTransitionSpeed = 10f;
    private float standingHeight;
    private float standingCameraY;
    private bool isCrouching;

    [Header("Dash")]
    public float dashSpeed = 18f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.5f;
    private bool isDashing;
    private float dashTimer;
    private float cooldownTimer;
    private Vector3 dashDirection;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    private bool isGrounded;

    [Header("Camera Look")]
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

        if (playerCollider == null) 
            playerCollider = GetComponentInChildren<CapsuleCollider>();

        if (playerCollider != null) standingHeight = playerCollider.height;        if (cameraTransform != null) standingCameraY = cameraTransform.localPosition.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Keyboard.current != null)
        {
            // Jump (Disabled while crouching)
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded && !isCrouching)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            // Crouch Input (Hold Left Ctrl or C)
            isCrouching = Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.cKey.isPressed;

            // Dash Input (Left Shift)
            if (Keyboard.current.leftShiftKey.wasPressedThisFrame && cooldownTimer <= 0f && !isDashing)
            {
                StartDash();
            }
        }

        HandleCrouchTransition();
        HandleDashTimers();
    }

    void FixedUpdate()
    {
        // If dashing, override normal movement with dash velocity
        if (isDashing)
        {
            rb.linearVelocity = new Vector3(dashDirection.x * dashSpeed, rb.linearVelocity.y, dashDirection.z * dashSpeed);
            return;
        }

        float x = 0f;
        float z = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) z += 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
            if (Keyboard.current.sKey.isPressed) z -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;
        }

        Vector3 moveInput = (transform.right * x + transform.forward * z).normalized;
        
        // Use crouchSpeed if crouching, otherwise normal moveSpeed
        float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
        Vector3 moveVelocity = moveInput * currentSpeed;

        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
    }

    private void HandleMouseLook()
    {
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            smoothMouseX = Mathf.Lerp(smoothMouseX, mouseDelta.x * mouseSensitivity, smoothness * Time.deltaTime);
            smoothMouseY = Mathf.Lerp(smoothMouseY, mouseDelta.y * mouseSensitivity, smoothness * Time.deltaTime);

            xRotation -= smoothMouseY;
            xRotation = Mathf.Clamp(xRotation, -85f, 85f);

            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * smoothMouseX);
        }
    }

    private void HandleCrouchTransition()
    {
        if (playerCollider != null)
        {
            float targetHeight = isCrouching ? crouchHeight : standingHeight;
            playerCollider.height = Mathf.Lerp(playerCollider.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);
            
            // Since your Capsule's Center Y is 0 (waist pivot), shift Center Y downward as height shrinks
            // so your feet stay on the ground and the top lowers!
            float heightDifference = standingHeight - playerCollider.height;
            playerCollider.center = new Vector3(0, -heightDifference / 2f, 0);
        }

        if (cameraTransform != null)
        {
            float targetCamY = isCrouching ? crouchCameraY : standingCameraY;
            Vector3 camPos = cameraTransform.localPosition;
            camPos.y = Mathf.Lerp(camPos.y, targetCamY, crouchTransitionSpeed * Time.deltaTime);
            cameraTransform.localPosition = camPos;
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;

        // Determine dash direction based on current movement keys, or default to forward
        float x = 0f;
        float z = 0f;
        if (Keyboard.current.wKey.isPressed) z += 1f;
        if (Keyboard.current.aKey.isPressed) x -= 1f;
        if (Keyboard.current.sKey.isPressed) z -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;

        Vector3 inputDir = (transform.right * x + transform.forward * z).normalized;
        dashDirection = inputDir.magnitude > 0 ? inputDir : transform.forward;
    }

    private void HandleDashTimers()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}
