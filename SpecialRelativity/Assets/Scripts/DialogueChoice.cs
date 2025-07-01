using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueNode nextNode;
}

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    [TextArea]
    public string dialogueText;

    public List<DialogueChoice> choices;

    [Header("Optional Events")]
    public UnityEvent onNodeEnter;
}
