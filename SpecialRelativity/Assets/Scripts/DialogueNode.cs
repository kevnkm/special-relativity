using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    [TextArea]
    public string dialogueText;

    public List<DialogueChoice> choices;

    [Header("Optional Events")]
    public UnityEvent onNodeEnter;
}
