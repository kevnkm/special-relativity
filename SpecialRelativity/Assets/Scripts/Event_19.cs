using System.Collections;
using UnityEngine;

public class Event_19 : MonoBehaviour
{
    private TrainCollisionDetector listener;

    private void Start()
    {
        DialogueManager.Instance.UserResponse.SetActive(false);

        Debug.Log($"{gameObject.name} started.");

        listener =
            DialogueManager.Instance.TrainCollider.gameObject.GetComponent<TrainCollisionDetector>();
        listener.OnTriggerEntered += HandleTrainTrigger;

        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        yield return new WaitForSeconds(1f);

        DialogueManager.Instance.Train.transform.position = new Vector3(20f, 0f, -1.32f);
        yield return StartCoroutine(
            DialogueManager.Instance.MoveTrain(new Vector3(-30f, 0, 0f), 5f)
        );

        yield return new WaitForSeconds(5f); // wait for the rest of the visualization

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }

    private void HandleTrainTrigger(Collider other)
    {
        Debug.Log($"Train collided with: {other.name}");
        DialogueManager.Instance.PlatformButtonLightSphere.TriggerScale(
            new Vector3(100, 100, 100),
            8f
        );
        listener.OnTriggerEntered -= HandleTrainTrigger;
    }

    private void OnDestroy()
    {
        if (listener != null)
            listener.OnTriggerEntered -= HandleTrainTrigger;
    }
}
