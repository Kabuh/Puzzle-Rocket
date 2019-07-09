using UnityEngine;
using System.Collections.Generic;
using System;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public GameObject[] blockPrefabs;

    public CameraS cameraScript;
    public Player player;

    public GridClass CombinedGrid { get; set; }

    public bool isDesigner;

    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    private List<Block> blocks = new List<Block>();

    private Designer designer;

    private int count = 0;

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

        CombinedGrid = new GridClass(5, 16, 1);

        PopulatePrefabs();

        designer = FindObjectOfType<Designer>();
        designer?.ClearLevel();
    }

    private void PopulatePrefabs()
    {
        foreach(GameObject g in blockPrefabs)
        {
            prefabs.Add(g.name, g);
        }
    }

    private void Start()
    {
        if (isDesigner)
        {
            designer.ClearLevel();
            player.gameObject.SetActive(true);
            designer.background.SetActive(false);
        }
        NewGameSetup();
    }

    private void NewGameSetup()
    {        
        SpawnFirstLevel();
        SpawnSecondLevel();
    }

    private void SpawnFirstLevel()
    {
        if(!isDesigner)
            SpawnLevel(0f, LevelManager.Instance.levels["1"]);
        count++;        
    }

    private void SpawnSecondLevel()
    {
        if (isDesigner)
        {
            SpawnLevel(CombinedGrid.halfHeight * count, LevelManager.Instance.levels[designer.currentLevelName]);
        }
        else
        {
            // тут має братись насправді другий рівень
            SpawnLevel(CombinedGrid.halfHeight * count, LevelManager.Instance.levels["1"]);
        }
        
        count++;
    }

    public void SpawnNewLevel(int count)
    {
        DestroyLowerBlocks();
        CombinedGrid.AddOffsetToOrigin();
        CombinedGrid.ShiftGrid();
        ReassignCells();
        if (isDesigner)
        {
            SpawnLevel(CombinedGrid.halfHeight * count, LevelManager.Instance.levels[designer.currentLevelName]);
        }
        else
        {
            // тут має братись насправді НАСТУПНИЙ рівень
            SpawnLevel(count * CombinedGrid.halfHeight, LevelManager.Instance.levels["1"]);
        }
        count++;        
    }

    private void SpawnLevel(float offset, LevelData levelData)
    {
        foreach (LevelDataItem item in levelData.items)
        {
            Block block = Instantiate(prefabs[item.prefabName], new Vector3(item.xPos, item.yPos + offset, 0f), Quaternion.identity).GetComponent<Block>();
            blocks.Add(block);
        }
    }

    private void ResetCamera()
    {
        cameraScript.ResetCamera();
    }

    private void ResetPlayer()
    {
        player.ResetPlayer();
    }    

    private void DestroyAllBlocks()
    {
        foreach(var item in blocks)
        {
            Destroy(item.gameObject);
        }
        blocks.Clear();
    }

    private void DestroyLowerBlocks()
    {
        List<Block> blockToDestroy = new List<Block>();

        for (int i = 0; i < blocks.Count; i++)
        {
            if(blocks[i].ShouldBeDestroyed())
            {
                Block block = blocks[i];
                blockToDestroy.Add(block);
            }
        }

        foreach (var item in blockToDestroy)
        {
            blocks.Remove(item);
            Destroy(item.gameObject);
        }
    }

    private void ReassignCells()
    {
        foreach(Block b in blocks)
        {
            b.ResetElementsCells();
        }
        player.ReassignPlayerElement();
    }

    private void ResetGrid()
    {
        CombinedGrid.ResetGrid();        
    }

    public void GameOver()
    {
        count = 0;
        DestroyAllBlocks();
        ResetGrid();
        ResetPlayer();
        NewGameSetup();
        ResetCamera();
    }


}
