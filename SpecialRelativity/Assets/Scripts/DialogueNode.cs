using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public bool isEventNode;
    public bool isQuizNode;

    [Tooltip("If is event node, this GameObject will be instantiated.")]
    public GameObject eventObject;

    [TextArea]
    public string dialogueText;

    public List<DialogueChoice> choices;
    public int quizIndex;

    public bool autoAdvance;
    public DialogueNode nextNode;

    public AudioClip voiceClip;
}
