
using UnityEngine;

public class RecordShelfInteractable : MonoBehaviour, IInteractable
{
    [Header("Records")]
    [SerializeField]
    private string[] records =
    {
        "Midnight Static - The Hollow Hours",
        "Pale Neon - Afterimage",
        "Glass Echo - Rain on Tape"
    };

    [Header("UI")]
    [SerializeField] private RecordBrowseUI browseUI;

    public string InteractionPrompt => "Browse Records";

    public void Interact()
    {
        if (browseUI == null)
        {
            Debug.LogWarning("Record Browse UI not assigned!");
            return;
        }

        browseUI.Open(records);
    }
}
