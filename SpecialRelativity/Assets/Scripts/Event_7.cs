using System.Collections;
using UnityEngine;

public class Event_7 : MonoBehaviour
{
    private void Start()
    {
        DialogueManager.Instance.UserResponse.SetActive(false);
        DialogueManager.Instance.SummaryCanvas.OnToggleButtonClicked += OnToggleButtonClick;
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        DialogueManager.Instance.SummaryCanvas.gameObject.SetActive(true);
        yield return StartCoroutine(
            DialogueManager.Instance.MoveTrainSmooth(new Vector3(50f, 0f, 0f), 3f)
        );
    }

    private void OnToggleButtonClick()
    {
        DialogueManager.Instance.StartNextNode();
        DialogueManager.Instance.UserResponse.SetActive(true);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.SummaryCanvas.OnToggleButtonClicked -= OnToggleButtonClick;
        }
    }
}
