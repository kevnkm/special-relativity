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
        DialogueManager.Instance.UserResponse.SetActive(false);
        StartCoroutine(EventCoroutine());
    }

    private IEnumerator EventCoroutine()
    {
        yield return new WaitForSeconds(1f);
        DialogueManager.Instance.PlatformStopwatch.gameObject.SetActive(false);
        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(false);

        yield return StartCoroutine(GateEventCoroutine());

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }

    private IEnumerator GateEventCoroutine()
    {
        var moveTime = 10f;
        Debug.Log("Right Gate Close by Event17");
        StartCoroutine(DialogueManager.Instance.RightGate.CloseGate(0.2f));
        StartCoroutine(DialogueManager.Instance.RightGate.OpenGate(0.2f));

        StartCoroutine(DialogueManager.Instance.MovePlatform(new Vector3(16, 0, 0), moveTime));

        yield return new WaitForSeconds(moveTime * 0.9f);

        Debug.Log("Left Gate Close by Event17");
        yield return StartCoroutine(DialogueManager.Instance.LeftGate.CloseGate(0.2f));
        yield return StartCoroutine(DialogueManager.Instance.LeftGate.OpenGate(0.2f));
    }
}
