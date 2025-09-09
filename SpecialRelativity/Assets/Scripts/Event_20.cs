using System.Collections;
using UnityEngine;

public class Event_20 : MonoBehaviour
{
    [Tooltip("The button triggers the first signal.")]
    [SerializeField]
    private DialogueNode explanationNode1;

    [Tooltip("The first signal triggers the second signals.")]
    [SerializeField]
    private DialogueNode explanationNode2;

    [Tooltip("Look, the front gate is triggered first!")]
    [SerializeField]
    private DialogueNode explanationNode3;

    [Tooltip("Now the back gate is triggered!")]
    [SerializeField]
    private DialogueNode explanationNode4;

    [Tooltip("Would you look at that!")]
    [SerializeField]
    private DialogueNode explanationNode5;

    private void Start()
    {
        DialogueManager.Instance.UserResponse.SetActive(false);

        Debug.Log($"{gameObject.name} started.");

        DialogueManager
            .Instance.TrainCollider.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered += HandleTrainTrigger;

        DialogueManager
            .Instance.SignalLightSphere.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered += HandleSignalLightTrigger;

        DialogueManager
            .Instance.RightGateCollider.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered += HandleRightGateTrigger;

        StartCoroutine(EventCoroutine());
    }

    private IEnumerator EventCoroutine()
    {
        var camera = Camera.main.GetComponent<FadeCamera>();

        yield return new WaitForSeconds(1f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeIn, 1f);

        DialogueManager.Instance.Train.transform.position = new Vector3(12f, -0.57f, -1.32f);
        DialogueManager.Instance.Train.transform.localScale = new Vector3(1f, 1, 1);

        DialogueManager.Instance.Platform.transform.localScale = new Vector3(0.5f, 1, 1);
        DialogueManager.Instance.Environment.transform.position = new Vector3(0f, 0f, 0f);

        DialogueManager.Instance.EinsteinAnimator.gameObject.transform.localRotation =
            Quaternion.Euler(0, -90, 0);

        DialogueManager.Instance.PlatformStopwatch.gameObject.SetActive(false);
        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(false);

        DialogueManager.Instance.PlatformButtonObject.SetActive(true);
        DialogueManager.Instance.SignalLightSphereObject.SetActive(true);

        DialogueManager.Instance.TrainFrameAnchor.transform.position = new Vector3(
            DialogueManager.Instance.TrainFrameAnchor.transform.position.x - 4f,
            DialogueManager.Instance.TrainFrameAnchor.transform.position.y,
            DialogueManager.Instance.TrainFrameAnchor.transform.position.z
        );

        DialogueManager.Instance.TrainFrameAnchor.transform.localRotation = Quaternion.Euler(
            0,
            -90,
            0
        );

        DialogueManager.Instance.SignalLightSphere.Reset();
        DialogueManager.Instance.PlatformButtonLightSphere.Reset();

        var anchor = DialogueManager.Instance.TrainFrameAnchor;
        var teleportationProvider = DialogueManager.Instance.TeleportationProvider;

        Utility.LocatePlayer(anchor, teleportationProvider);

        yield return new WaitForSeconds(0.5f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeOut, 1f);
        yield return new WaitForSeconds(3f);

        ///

        StartCoroutine(WaitForThreshold());
        StartCoroutine(WaitForEnvironmentThreshold());

        yield return StartCoroutine(
            DialogueManager.Instance.MoveEnvironment(new Vector3(28f, 0, 0f), 120f)
        );

        ///

        yield return new WaitForSeconds(1f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeIn, 1f);

        DialogueManager.Instance.Train.transform.position = new Vector3(20f, 0f, -1.32f);
        DialogueManager.Instance.Train.transform.localScale = new Vector3(0.25f, 1, 1);

        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(false);

        DialogueManager.Instance.Platform.transform.localScale = new Vector3(1f, 1, 1);

        DialogueManager.Instance.EinsteinAnimator.gameObject.transform.localRotation =
            Quaternion.Euler(0, 0, 0);

        DialogueManager.Instance.PlatformStopwatch.gameObject.SetActive(false);
        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(false);

        DialogueManager.Instance.PlatformButtonObject.SetActive(true);
        DialogueManager.Instance.SignalLightSphereObject.SetActive(true);

        DialogueManager.Instance.Train.gameObject.SetActive(true);

        anchor = DialogueManager.Instance.PlatformFrameAnchor;
        teleportationProvider = DialogueManager.Instance.TeleportationProvider;

        Utility.LocatePlayer(anchor, teleportationProvider);

        yield return new WaitForSeconds(0.5f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeOut, 1f);
        yield return new WaitForSeconds(1f);

        ///

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }

    private void HandleTrainTrigger(Collider other)
    {
        Debug.Log($"Train collided with: {other.name}");
        DialogueManager
            .Instance.TrainCollider.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered -= HandleTrainTrigger;
        DialogueManager.Instance.PlatformButtonLightSphere.TriggerScale(
            new Vector3(50, 50, 50),
            80f
        );
        DialogueManager.Instance.StartNode(explanationNode1);
    }

    private IEnumerator WaitForThreshold()
    {
        while (DialogueManager.Instance.Platform.transform.position.x < 25f)
            yield return null;

        DialogueManager.Instance.StartNode(explanationNode4);

        yield return StartCoroutine(DialogueManager.Instance.LeftGate.CloseGate(0.2f));
        yield return StartCoroutine(DialogueManager.Instance.LeftGate.OpenGate(0.2f));
    }

    private IEnumerator WaitForEnvironmentThreshold()
    {
        while (DialogueManager.Instance.Environment.transform.position.x < 27f)
            yield return null;

        DialogueManager.Instance.StartNode(explanationNode5);
    }

    private void HandleSignalLightTrigger(Collider other)
    {
        Debug.Log($"Signal Light Sphere collided with: {other.name}");
        DialogueManager
            .Instance.SignalLightSphere.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered -= HandleSignalLightTrigger;
        DialogueManager.Instance.StartNode(explanationNode2);
        DialogueManager.Instance.SignalLightSphere.TriggerScale(new Vector3(50, 50, 50), 80f);
    }

    private void HandleRightGateTrigger(Collider other)
    {
        Debug.Log($"Right Gate Collider triggered by: {other.name}");
        if (!other.CompareTag("SignalLightSphere"))
            return;

        DialogueManager
            .Instance.RightGateCollider.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered -= HandleRightGateTrigger;
        DialogueManager.Instance.StartNode(explanationNode3);
        StartCoroutine(
            DialogueManager
                .Instance.RightGateCollider.GetComponent<GateCollisionDetector>()
                .CloseAndOpenGate()
        );
    }

    private void OnDestroy()
    {
        DialogueManager
            .Instance.TrainCollider.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered -= HandleTrainTrigger;
    }
}
