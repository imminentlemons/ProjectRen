
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;


public class SaveManager : MonoBehaviour
{
    [Header("Loading")]
    [SerializeField] private bool loadOnStart = true;

    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(
            Application.persistentDataPath,
            "save.json"
        );
    }

    private void Start()
    {
        // All Awake methods have run, including RelationshipManager's setup.
        if (loadOnStart && File.Exists(savePath))
        {
            LoadGame();
        }
    }


    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            SaveGame();
        }

        if (Keyboard.current.f9Key.wasPressedThisFrame)
        {
            LoadGame();
        }
    }


    public void SaveGame()
    {
        RelationshipManager relationship =
            RelationshipManager.Instance;

        if (relationship == null)
        {
            Debug.LogError("RelationshipManager not found!");
            return;
        }

        SaveData data = relationship.GetSaveData();

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);

        Debug.Log("Game saved to: " + savePath);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found.");
            return;
        }

        RelationshipManager relationship =
            RelationshipManager.Instance;

        if (relationship == null)
        {
            Debug.LogError("RelationshipManager not found!");
            return;
        }

        try
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (data == null)
            {
                Debug.LogWarning("Save file is empty. Current progress was kept.");
                return;
            }

            relationship.LoadSaveData(data);
        }
        catch (System.Exception exception) when (
            exception is IOException ||
            exception is System.UnauthorizedAccessException ||
            exception is System.ArgumentException)
        {
            Debug.LogWarning("Could not load save. Current progress was kept. " + exception.Message);
        }
    }
}
