
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

    private int currentRecordIndex = 0;

    public string InteractionPrompt => "Browse Records";

    public void Interact()
    {
        if (records == null || records.Length == 0)
        {
            Debug.Log("This shelf is empty.");
            return;
        }

        Debug.Log($"Browsing: {records[currentRecordIndex]}");

        currentRecordIndex =
            (currentRecordIndex + 1) % records.Length;
    }
}
