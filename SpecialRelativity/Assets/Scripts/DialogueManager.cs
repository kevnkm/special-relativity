using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public TextMeshProUGUI dialogueText;
    public GameObject choicesContainer;
    public Button choiceButtonPrefab;
    public DialogueNode startNode;

    [Header("Choice")]
    [SerializeField]
    private Button[] choiceButtons;

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
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            var index = i;
            choiceButtons[i].onClick.AddListener(() => OnButtonClick(index));
            choiceButtons[i].gameObject.SetActive(false);
        }

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
    }

    private int OnButtonClick(int choiceIndex)
    {
        if (choiceIndex < 0 || choiceIndex >= currentNode.choices.Count)
        {
            Debug.LogWarning("Invalid choice index.");
            return -1;
        }

        return choiceIndex;
    }
}
