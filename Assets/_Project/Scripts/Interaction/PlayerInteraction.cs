using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayers = ~0;

    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TMP_Text promptText;

    private PlayerControls controls;
    private IInteractable currentInteractable;
    private bool interactionEnabled = true;
    private bool waitForInteractRelease = false;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Start()
    {
        interactionPrompt.SetActive(false);
    }

    private void Update()
    {
        if (!interactionEnabled)
            return;

        // Wait for E to be released before allowing another interaction
        if (waitForInteractRelease)
        {
            if (Keyboard.current != null &&
                !Keyboard.current.eKey.isPressed)
            {
                waitForInteractRelease = false;
            }

            return;
        }

        CheckForInteraction();

        if (currentInteractable != null &&
            controls.Player.Interact.WasPressedThisFrame())
        {
            currentInteractable.Interact();
        }
    }


    private void CheckForInteraction()
    {
        currentInteractable = null;

        Collider[] nearbyColliders = Physics.OverlapSphere(
            transform.position,
            interactionDistance,
            interactionLayers
        );

        float closestDistance = Mathf.Infinity;

        foreach (Collider col in nearbyColliders)
        {
            IInteractable interactable =
                col.GetComponentInParent<IInteractable>();

            if (interactable == null)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                col.ClosestPoint(transform.position)
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentInteractable = interactable;
            }
        }

        if (currentInteractable != null)
        {
            interactionPrompt.SetActive(true);
            promptText.text =
                "[E] " + currentInteractable.InteractionPrompt;
        }
        else
        {
            interactionPrompt.SetActive(false);
        }
    }


    public void SetInteractionEnabled(bool enabled)
    {
        interactionEnabled = enabled;

        if (!enabled)
        {
            currentInteractable = null;
            interactionPrompt.SetActive(false);
        }
        else
        {
            // Require a fresh E press after closing dialogue
            waitForInteractRelease = true;
        }
    }
}