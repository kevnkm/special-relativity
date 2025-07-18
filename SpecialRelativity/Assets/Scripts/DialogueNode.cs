using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    [TextArea]
    public string dialogueText;

    public List<DialogueChoice> choices;

    public bool autoAdvance;
    public DialogueNode nextNode;

    [Header("Optional Events")]
    public UnityEvent onNodeEnter;
}

[CustomEditor(typeof(DialogueNode))]
public class DialogueNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty dialogueText = serializedObject.FindProperty("dialogueText");
        SerializedProperty choices = serializedObject.FindProperty("choices");
        SerializedProperty autoAdvance = serializedObject.FindProperty("autoAdvance");
        SerializedProperty nextNode = serializedObject.FindProperty("nextNode");

        EditorGUILayout.PropertyField(dialogueText);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Auto-Advance Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(autoAdvance);

        if (autoAdvance.boolValue)
            EditorGUILayout.PropertyField(nextNode);
        else
            EditorGUILayout.PropertyField(choices, true);

        serializedObject.ApplyModifiedProperties();
    }
}
