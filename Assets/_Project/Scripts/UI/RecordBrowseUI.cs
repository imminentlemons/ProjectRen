
using UnityEngine;
using TMPro;

public class RecordBrowseUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text albumText;
    [SerializeField] private TMP_Text artistText;
    [SerializeField] private TMP_Text recordCounterText;

    [Header("Player References")]
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private MonoBehaviour playerMovement;

    private string[] records;
    private int currentIndex;

    private bool movementWasEnabled;
    private CursorLockMode previousCursorLock;
    private bool previousCursorVisible;

    public void Open(string[] shelfRecords)
    {
        if (shelfRecords == null || shelfRecords.Length == 0)
            return;

        records = shelfRecords;
        currentIndex = 0;

        movementWasEnabled = playerMovement != null &&
                             playerMovement.enabled;

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerInteraction != null)
            playerInteraction.SetInteractionEnabled(false);

        previousCursorLock = Cursor.lockState;
        previousCursorVisible = Cursor.visible;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameObject.SetActive(true);
        RefreshDisplay();
    }

    public void NextRecord()
    {
        if (records == null || records.Length == 0)
            return;

        currentIndex = (currentIndex + 1) % records.Length;
        RefreshDisplay();
    }

    public void PreviousRecord()
    {
        if (records == null || records.Length == 0)
            return;

        currentIndex =
            (currentIndex - 1 + records.Length) % records.Length;

        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        string record = records[currentIndex];

        int separator = record.IndexOf(" - ");

        if (separator >= 0)
        {
            artistText.text = record.Substring(0, separator);
            albumText.text = record.Substring(separator + 3);
        }
        else
        {
            albumText.text = record;
            artistText.text = "Unknown Artist";
        }

        recordCounterText.text =
            $"{currentIndex + 1} / {records.Length}";
    }

    public void Close()
    {
        gameObject.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = movementWasEnabled;

        if (playerInteraction != null)
            playerInteraction.SetInteractionEnabled(true);

        Cursor.lockState = previousCursorLock;
        Cursor.visible = previousCursorVisible;
    }
}
