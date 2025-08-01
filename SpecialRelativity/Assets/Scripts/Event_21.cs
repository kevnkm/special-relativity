using System.Collections;
using UnityEngine;

public class Event_21 : MonoBehaviour
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
        DialogueManager.Instance.SummaryCanvas.ShowPanel2();
        yield return new WaitForSeconds(1f);
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
