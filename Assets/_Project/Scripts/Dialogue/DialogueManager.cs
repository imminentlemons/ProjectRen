
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Data")]
    [SerializeField] private DialogueData renDialogue;

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject choicesContainer;
    [SerializeField] private TMP_Text speakerName;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button[] choiceButtons;

    [Header("Player")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInteraction playerInteraction;

    private DialogueData currentDialogue;
    private bool dialogueActive;
    private int currentNodeIndex;
    private int nodeShownFrame;

    private void Start()
    {
        dialoguePanel.SetActive(false);
        choicesContainer.SetActive(false);
    }

    // Keep this name so RenInteraction still works!
    public void StartRenDialogue()
    {
        StartDialogue(renDialogue);
    }

    public void StartDialogue(DialogueData dialogue, int startNodeIndex = 0)
    {
        if (dialogueActive || dialogue == null ||
            dialogue.nodes == null ||
            dialogue.nodes.Length == 0)
            return;

        currentDialogue = dialogue;
        dialogueActive = true;

        playerMovement.SetDialogueMode(true);
        playerInteraction.SetInteractionEnabled(false);

        dialoguePanel.SetActive(true);

        ShowNode(startNodeIndex);
    }


    private bool CanShowChoice(DialogueChoice choice)
    {
        RelationshipManager relationship =
            RelationshipManager.Instance;

        if (!string.IsNullOrEmpty(choice.requiredFlag))
        {
            if (relationship == null ||
                !relationship.HasFlag(choice.requiredFlag))
                return false;
        }

        if (!string.IsNullOrEmpty(choice.forbiddenFlag))
        {
            if (relationship != null &&
                relationship.HasFlag(choice.forbiddenFlag))
                return false;
        }

        return true;
    }

    private void ShowNode(int index)
    {
        if (index < 0 ||
            index >= currentDialogue.nodes.Length)
        {
            EndDialogue();
            return;
        }

        currentNodeIndex = index;
        nodeShownFrame = Time.frameCount;

        DialogueNode node =
            currentDialogue.nodes[currentNodeIndex];

        speakerName.text = node.speakerName;
        dialogueText.text = node.dialogueText;

        // Hide previous choices
        foreach (Button button in choiceButtons)
            button.gameObject.SetActive(false);


        int visibleCount = 0;

        if (node.choices != null)
        {
            foreach (DialogueChoice choice in node.choices)
            {
                if (!CanShowChoice(choice))
                    continue;

                if (visibleCount >= choiceButtons.Length)
                {
                    Debug.LogWarning(
                        "More available dialogue choices than UI buttons!");
                    break;
                }

                Button button = choiceButtons[visibleCount];
                button.gameObject.SetActive(true);

                button.GetComponentInChildren<TMP_Text>().text =
                    choice.responseText;

                DialogueChoice selectedChoice = choice;

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                    SelectChoice(selectedChoice));

                visibleCount++;
            }
        }

        choicesContainer.SetActive(visibleCount > 0);

    }


    private void SelectChoice(DialogueChoice choice)
    {
        RelationshipManager relationship =
            RelationshipManager.Instance;

        if (relationship != null)
        {
            relationship.ModifyRelationship(
                choice.familiarityChange,
                choice.trustChange,
                choice.comfortChange
            );

            if (!string.IsNullOrEmpty(choice.flagToSet))
            {
                relationship.SetFlag(choice.flagToSet);
            }
        }

        if (choice.nextNodeIndex == -1)
        {
            EndDialogue();
        }
        else
        {
            ShowNode(choice.nextNodeIndex);
        }
    }


    private void Update()
    {
        if (!dialogueActive)
            return;

        // Don't close dialogue on the same frame
        // that a choice button was clicked.
        if (Time.frameCount == nodeShownFrame)
            return;

        // Wait for a response when choices are visible.
        if (choicesContainer.activeSelf)
            return;

        bool ePressed = Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame;

        bool mouseClicked = Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame;


        if (ePressed || mouseClicked)
        {
            DialogueNode currentNode =
                currentDialogue.nodes[currentNodeIndex];

            if (currentNode.nextNodeIndex >= 0)
            {
                ShowNode(currentNode.nextNodeIndex);
            }
            else
            {
                EndDialogue();
            }
        }

    }

    public void EndDialogue()
    {
        if (!dialogueActive)
            return;

        dialogueActive = false;
        currentDialogue = null;

        dialoguePanel.SetActive(false);
        choicesContainer.SetActive(false);

        playerMovement.SetDialogueMode(false);
        playerInteraction.SetInteractionEnabled(true);
    }
}
