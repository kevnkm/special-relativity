using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject choicesContainer;
    public Button choiceButtonPrefab;

    private DialogueNode currentNode;

    public void StartDialogue(DialogueNode startNode)
    {
        currentNode = startNode;
        ShowNode();
    }

    private void ShowNode()
    {
        dialogueText.text = currentNode.dialogueText;

        currentNode.onNodeEnter?.Invoke();

        foreach (Transform child in choicesContainer.transform)
            Destroy(child.gameObject);

        foreach (var choice in currentNode.choices)
        {
            var button = Instantiate(choiceButtonPrefab, choicesContainer.transform);
            button.GetComponentInChildren<Text>().text = choice.choiceText;
            button.onClick.AddListener(() => StartDialogue(choice.nextNode));
        }
    }
}
