using System.Collections;
using UnityEngine;

/// <summary>
/// Unfreeze frame, train moves forward,
/// front gate closes and opens,
/// then back gate closes and opens
/// once the back of the train makes it through.
/// </summary>
public class Event_17 : MonoBehaviour
{
    private void Start()
    {
        Debug.Log($"{gameObject.name} started.");
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        yield return new WaitForSeconds(1f);
        DialogueManager.Instance.PlatformStopwatch.gameObject.SetActive(false);
        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(false);

        if (DialogueManager.Instance.Platform.transform.position.x >= 32f)
        {
            DialogueManager.Instance.Platform.transform.position = new Vector3(16f, -0.57f, 0f);
        }

        // Now gates can wait for position change *while* movement happens
        yield return StartCoroutine(CloseAndOpenGate());

        // Ensure movement completes
        yield return StartCoroutine(
            DialogueManager.Instance.MovePlatform(new Vector3(16, 0, 0), 10f)
        );

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }

    private IEnumerator CloseAndOpenGate()
    {
        Debug.Log("Right Gate Close by Event17");
        yield return StartCoroutine(DialogueManager.Instance.RightGate.CloseGate(0.2f));
        yield return StartCoroutine(DialogueManager.Instance.RightGate.OpenGate(0.2f));

        // yield return new WaitForSeconds(10f);
        while (DialogueManager.Instance.Platform.transform.position.x < 32f)
        {
            yield return null;
        }

        Debug.Log("Left Gate Close by Event17");
        yield return StartCoroutine(DialogueManager.Instance.LeftGate.CloseGate(0.2f));
        yield return StartCoroutine(DialogueManager.Instance.LeftGate.OpenGate(0.2f));
    }
}
