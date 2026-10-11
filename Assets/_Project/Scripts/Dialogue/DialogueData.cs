
using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewDialogue",
    menuName = "Project Ren/Dialogue"
)]
public class DialogueData : ScriptableObject
{
    public DialogueNode[] nodes;
}

[Serializable]
public class DialogueNode
{
    public string speakerName;

    [TextArea(3, 6)]
    public string dialogueText;

    public DialogueChoice[] choices;
}

[Serializable]
public class DialogueChoice
{
    public string responseText;

    // Index of the node this response leads to.
    // -1 means the conversation ends.
    public int nextNodeIndex = -1;
}
