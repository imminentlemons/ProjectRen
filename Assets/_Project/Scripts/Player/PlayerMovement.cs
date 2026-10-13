using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float animationSmoothTime = 0.1f;

    private static readonly int SpeedParameter = Animator.StringToHash("Speed");

    [Header("Camera")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float minPitch = -35f;
    [SerializeField] private float maxPitch = 65f;

    private CharacterController controller;
    private PlayerControls controls;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float verticalVelocity;
    private float cameraYaw;
    private float cameraPitch;
    private bool isInDialogue = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator != null)
        {
            // The CharacterController owns movement; animation only poses the model.
            animator.applyRootMotion = false;
        }

        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void Start()
    {
        cameraYaw = cameraTarget.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        if(isInDialogue)
        {
            UpdateAnimation(0f);
            return;
        }

        HandleMovement();
        HandleCamera();
    }

    private void HandleMovement()
    {
        Vector3 forward = cameraTarget.forward;
        Vector3 right = cameraTarget.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 direction = 
            forward * moveInput.y + right * moveInput.x;

        direction = Vector3.ClampMagnitude(direction, 1f);

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = direction * moveSpeed + Vector3.up * verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;
        float normalizedSpeed = moveSpeed > 0f
            ? Mathf.Clamp01(horizontalVelocity.magnitude / moveSpeed)
            : 0f;
        UpdateAnimation(normalizedSpeed);
    }

    private void UpdateAnimation(float normalizedSpeed)
    {
        if (animator != null && animator.isActiveAndEnabled && animator.runtimeAnimatorController != null)
        {
            animator.SetFloat(SpeedParameter, normalizedSpeed, animationSmoothTime, Time.deltaTime);
        }
    }

    private void HandleCamera()
    {
        cameraYaw += lookInput.x * mouseSensitivity;
        cameraPitch -= lookInput.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);

        cameraTarget.rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
    }

    public void SetDialogueMode(bool active)
    {
        isInDialogue = active;

        if (active)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    
    }
}
