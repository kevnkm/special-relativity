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

        StartCoroutine(CloseAndOpenGate());
        yield return StartCoroutine(
            DialogueManager.Instance.MoveEnvironment(new Vector3(6, 0, 0), 10f)
        );

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }

    private IEnumerator CloseAndOpenGate()
    {
        yield return StartCoroutine(DialogueManager.Instance.RightGate.CloseGate(0.2f));
        yield return StartCoroutine(DialogueManager.Instance.RightGate.OpenGate(0.2f));

        yield return new WaitForSeconds(7f);
        while (DialogueManager.Instance.Environment.transform.position.x < 0f)
        {
            yield return null;
        }

        yield return StartCoroutine(DialogueManager.Instance.RightGate.CloseGate(0.2f));
        yield return StartCoroutine(DialogueManager.Instance.RightGate.OpenGate(0.2f));
    }
}
