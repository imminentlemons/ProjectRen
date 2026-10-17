
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
            int openingNode = 0;

            if (relationship.HasFlag("TeasedRen"))
            {
                openingNode = 3;
            }
            else if (relationship.HasFlag("AskedWhyLate"))
            {
                openingNode = 4;
            }
            else if (relationship.HasFlag("ApologizedToRen"))
            {
                openingNode = 5;
            }

            dialogueManager.StartDialogue(returnVisit, openingNode);
        }

        else
        {
            dialogueManager.StartDialogue(firstEncounter);
            relationship.SetFlag("HasMetRen");
        }
    }
}
