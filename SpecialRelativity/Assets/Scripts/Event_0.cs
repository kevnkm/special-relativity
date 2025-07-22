using System.Collections;
using UnityEngine;

public class Event_0 : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Walk());
    }

    private IEnumerator Walk()
    {
        yield return new WaitForSeconds(1f);
        DialogueManager.Instance.EinsteinAnimator.SetTrigger("Walk Start");
        yield return new WaitForSeconds(3f);
        DialogueManager.Instance.EinsteinAnimator.SetTrigger("Walk Stop");
    }
}
