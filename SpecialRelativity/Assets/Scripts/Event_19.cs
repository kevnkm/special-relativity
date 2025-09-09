using System.Collections;
using UnityEngine;

public class Event_19 : MonoBehaviour
{
    [Tooltip("Here comes the train.")]
    [SerializeField]
    private DialogueNode explanationNode1;

    [Tooltip("The button triggers the first signal")]
    [SerializeField]
    private DialogueNode explanationNode2;

    [Tooltip("The first signal triggers the second signals.")]
    [SerializeField]
    private DialogueNode explanationNode3;

    [Tooltip("The second signals trigger the gates.")]
    [SerializeField]
    private DialogueNode explanationNode4;

    [Tooltip("The train leaves.")]
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
            .Instance.LeftGateCollider.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered += HandleLeftGateTrigger;

        DialogueManager
            .Instance.RightGateCollider.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered += HandleRightGateTrigger;

        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        yield return new WaitForSeconds(1f);

        DialogueManager.Instance.StartNode(explanationNode1);

        DialogueManager.Instance.Train.transform.position = new Vector3(16f, 0f, -1.32f);
        StartCoroutine(StartLastNodeWithCondition());
        yield return StartCoroutine(
            DialogueManager.Instance.MoveTrain(new Vector3(-30f, 0, 0f), 27f)
        );

        yield return new WaitForSeconds(15f); // wait for the rest of the visualization

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }

    private void HandleTrainTrigger(Collider other)
    {
        Debug.Log($"Train collided with: {other.name}");
        DialogueManager.Instance.StartNode(explanationNode2);
        DialogueManager.Instance.PlatformButtonLightSphere.TriggerScale(new Vector3(50, 50, 50));
        DialogueManager
            .Instance.TrainCollider.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered -= HandleTrainTrigger;
    }

    private void HandleSignalLightTrigger(Collider other)
    {
        Debug.Log($"Signal Light Sphere collided with: {other.name}");
        DialogueManager.Instance.StartNode(explanationNode3);
        DialogueManager
            .Instance.SignalLightSphere.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered -= HandleSignalLightTrigger;
    }

    private void HandleLeftGateTrigger(Collider other)
    {
        Debug.Log($"Left Gate collided with: {other.name}");
        DialogueManager
            .Instance.LeftGateCollider.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered -= HandleLeftGateTrigger;
        DialogueManager.Instance.StartNode(explanationNode4);
        StartCoroutine(
            DialogueManager
                .Instance.LeftGateCollider.GetComponent<GateCollisionDetector>()
                .CloseAndOpenGate()
        );
    }

    private void HandleRightGateTrigger(Collider other)
    {
        Debug.Log($"Right Gate collided with: {other.name}");
        DialogueManager
            .Instance.RightGateCollider.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered -= HandleRightGateTrigger;
        StartCoroutine(
            DialogueManager
                .Instance.RightGateCollider.GetComponent<GateCollisionDetector>()
                .CloseAndOpenGate()
        );
    }

    private IEnumerator StartLastNodeWithCondition()
    {
        // Wait until DialogueManager.Instance.Train's x position is less than -2
        while (DialogueManager.Instance.Train.transform.position.x >= -2)
            yield return null;
        DialogueManager.Instance.StartNode(explanationNode5);
    }

    private void OnDestroy()
    {
        DialogueManager
            .Instance.TrainCollider.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered -= HandleTrainTrigger;
        DialogueManager
            .Instance.SignalLightSphere.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered -= HandleSignalLightTrigger;
        DialogueManager
            .Instance.LeftGateCollider.gameObject.GetComponent<CollisionDetector>()
            .OnTriggerEntered -= HandleLeftGateTrigger;
    }
}
