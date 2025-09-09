using System.Collections;
using UnityEngine;

public class Event_21 : MonoBehaviour
{
    private void Start()
    {
        DialogueManager.Instance.UserResponse.SetActive(false);
        DialogueManager.Instance.SummaryCanvas.OnToggleButtonClicked += OnToggleButtonClick;
        StartCoroutine(ShowSummaryCanvas());
    }

    private IEnumerator ShowSummaryCanvas()
    {
        DialogueManager.Instance.SummaryCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        DialogueManager.Instance.SummaryCanvas.ShowButtons();
        DialogueManager.Instance.SummaryCanvas.ShowPanel2();
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
