using UnityEditor;

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
        SerializedProperty answerExplanationNode = serializedObject.FindProperty(
            "answerExplanationNode"
        );

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

            if (isQuizNode.boolValue)
            {
                EditorGUILayout.PropertyField(quizIndex);
                EditorGUILayout.PropertyField(allowedTries);
                EditorGUILayout.PropertyField(answerExplanationNode);
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
