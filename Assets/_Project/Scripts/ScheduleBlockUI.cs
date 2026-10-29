
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class ScheduleBlockUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text activityText;

    [Header("Block Sizing")]
    [SerializeField] private float pixelsPerHour = 40f;

    private LayoutElement layoutElement;

    private void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
    }

    public void Setup(string timeRange, string activityName, int durationHours)
    {
        timeText.text = timeRange;
        activityText.text = activityName;

        layoutElement.preferredHeight = durationHours * pixelsPerHour;
    }
}
