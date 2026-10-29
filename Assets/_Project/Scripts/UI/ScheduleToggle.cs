
using UnityEngine;
using UnityEngine.InputSystem;

public class ScheduleToggle : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject schedulePanel;


    [Header("Gameplay Controls")]
    [SerializeField] private MonoBehaviour[] controlsToPause;

    private bool[] previousStates;
    private CursorLockMode previousCursorLock;
    private bool previousCursorVisible;


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
        bool opening = !schedulePanel.activeSelf;

        if (opening)
        {
            previousStates = new bool[controlsToPause.Length];

            for (int i = 0; i < controlsToPause.Length; i++)
            {
                if (controlsToPause[i] == null)
                    continue;

                previousStates[i] = controlsToPause[i].enabled;
                controlsToPause[i].enabled = false;
            }

            previousCursorLock = Cursor.lockState;
            previousCursorVisible = Cursor.visible;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            for (int i = 0; i < controlsToPause.Length; i++)
            {
                if (controlsToPause[i] != null)
                    controlsToPause[i].enabled = previousStates[i];
            }

            Cursor.lockState = previousCursorLock;
            Cursor.visible = previousCursorVisible;
        }

        schedulePanel.SetActive(opening);
    }

}
