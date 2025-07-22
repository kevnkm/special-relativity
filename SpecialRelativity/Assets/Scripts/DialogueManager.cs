using System.Collections;
using TMPro;
using UnityEngine;
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
    private Button autoAdvanceButton;

    [SerializeField]
    private Button[] choiceButtons;

    [SerializeField]
    private Button backgroundButton;

    [SerializeField]
    private TextMeshProUGUI[] choiceTexts;

    [Header("Animation")]
    [SerializeField]
    private Animator einsteinAnimator;
    public Animator EinsteinAnimator
    {
        get { return einsteinAnimator; }
    }

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

        autoAdvanceButton.onClick.AddListener(() =>
        {
            if (currentNode.autoAdvance && currentNode.nextNode != null)
                StartDialogue(currentNode.nextNode);
            else
                Debug.Log("No auto-advance or next node defined.");
        });

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
        if (currentNode.isEventNode)
        {
            dialogueCanvas.gameObject.SetActive(false);
            if (currentNode.eventObject != null)
                Instantiate(currentNode.eventObject);
            else
                Debug.LogWarning("Event object is not assigned in the DialogueNode.");
        }
        else
        {
            dialogueCanvas.gameObject.SetActive(true);
            dialogueText.text = currentNode.dialogueText;

            // 🔊 Play voice clip if present
            if (currentNode.voiceClip != null)
            {
                StartCoroutine(TalkAnimation());
                AudioManager.Instance.Play(currentNode.voiceClip);
            }

            ClearChoices();

            if (currentNode.choices.Count == 0 || currentNode.autoAdvance)
                SetDefaultChoice();
            else
            {
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
        }
    }

    private void SetDefaultChoice()
    {
        autoAdvanceButton.gameObject.SetActive(true);
    }

    private void ClearChoices()
    {
        autoAdvanceButton.gameObject.SetActive(false);

        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }

        choicesContainer.SetActive(false);
    }

    public void StartNextNode()
    {
        if (currentNode.nextNode != null)
            StartDialogue(currentNode.nextNode);
        else
            Debug.Log($"No next node defined for this dialogue: {currentNode.name}");
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

    private IEnumerator TalkAnimation()
    {
        if (currentNode.voiceClip != null)
        {
            float clipLength = currentNode.voiceClip.length;
            float startTime = Time.time;

            while (Time.time - startTime < clipLength)
            {
                int talkIndex = Random.Range(0, 4);
                EinsteinAnimator.SetInteger("TalkIndex", talkIndex);
                EinsteinAnimator.SetTrigger("Talk");

                yield return new WaitUntil(() =>
                {
                    if (Time.time - startTime >= clipLength)
                        return true;

                    AnimatorStateInfo stateInfo = EinsteinAnimator.GetCurrentAnimatorStateInfo(0);
                    return stateInfo.normalizedTime >= 1.0f && !EinsteinAnimator.IsInTransition(0);
                });
            }

            EinsteinAnimator.SetTrigger("Idle");
        }
    }
}
