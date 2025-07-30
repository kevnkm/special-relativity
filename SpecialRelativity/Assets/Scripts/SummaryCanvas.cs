using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryCanvas : MonoBehaviour
{
    public Action OnToggleButtonClicked;

    [SerializeField]
    private GameObject panel;

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

    public void TogglePanelVisibility()
    {
        isPanelVisible = !isPanelVisible;
        panel.SetActive(isPanelVisible);
        toggleText.text = isPanelVisible ? "Hide Summary" : "Show Summary";
        gameObject.GetComponent<FollowCamera>().yPos = isPanelVisible ? 1.2f : 1.8f;
    }
}
