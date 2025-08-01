using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryCanvas : MonoBehaviour
{
    public Action OnToggleButtonClicked;

    [SerializeField]
    private GameObject panel1;

    [SerializeField]
    private GameObject panel2;

    [SerializeField]
    private Button toggleButton;

    [SerializeField]
    private TextMeshProUGUI toggleText;

    private void Start()
    {
        toggleButton.onClick.AddListener(() => OnToggleButtonClicked?.Invoke());
        toggleButton.onClick.AddListener(TogglePanelVisibility);
    }

    private bool isPanelVisible = true;

    // public void TogglePanelVisibility()
    // {

    //     isPanelVisible = !isPanelVisible;

    //     // If panel1 is active, deactivate it
    //     if (panel1.activeSelf)
    //     {
    //         panel1.SetActive(isPanelVisible);
    //     }

    //     // Else if panel2 is active, deactivate it
    //     else if (panel2.activeSelf)
    //     {
    //         panel2.SetActive(isPanelVisible);
    //     }
    //     // panel.SetActive(isPanelVisible);
    //     toggleText.text = isPanelVisible ? "Hide Summary" : "Show Summary";
    //     gameObject.GetComponent<FollowCamera>().yPos = isPanelVisible ? 1.2f : 1.8f;
    // }

    public void TogglePanelVisibility()
    {
        isPanelVisible = !isPanelVisible;
        panel1.SetActive(isPanelVisible);
        toggleText.text = isPanelVisible ? "Hide Summary" : "Show Summary";
        gameObject.GetComponent<FollowCamera>().yPos = isPanelVisible ? 1.2f : 1.8f;
    }


   
    // public void TogglePanelVisibility()
    // {
    //     bool anyPanelVisible = panel1.activeSelf || panel2.activeSelf;

    //     if (anyPanelVisible)
    //     {
    //         // Hide whichever one is currently active
    //         if (panel1.activeSelf) panel1.SetActive(false);
    //         if (panel2.activeSelf) panel2.SetActive(false);
    //     }
    //     else
    //     {
    //         // If nothing is active, show panel1 by default
    //         panel1.SetActive(true);
    //     }

    //     // Update status after toggling
    //     bool updatedVisibility = panel1.activeSelf || panel2.activeSelf;
    //     toggleText.text = updatedVisibility ? "Hide Summary" : "Show Summary";
    //     gameObject.GetComponent<FollowCamera>().yPos = updatedVisibility ? 1.2f : 1.8f;
    // }
}
