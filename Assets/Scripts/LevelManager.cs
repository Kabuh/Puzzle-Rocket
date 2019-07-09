using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string levelName;

    public Dictionary<string, LevelData> levels { get; private set; }

    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        #region Singleton

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        LoadLevels();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //        SaveLevel(levelName);
    //}

    //public void SaveLevel(string levelName)
    //{
    //    List<LevelDataItem> levelDataItems = new List<LevelDataItem>();

    //    Block[] blocks = FindObjectsOfType<Block>();
    //    foreach(Block b in blocks)
    //    {
    //        levelDataItems.Add(new LevelDataItem(b.gameObject.name, b.transform.position.x, b.transform.position.y));
    //    }

    //    LevelData levelData = new LevelData();
    //    levelData.items = levelDataItems.ToArray();

    //    string dataAsString = JsonUtility.ToJson(levelData);

    //    File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "Levels",levelName+".json"), dataAsString);

    //    Debug.Log("saved");
    //}

    public void LoadLevels() // level reader
    {
        levels = new Dictionary<string, LevelData>();

        string[] files = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Levels"));

        if (files == null)
            return;

        foreach (var item in files) //parser
        {
            if (Path.GetExtension(item) != ".json")
                continue;
            LevelData levelData = JsonUtility.FromJson<LevelData>(File.ReadAllText(item));
            string levelName = Path.GetFileNameWithoutExtension(item);
            levels.Add(levelName, levelData);
        }
    }
}
