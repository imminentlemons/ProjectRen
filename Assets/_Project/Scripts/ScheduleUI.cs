using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScheduleUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform contentRoot;
    [SerializeField] private ScheduleBlockUI scheduleBlockPrefab;

    [Header("Day Bounds")]
    [SerializeField] private int dayStartHour = 8;
    [SerializeField] private int dayEndHour = 23;

    private TimeManager timeManager;

    private void Start()
    {
        timeManager = TimeManager.Instance;

        if (timeManager == null || contentRoot == null || scheduleBlockPrefab == null)
        {
            Debug.LogWarning("Schedule UI setup incomplete!");
            return;
        }

        timeManager.OnTimeChanged += RefreshSchedule;
        RefreshSchedule();
    }

    private void RefreshSchedule()
    {
        ClearBlocks();

        var today = timeManager.GetTodaySchedule();

        int currentStart = dayStartHour;

        if (today.Count == 0)
        {
            CreateBlock(dayStartHour, dayEndHour, "Free Time");
            return;
        }

        foreach (ScheduledBlock block in today)
        {
            if (block.startHour > currentStart)
            {
                CreateBlock(currentStart, block.startHour, "Free Time");
            }

            CreateBlock(block.startHour, block.endHour, block.activityName);
            currentStart = block.endHour;
        }

        if (currentStart < dayEndHour)
        {
            CreateBlock(currentStart, dayEndHour, "Free Time");
        }
    }

    private void CreateBlock(int startHour, int endHour, string activityName)
    {
        ScheduleBlockUI blockUI = Instantiate(scheduleBlockPrefab, contentRoot);

        string timeRange = $"{FormatHour(startHour)} - {FormatHour(endHour)}";

        int duration = endHour - startHour;

        blockUI.Setup(timeRange, activityName, duration);
    }


    private void ClearBlocks()
    {
        for (int i = contentRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(contentRoot.GetChild(i).gameObject);
        }
    }

    private string FormatHour(int hour)
    {
        int displayHour = hour % 12;

        if (displayHour == 0)
            displayHour = 12;

        string period = hour >= 12 ? "PM" : "AM";

        return $"{displayHour}:00 {period}";
    }

    private void OnDestroy()
    {
        if (timeManager != null)
            timeManager.OnTimeChanged -= RefreshSchedule;
    }
}