using System.Collections;
using UnityEngine;

public class Event_14 : MonoBehaviour
{
    private void Start()
    {
        Debug.Log($"{gameObject.name} started.");
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(DialogueManager.Instance.TrainStopwatch.CountToTime(1.49f, 2f));
        StartCoroutine(DialogueManager.Instance.PlatformStopwatch.CountToTime(2f, 2f));
        yield return StartCoroutine(DialogueManager.Instance.MoveTrain(new Vector3(-9, 0, 0), 2f));

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }
}
