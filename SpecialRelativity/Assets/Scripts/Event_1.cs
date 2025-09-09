using System.Collections;
using UnityEngine;

public class Event_1 : MonoBehaviour
{
    private void Start()
    {
        DialogueManager.Instance.UserResponse.SetActive(false);
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(
            DialogueManager.Instance.MoveTrainSmooth(new Vector3(-50, 0, 0), 3f)
        );
        yield return new WaitForSeconds(1f);

        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }
}
