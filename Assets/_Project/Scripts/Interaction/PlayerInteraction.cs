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
        {
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

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit,
    20f, interactionLayers))
        {
            IInteractable interactable =
                hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                float distance = Vector3.Distance(
                    transform.position,
                    hit.collider.ClosestPoint(transform.position)
                );

                if (distance <= interactionDistance)
                {
                    currentInteractable = interactable;
                }
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
    }    
}