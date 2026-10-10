using UnityEngine;

public class RenInteraction : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => "Talk to Ren";

    public void Interact()
    {
        Debug.Log("Ren: You're late.");
    }
}