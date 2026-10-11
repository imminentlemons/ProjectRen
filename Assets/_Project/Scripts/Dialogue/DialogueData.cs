
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

    // Where to go when the player presses E
    // or clicks to continue.
    // -1 means end the conversation.
    public int nextNodeIndex = -1;

    public DialogueChoice[] choices;
}



[Serializable]
public class DialogueChoice
{
    public string responseText;
    public int nextNodeIndex = -1;

    [Header("Relationship Changes")]
    public int familiarityChange;
    public int trustChange;
    public int comfortChange;

    [Header("Memory")]
    public string flagToSet;
}

