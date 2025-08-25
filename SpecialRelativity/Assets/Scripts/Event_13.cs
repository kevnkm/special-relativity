using System.Collections;
using UnityEngine;

public class Event_13 : MonoBehaviour
{
    private void Start()
    {
        Debug.Log($"{gameObject.name} started.");
        DialogueManager.Instance.UserResponse.SetActive(false);
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        yield return new WaitForSeconds(1f);
        DialogueManager.Instance.Train.transform.localScale = new Vector3(0.25f, 1, 1);

        DialogueManager.Instance.TrainStopwatch.transform.localScale = new Vector3(2.5f, 1, 1);
        DialogueManager.Instance.TrainStopwatch.transform.localEulerAngles = new Vector3(0, 180, 0);

        DialogueManager.Instance.TrainStopwatch.gameObject.SetActive(true);
        DialogueManager.Instance.TrainStopwatch.GetComponent<FaceCamera>().enabled = false;
        DialogueManager.Instance.PlatformStopwatch.gameObject.SetActive(true);

        DialogueManager.Instance.Ball.SetActive(false);
        DialogueManager.Instance.EinsteinOnTrain.SetActive(true);

        DialogueManager.Instance.Train.transform.position = new Vector3(60f, -0.57f, -1.32f);

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(
            DialogueManager.Instance.MoveTrainSmooth(new Vector3(-40, 0, 0), 3f)
        );

        Debug.Log("Transitioning to the next event.");
        DialogueManager.Instance.StartNextNode();
        Destroy(gameObject);
    }
}
