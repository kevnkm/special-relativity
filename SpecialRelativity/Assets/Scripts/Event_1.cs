using System.Collections;
using UnityEngine;

public class Event_1 : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(
            DialogueManager.Instance.MoveTrainSmooth(new Vector3(12, 0, 0), 2f)
        );
        yield return new WaitForSeconds(1f);

        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }
}
