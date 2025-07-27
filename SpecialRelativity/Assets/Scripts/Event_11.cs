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

        DialogueManager.Instance.Train.transform.localScale = new Vector3(0.5f, 1, 1);
        DialogueManager.Instance.Train.gameObject.SetActive(true);

        yield return StartCoroutine(DialogueManager.Instance.MoveTrain(new Vector3(25, 0, 0), 3f));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(DialogueManager.Instance.MoveTrain(new Vector3(-40, 0, 0), 8f));

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }
}
