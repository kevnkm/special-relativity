using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public TextMeshProUGUI dialogueText;
    public GameObject choicesContainer;
    public DialogueNode startNode;

    [SerializeField]
    private Canvas dialogueCanvas;

    [Header("Choice")]
    [SerializeField]
    private Button[] choiceButtons;

    [SerializeField]
    private Button backgroundButton;

    [SerializeField]
    private TextMeshProUGUI[] choiceTexts;

    [Header("Optional Events")]
    [SerializeField]
    private UnityEvent onTrainEnter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private DialogueNode currentNode;

    public void Start()
    {
        backgroundButton.onClick.AddListener(() => OnBackgroundClick());

        TypewriterEffect.TextHeightUpdated += (float height) =>
        {
            StartCoroutine(UpdateSizeNextFrame(height));
        };

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            var index = i;
            choiceButtons[i].onClick.AddListener(() => OnButtonClick(index));
            choiceButtons[i].gameObject.SetActive(false);
        }

        // Listen to CompleteTextRevealed event to show choices
        TypewriterEffect.CompleteTextRevealed += RevealChoices;

        StartDialogue(startNode);
    }

    public void StartDialogue(DialogueNode startNode)
    {
        currentNode = startNode;
        ShowNode();
    }

    private void ShowNode()
    {
        dialogueText.text = currentNode.dialogueText;
        currentNode.onNodeEnter?.Invoke();

        ClearChoices();

        if (currentNode.choices.Count == 0)
        {
            choiceButtons[0].gameObject.SetActive(true);
            choiceTexts[0].text = "Click anywhere to continue";

            choiceTexts[0].ForceMeshUpdate();

            choiceButtons[0].GetComponent<RectTransform>().sizeDelta = new Vector2(
                choiceButtons[0].GetComponent<RectTransform>().sizeDelta.x,
                choiceTexts[0].preferredHeight + 50f
            );
        }

        for (int i = 0; i < currentNode.choices.Count; i++)
        {
            if (i >= choiceButtons.Length)
            {
                Debug.LogWarning("Not enough choice buttons available.");
                break;
            }

            choiceButtons[i].gameObject.SetActive(true);
            choiceTexts[i].text = currentNode.choices[i].choiceText;

            choiceTexts[i].ForceMeshUpdate();

            choiceButtons[i].GetComponent<RectTransform>().sizeDelta = new Vector2(
                choiceButtons[i].GetComponent<RectTransform>().sizeDelta.x,
                choiceTexts[i].preferredHeight + 50f
            );
        }
    }

    private void ClearChoices()
    {
        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }

        choicesContainer.SetActive(false);
    }

    private void OnButtonClick(int choiceIndex)
    {
        if (currentNode.autoAdvance && currentNode.nextNode != null)
        {
            StartDialogue(currentNode.nextNode);
            return;
        }

        if (choiceIndex < 0 || choiceIndex >= currentNode.choices.Count)
        {
            Debug.LogWarning("Invalid choice index.");
            return;
        }

        DialogueChoice selectedChoice = currentNode.choices[choiceIndex];
        if (selectedChoice.nextNode != null)
            StartDialogue(selectedChoice.nextNode);
        else
            Debug.Log("No next node defined for this choice.");
    }

    private void OnBackgroundClick()
    {
        if (currentNode.autoAdvance && currentNode.nextNode != null)
        {
            StartDialogue(currentNode.nextNode);
        }
        else
        {
            Debug.Log("No auto-advance or next node defined.");
        }
    }

    private void RevealChoices()
    {
        choicesContainer.SetActive(true);
    }

    private IEnumerator UpdateSizeNextFrame(float height)
    {
        yield return null; // wait until next frame
        var rectTransform = dialogueCanvas.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height + 220f);
    }
}
