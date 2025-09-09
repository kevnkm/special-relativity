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

    [SerializeField]
    private Transform inactivePosition;

    [SerializeField]
    private Button panel1Button;

    [SerializeField]
    private Button panel2Button;

    private void Start()
    {
        toggleButton.onClick.AddListener(() => OnToggleButtonClicked?.Invoke());
        toggleButton.onClick.AddListener(TogglePanelVisibility);

        panel1Button.gameObject.SetActive(false);
        panel2Button.gameObject.SetActive(false);

        panel1.SetActive(true);
        panel2.SetActive(false);
    }

    private void Update()
    {
        if (isPanelVisible)
            return;

        // If the panel is not visible, move it to the inactive position smoothly
        transform.position = Vector3.Lerp(
            transform.position,
            inactivePosition.position,
            Time.deltaTime * 2f
        );
    }

    private bool isPanelVisible = true;

    public void TogglePanelVisibility()
    {
        isPanelVisible = !isPanelVisible;
        if (isPanelVisible)
        {
            if (currentPanel == 1)
            {
                panel1.SetActive(true);
                panel2.SetActive(false);
            }
            else if (currentPanel == 2)
            {
                panel1.SetActive(false);
                panel2.SetActive(true);
            }
        }
        else
        {
            panel1.SetActive(false);
            panel2.SetActive(false);
        }

        toggleText.text = isPanelVisible ? "Hide Summary" : "Show Summary";

        // Enable FollowCamera only if a panel is visible
        gameObject.GetComponent<FollowCamera>().enabled = isPanelVisible;
    }

    private int currentPanel = 1;

    public void ShowPanel2()
    {
        panel1.SetActive(false);
        panel2.SetActive(true);
        toggleText.text = "Hide Summary";
    }

    public void ShowButtons()
    {
        panel1Button.gameObject.SetActive(true);
        panel2Button.gameObject.SetActive(true);

        panel1Button.onClick.AddListener(() => SwitchPanel(2));
        panel2Button.onClick.AddListener(() => SwitchPanel(1));
    }

    private void SwitchPanel(int panelNumber)
    {
        currentPanel = panelNumber;
        if (panelNumber == 1)
        {
            panel1.SetActive(true);
            panel2.SetActive(false);
        }
        else if (panelNumber == 2)
        {
            panel1.SetActive(false);
            panel2.SetActive(true);
        }
    }
}
