
using UnityEngine;
using UnityEngine.InputSystem;

public class ScheduleToggle : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject schedulePanel;

    private void Start()
    {
        if (schedulePanel != null)
        {
            schedulePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Keyboard.current == null || schedulePanel == null)
            return;

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleSchedule();
        }
    }

    private void ToggleSchedule()
    {
        bool isOpen = schedulePanel.activeSelf;

        schedulePanel.SetActive(!isOpen);
    }
}
