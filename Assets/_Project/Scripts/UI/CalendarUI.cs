using UnityEngine;
using TMPro;

public class CalendarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text timeText;

    private TimeManager timeManager;

    private void Start()
    {
        timeManager = TimeManager.Instance;

        if (timeManager == null)
        {
            Debug.LogWarning("TimeManager not found!");
            return;
        }

        timeManager.OnTimeChanged += UpdateCalendar;

        UpdateCalendar();
    }

    private void UpdateCalendar()
    {
        int hour = timeManager.CurrentHour;

        int displayHour = hour % 12;

        if (displayHour == 0)
            displayHour = 12;

        string period = hour >= 12 ? "PM" : "AM";

        dayText.text = $"{timeManager.CurrentWeekday} - Day {timeManager.CurrentDay}";

        timeText.text = $"{displayHour}:00 {period}";
    }

    private void OnDestroy()
    {
        if (timeManager != null)
            timeManager.OnTimeChanged -= UpdateCalendar;
    }
}
