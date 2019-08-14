using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Designer : MonoBehaviour
{
    private LevelData levelData;
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    public GameObject tileMap;
    public GameObject background;

    public GameObject[] blockPrefabs;
    public string currentLevelName = "[no name]";

    public void PopulatePrefabs()
    {
        foreach (GameObject g in blockPrefabs)
        {
            if (prefabs.ContainsKey(g.name))
                continue;
            prefabs.Add(g.name, g);
        }
    }

    public void ClearLevel()
    {
        Block[] blocks = FindObjectsOfType<Block>();
        for(int i=0;i<blocks.Length;i++)
        {
            DestroyImmediate(blocks[i].gameObject);
        }
    }

    public void LoadLevel()
    {
        if (!File.Exists(Path.Combine(Application.streamingAssetsPath, "Levels", currentLevelName + ".json")))
        {
            Debug.LogError($"Level with name \"{currentLevelName}\" does not exist");
            return;
        }

        ClearLevel();

        levelData = JsonUtility.FromJson<LevelData>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Levels", currentLevelName + ".json")));
        InstantiateLevel();
    }

    public void SaveLevel()
    {
        CreateLevelDataFromScene();
        string dataAsString = JsonUtility.ToJson(levelData);
        File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "Levels", currentLevelName + ".json"), dataAsString);
    }

    private void InstantiateLevel()
    {
        foreach (LevelDataItem item in levelData.items)
        {
            Block block = Instantiate(prefabs[item.prefabName], new Vector3(item.xPos, item.yPos, 0f), Quaternion.identity,tileMap.transform).GetComponent<Block>();
            block.name = item.prefabName;
        }
    }

    private void CreateLevelDataFromScene()
    {
        List<LevelDataItem> levelDataItems = new List<LevelDataItem>();

        Block[] blocks = FindObjectsOfType<Block>();
        foreach (Block b in blocks)
        {
            Vector2 relativePosition = PositionToIndex(b.transform.position);
            levelDataItems.Add(new LevelDataItem(b.gameObject.name, relativePosition.x, relativePosition.y));
        }

        levelData = new LevelData
        {
            items = levelDataItems.ToArray()
        };
    }

    private Vector2 PositionToIndex(Vector2 position)
    {
        return new Vector2(position.x + 2f, position.y + 3.5f);
    }
}
