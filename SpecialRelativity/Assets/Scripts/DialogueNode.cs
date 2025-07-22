using System.Collections.Generic;
using UnityEditor;
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

    public bool autoAdvance;
    public DialogueNode nextNode;

    public AudioClip voiceClip;
}

public static class DialogueEditorContext
{
    public static bool isQuizNode;
}

[CustomEditor(typeof(DialogueNode))]
public class DialogueNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty isEventNode = serializedObject.FindProperty("isEventNode");
        SerializedProperty eventObject = serializedObject.FindProperty("eventObject");
        SerializedProperty dialogueText = serializedObject.FindProperty("dialogueText");
        SerializedProperty voiceClip = serializedObject.FindProperty("voiceClip");
        SerializedProperty autoAdvance = serializedObject.FindProperty("autoAdvance");
        SerializedProperty nextNode = serializedObject.FindProperty("nextNode");
        SerializedProperty choices = serializedObject.FindProperty("choices");
        SerializedProperty isQuizNode = serializedObject.FindProperty("isQuizNode");
        SerializedProperty quizIndex = serializedObject.FindProperty("quizIndex");
        SerializedProperty allowedTries = serializedObject.FindProperty("allowedTries");

        EditorGUILayout.PropertyField(isEventNode);
        EditorGUILayout.Space();

        if (isEventNode.boolValue)
        {
            EditorGUILayout.PropertyField(eventObject);
            EditorGUILayout.PropertyField(nextNode);
        }
        else
        {
            EditorGUILayout.PropertyField(dialogueText);
            EditorGUILayout.PropertyField(voiceClip);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(isQuizNode);
            DialogueEditorContext.isQuizNode = isQuizNode.boolValue;

            if (isQuizNode.boolValue)
            {
                EditorGUILayout.PropertyField(quizIndex);
                EditorGUILayout.PropertyField(allowedTries);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Auto-Advance Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(autoAdvance);

            if (autoAdvance.boolValue)
            {
                EditorGUILayout.PropertyField(nextNode);
            }
            else
            {
                EditorGUILayout.PropertyField(choices, true);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomPropertyDrawer(typeof(DialogueChoice))]
public class DialogueChoiceDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lines = DialogueEditorContext.isQuizNode ? 4 : 2;
        return EditorGUIUtility.singleLineHeight * lines + 10f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = 2f;

        var choiceText = property.FindPropertyRelative("choiceText");
        var nextNode = property.FindPropertyRelative("nextNode");
        var isCorrectChoice = property.FindPropertyRelative("isCorrectChoice");

        Rect lineRect = new Rect(position.x, position.y, position.width, lineHeight);
        EditorGUI.PropertyField(lineRect, choiceText);

        lineRect.y += lineHeight + spacing;
        EditorGUI.PropertyField(lineRect, nextNode);

        if (DialogueEditorContext.isQuizNode)
        {
            EditorGUI.indentLevel++;
            lineRect.y += lineHeight + spacing;
            EditorGUI.PropertyField(lineRect, isCorrectChoice);
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }
}
