using System.Collections;
using UnityEngine;

public class Event_10 : MonoBehaviour
{
    private void Start()
    {
        DialogueManager.Instance.UserResponse.SetActive(false);
        Debug.Log($"{gameObject.name} started.");
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        yield return new WaitForSeconds(1f);

        DialogueManager.Instance.Ball.GetComponent<ShowTrajectory>().Hide();
        DialogueManager.Instance.ResetBall();
        DialogueManager.Instance.UserResponse.SetActive(false);
        DialogueManager.Instance.EinsteinOnTrain.SetActive(false);

        DialogueManager.Instance.Train.transform.position = new Vector3(61.5f, -0.57f, -1.32f);
        DialogueManager.Instance.Train.transform.localScale = new Vector3(0.25f, 1, 1);
        DialogueManager.Instance.Train.gameObject.SetActive(true);

        yield return StartCoroutine(DialogueManager.Instance.MoveTrain(new Vector3(-60, 0, 0), 7f));

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }
}
