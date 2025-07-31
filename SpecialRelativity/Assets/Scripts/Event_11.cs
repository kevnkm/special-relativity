using System.Collections;
using UnityEngine;

public class Event_11 : MonoBehaviour
{
    private void Start()
    {
        Debug.Log($"{gameObject.name} started.");
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        yield return new WaitForSeconds(1f);

        DialogueManager.Instance.Train.transform.localScale = new Vector3(0.25f, 1, 1);
        DialogueManager.Instance.Train.gameObject.SetActive(true);

        yield return StartCoroutine(
            DialogueManager.Instance.MoveTrainSmooth(new Vector3(25, 0, 0), 3f)
        );
        yield return new WaitForSeconds(1f);

        StartCoroutine(CloseAndOpenGate());
        yield return StartCoroutine(DialogueManager.Instance.MoveTrain(new Vector3(-80, 0, 0), 8f));

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }

    private IEnumerator CloseAndOpenGate()
    {
        while (DialogueManager.Instance.Train.transform.position.x > 2.5f)
        {
            yield return null;
        }

        Debug.Log("Closing gate.");
        StartCoroutine(DialogueManager.Instance.LeftGate.CloseGate(0.2f));
        yield return StartCoroutine(DialogueManager.Instance.RightGate.CloseGate(0.2f));

        Debug.Log("Opening gate.");
        StartCoroutine(DialogueManager.Instance.LeftGate.OpenGate(0.2f));
        yield return StartCoroutine(DialogueManager.Instance.RightGate.OpenGate(0.2f));
    }
}
