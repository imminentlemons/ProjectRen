
using UnityEngine;

public class RenInteraction : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueData firstEncounter;
    [SerializeField] private DialogueData returnVisit;

    public string InteractionPrompt => "Talk to Ren";

    public void Interact()
    {
        RelationshipManager relationship =
            RelationshipManager.Instance;

        if (relationship == null)
        {
            Debug.LogError("RelationshipManager is missing!");
            return;
        }

        if (relationship.HasFlag("HasMetRen"))
        {
            dialogueManager.StartDialogue(returnVisit);
        }
        else
        {
            dialogueManager.StartDialogue(firstEncounter);

            // Temporary approach: remembers the player
            // as soon as the conversation begins.
            relationship.SetFlag("HasMetRen");
        }
    }
}
