using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueChoice))]
public class DialogueChoiceDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 3 lines: choiceText, nextNode, isCorrectChoice + padding
        int lines = 3;
        float spacing = 2f;
        return EditorGUIUtility.singleLineHeight * lines + spacing * (lines - 1) + 6f; // total height
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

        lineRect.y += lineHeight + spacing;
        EditorGUI.PropertyField(lineRect, isCorrectChoice);

        EditorGUI.EndProperty();
    }
}
