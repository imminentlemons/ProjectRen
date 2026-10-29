using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Calendar")]
    [SerializeField] private int currentDay = 1;
    [SerializeField] private int currentHour = 8;
    [SerializeField] private int endOfDay = 23;

    [Header("Schedule")]
    [SerializeField] private List<ScheduledBlock> scheduledBlocks = new();

    private int activeActivityDuration = 0;

    public int CurrentDay => currentDay;
    public int CurrentHour => currentHour;
    public GameWeekday CurrentWeekday => 
        (GameWeekday)((Mathf.Max(currentDay, 1) - 1) % 7);

    public bool IsInActivity => activeActivityDuration > 0;

    public event Action OnTimeChanged;

    private void Awake()
    {
        if ( Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool CanStartActivity(int duration)
    {
        if(IsInActivity || duration <= 0)
            return false;

        if (CurrentHour + duration > endOfDay)
            return false;

        if (!IsTimeSlotAvailable(currentHour, duration))
            return false;


        return true;
    }

    public bool IsTimeSlotAvailable(int startHour, int duration)
    {
        int endHour = startHour + duration;

        foreach (ScheduledBlock block in scheduledBlocks)
        {
            if (block.weekday != CurrentWeekday)
                continue;
            if (startHour < block.endHour &&
                endHour > block.startHour)
            {
                return false;
            }
        }

        return true;
    }

    public bool StartActivity(int duration)
    {
        if(!CanStartActivity(duration))
            return false;

        activeActivityDuration = duration;

        Debug.Log("Activity started");

        return true;
    }

    public bool StartScheduledActivity(string activityName)
    {
        if (IsInActivity)
            return false;

        foreach (ScheduledBlock block in scheduledBlocks)
        {
            if (block.weekday != CurrentWeekday || 
                block.activityName != activityName)
                continue;

            if (currentHour != block.startHour)
            {
                Debug.Log("It's not time for this activity!");
                return false;
            }

            int duration = block.endHour - block.startHour;

            if (duration <= 0 || block.endHour > endOfDay)
                return false;

            activeActivityDuration = duration;

            Debug.Log($"{activityName} started!");

            return true;
        }

        Debug.LogWarning($"Activity not found: {activityName}");
        return false;
    }

    public void FinishActivity()
    {
        if (!IsInActivity)
            return;

        currentHour += activeActivityDuration;
        activeActivityDuration = 0;

        Debug.Log($"Current time: {currentHour}:00");

        OnTimeChanged?.Invoke();
    }

    public void Sleep()
    {
        if (IsInActivity)
            return;

        currentDay++;
        currentHour = 8;

        Debug.Log($"Day {currentDay} ({CurrentWeekday}) - 8:00 AM");

        OnTimeChanged?.Invoke();
    }

    public bool WaitOneHour()
    {
        if (IsInActivity)
            return false;

        if (currentHour + 1 > endOfDay)
            return false;

        if (!IsTimeSlotAvailable(currentHour, 1))
        {
            Debug.Log("You have a scheduled activity right now!");
            return false;
        }

        currentHour++;

        Debug.Log($"Current time: {currentHour}:00");

        OnTimeChanged?.Invoke();

        return true;

    }


    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            StartActivity(2);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            FinishActivity();
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Sleep();
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            StartScheduledActivity("Work");
        }

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            WaitOneHour();
        }
    }

}

[System.Serializable]
public class ScheduledBlock
{
    public string activityName;
    public GameWeekday weekday;
    public int startHour;
    public int endHour;
}

public enum GameWeekday
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}
