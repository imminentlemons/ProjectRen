

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject choicesContainer;
    [SerializeField] private TMP_Text speakerName;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button[] choiceButtons;

    [Header("Player")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInteraction playerInteraction;

    private bool dialogueActive;

    private void Start()
    {
        dialoguePanel.SetActive(false);
        choicesContainer.SetActive(false);
    }

    public void StartRenDialogue()
    {
        if (dialogueActive)
            return;

        dialogueActive = true;

        playerMovement.SetDialogueMode(true);
        playerInteraction.SetInteractionEnabled(false);

        dialoguePanel.SetActive(true);
        choicesContainer.SetActive(true);

        speakerName.text = "REN";
        dialogueText.text = "You're late.";

        SetChoice(0, "Late for what?", 0);
        SetChoice(1, "Miss me that much?", 1);
        SetChoice(2, "Sorry, got held up.", 2);
    }

    private void SetChoice(int index, string text, int choice)
    {
        Button button = choiceButtons[index];

        button.gameObject.SetActive(true);
        button.GetComponentInChildren<TMP_Text>().text = text;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => SelectChoice(choice));
    }

    private void SelectChoice(int choice)
    {
        switch (choice)
        {
            case 0:
                dialogueText.text =
                    "Ren glances up. \"You're asking me?\"";
                break;

            case 1:
                dialogueText.text =
                    "\"Don't flatter yourself.\" He looks away.";
                break;

            case 2:
                dialogueText.text =
                    "\"Didn't ask for an excuse.\"";
                break;
        }

        choicesContainer.SetActive(false);
    }

    public void EndDialogue()
    {
        dialogueActive = false;

        dialoguePanel.SetActive(false);
        choicesContainer.SetActive(false);

        playerMovement.SetDialogueMode(false);
        playerInteraction.SetInteractionEnabled(true);
    }

    private void Update()
    {
        if (dialogueActive &&
            !choicesContainer.activeSelf &&
            UnityEngine.InputSystem.Keyboard.current != null &&
            UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            EndDialogue();
        }
    }
}
