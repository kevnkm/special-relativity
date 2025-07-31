using System.Collections;
using UnityEngine;

public class Event_3 : MonoBehaviour
{
    private void Start()
    {
        DialogueManager.Instance.UserResponse.SetActive(false);
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        yield return new WaitForSeconds(1f);
        DialogueManager.Instance.EinsteinAnimator.SetTrigger("Left Turn");
        yield return new WaitForSeconds(1f);

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }
}
