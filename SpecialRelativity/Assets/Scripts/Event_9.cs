using System.Collections;
using UnityEngine;

public class Event_9 : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;

    private void Start()
    {
        Debug.Log($"{gameObject.name} started.");
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        yield return new WaitForSeconds(1f);

        AudioManager.Instance.Play(audioClip);
        yield return new WaitForSeconds(audioClip.length);

        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }
}
