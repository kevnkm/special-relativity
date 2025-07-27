using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public bool isEventNode;
    public bool isQuizNode;

    [Tooltip("If event mode is enabled, this GameObject will be triggered.")]
    public GameObject eventObject;

    [TextArea]
    public string dialogueText;

    public List<DialogueChoice> choices;
    public int quizIndex;
    public int allowedTries;
    public DialogueNode answerExplanationNode;

    public bool autoAdvance;
    public DialogueNode nextNode;

    public AudioClip voiceClip;
}
