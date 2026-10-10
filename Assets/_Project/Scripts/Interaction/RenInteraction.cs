using UnityEngine;

public class RenInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueManager dialogueManager;

    public string InteractionPrompt => "Talk to Ren";

    public void Interact()
    {
        dialogueManager.StartRenDialogue();
    }
}
