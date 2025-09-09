using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public TextMeshProUGUI dialogueText;
    public GameObject choicesContainer;
    public DialogueNode startNode;

    [SerializeField]
    private Canvas dialogueCanvas;

    [SerializeField]
    private SummaryCanvas summaryCanvas;
    public SummaryCanvas SummaryCanvas
    {
        get { return summaryCanvas; }
    }

    [SerializeField]
    private GameObject userResponse;
    public GameObject UserResponse
    {
        get { return userResponse; }
    }

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

    [SerializeField]
    private Animator trainEinsteinAnimator;
    public Animator TrainEinsteinAnimator
    {
        get { return trainEinsteinAnimator; }
    }

    [Header("Scene Objects")]
    [SerializeField]
    private GameObject train;
    public GameObject Train
    {
        get { return train; }
    }

    [SerializeField]
    private GameObject platform;
    public GameObject Platform
    {
        get { return platform; }
    }

    [SerializeField]
    private Gate leftGate;
    public Gate LeftGate
    {
        get { return leftGate; }
    }

    [SerializeField]
    private Gate rightGate;
    public Gate RightGate
    {
        get { return rightGate; }
    }

    [SerializeField]
    private GameObject einsteinOnTrain;
    public GameObject EinsteinOnTrain
    {
        get { return einsteinOnTrain; }
    }

    [SerializeField]
    private Stopwatch platformStopwatch;
    public Stopwatch PlatformStopwatch
    {
        get { return platformStopwatch; }
    }

    [SerializeField]
    private Stopwatch trainStopwatch;
    public Stopwatch TrainStopwatch
    {
        get { return trainStopwatch; }
    }

    [Header("Event 2")]
    [SerializeField]
    private GameObject ball;
    public GameObject Ball
    {
        get { return ball; }
    }

    [SerializeField]
    private GameObject einsteinHand;

    private DialogueNode currentNode;

    [Header("Teleportation")]
    [SerializeField]
    private GameObject trainFrameAnchor;
    public GameObject TrainFrameAnchor
    {
        get { return trainFrameAnchor; }
    }

    [SerializeField]
    private GameObject platformFrameAnchor;
    public GameObject PlatformFrameAnchor
    {
        get { return platformFrameAnchor; }
    }

    [SerializeField]
    private TeleportationProvider teleportationProvider;
    public TeleportationProvider TeleportationProvider
    {
        get { return teleportationProvider; }
    }

    [Header("Event 19")]
    [SerializeField]
    private Collider trainCollider;
    public Collider TrainCollider
    {
        get { return trainCollider; }
    }

    [SerializeField]
    private LightSphere platformButtonLightSphere;
    public LightSphere PlatformButtonLightSphere
    {
        get { return platformButtonLightSphere; }
    }

    [SerializeField]
    private LightSphere signalLightSphere;
    public LightSphere SignalLightSphere
    {
        get { return signalLightSphere; }
    }

    [SerializeField]
    private GameObject platformButtonObject;
    public GameObject PlatformButtonObject
    {
        get { return platformButtonObject; }
    }

    [SerializeField]
    private GameObject signalLightSphereObject;
    public GameObject SignalLightSphereObject
    {
        get { return signalLightSphereObject; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        einsteinOnTrain.SetActive(false);
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
            userResponse.SetActive(true);
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

    public void RelocateTrain(Vector3 positionDelta)
    {
        train.transform.position += positionDelta;
    }

    public IEnumerator MoveTrain(Vector3 positionDelta, float duration)
    {
        Vector3 startPosition = train.transform.position;
        Vector3 targetPosition = startPosition + positionDelta;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            train.transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                elapsedTime / duration
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        train.transform.position = targetPosition;
    }

    public IEnumerator MoveTrainSmooth(Vector3 positionDelta, float duration)
    {
        Vector3 startPosition = train.transform.position;
        Vector3 targetPosition = startPosition + positionDelta;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            train.transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                Mathf.SmoothStep(0f, 1f, elapsedTime / duration)
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        train.transform.position = targetPosition;
    }

    public IEnumerator MovePlatform(Vector3 positionDelta, float duration)
    {
        Vector3 startPosition = platform.transform.position;
        Vector3 targetPosition = startPosition + positionDelta;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            platform.transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                elapsedTime / duration
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        platform.transform.position = targetPosition;
    }

    public void ReleaseBall(Vector3 force)
    {
        ball.transform.SetParent(train.transform);
        var ballRigidbody = ball.GetComponent<Rigidbody>();
        ballRigidbody.isKinematic = false;
        ballRigidbody.AddForce(force, ForceMode.Impulse);
    }

    public void ResetBall()
    {
        ball.transform.SetParent(einsteinHand.transform);
        ball.transform.localPosition = Vector3.zero;
    }
}
