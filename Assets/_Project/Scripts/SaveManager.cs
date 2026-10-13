
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;


public class SaveManager : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(
            Application.persistentDataPath,
            "save.json"
        );
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

        string json = File.ReadAllText(savePath);

        SaveData data =
            JsonUtility.FromJson<SaveData>(json);

        RelationshipManager relationship =
            RelationshipManager.Instance;

        if (relationship != null)
        {
            relationship.LoadSaveData(data);
        }
    }
}
