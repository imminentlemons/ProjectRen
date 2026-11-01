
using UnityEngine;

public class BulletinBoardInteraction : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueData bulletinBoardDialogue;

    public string InteractionPrompt => "Inspect Bulletin Board";

    public void Interact()
    {
        dialogueManager.StartDialogue(bulletinBoardDialogue);

        RelationshipManager relationship =
            RelationshipManager.Instance;

        if (relationship != null)
        {
            relationship.SetFlag("SawConcertFlyer");
        }
    }
}
