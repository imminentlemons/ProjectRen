
using System.Collections.Generic;
using UnityEngine;

public class RelationshipManager : MonoBehaviour
{
    public static RelationshipManager Instance { get; private set; }

    [Header("Ren's Relationship Stats")]
    [Range(0, 100)] public int familiarity = 0;
    [Range(0, 100)] public int trust = 0;
    [Range(0, 100)] public int comfort = 0;

    private HashSet<string> memoryFlags = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ModifyRelationship(
        int familiarityChange,
        int trustChange,
        int comfortChange)
    {
        familiarity = Mathf.Clamp(
            familiarity + familiarityChange, 0, 100);

        trust = Mathf.Clamp(
            trust + trustChange, 0, 100);

        comfort = Mathf.Clamp(
            comfort + comfortChange, 0, 100);

        Debug.Log(
            $"Ren Relationship: Familiarity={familiarity}, " +
            $"Trust={trust}, Comfort={comfort}");
    }

    public void SetFlag(string flag)
    {
        if (!string.IsNullOrEmpty(flag))
        {
            memoryFlags.Add(flag);
            Debug.Log($"Ren remembered: {flag}");
        }
    }

    public bool HasFlag(string flag)
    {
        return memoryFlags.Contains(flag);
    }


    public SaveData GetSaveData()
    {
        SaveData data = new SaveData();

        data.familiarity = familiarity;
        data.trust = trust;
        data.comfort = comfort;

        data.memoryFlags = new List<string>(memoryFlags);

        return data;
    }


    public void LoadSaveData(SaveData data)
    {
        if (data == null)
            return;

        familiarity = Mathf.Clamp(data.familiarity, 0, 100);
        trust = Mathf.Clamp(data.trust, 0, 100);
        comfort = Mathf.Clamp(data.comfort, 0, 100);

        memoryFlags = data.memoryFlags != null
            ? new HashSet<string>(data.memoryFlags)
            : new HashSet<string>();

        Debug.Log("Ren's relationship data loaded!");
    }


}
