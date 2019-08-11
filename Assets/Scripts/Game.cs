using UnityEngine;
using System.Collections.Generic;
using System;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public GameObject[] blockPrefabs;

    public CameraS cameraScript;
    public Player player;
    public Element playerElement;

    public GridClass CombinedGrid;

    public bool isDesigner;
    public bool isTestMode;

    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    private List<Block> blocks = new List<Block>();

    private Designer designer;

    private int count = 0;

    public delegate void StartEvent();
    public static event StartEvent CreateData;

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

        CombinedGrid = new GridClass(5, 16, 1, "GameGrid");

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

        if(isTestMode)
        {
            TestGameSetup();
        }
        else
        {
            NewGameSetup();
        }
    }

    private void NewGameSetup()
    {
        SpawnFirstLevel();
        //SpawnSecondLevel();
    }

    private void SpawnFirstLevel()
    {
        //player.enabled = true;
        if (!isDesigner) {
            CreateData();
            SpawnLevel(0f, LevelManager.Instance.levels[1]);
            PreparePlayerStart();

        }
        
        count++;        
    }

    private void SpawnSecondLevel()
    {
        if (isDesigner)
        {
            //SpawnLevel(CombinedGrid.halfHeight * count, LevelManager.Instance.levels[designer.currentLevelName]);
        }
        else
        {
            // тут має братись насправді другий рівень
            CreateData();
            SpawnLevel(CombinedGrid.halfHeight * count, LevelManager.Instance.levels[2]);
        }
        
        count++;
    }

    public void SpawnNewLevel(int count)
    {
        CreateData();
        DestroyLowerBlocks();
        CombinedGrid.AddOffsetToOrigin();
        CombinedGrid.ShiftGrid();
        ReassignCells();
        if (isDesigner)
        {
            //SpawnLevel(CombinedGrid.halfHeight * count, LevelManager.Instance.levels[designer.currentLevelName]);
        }
        else
        {
            // тут має братись насправді НАСТУПНИЙ рівень
            SpawnLevel(count * CombinedGrid.halfHeight, LevelManager.Instance.levels[3]);
        }
        count++;        
    }

    private void SpawnLevel(float offset, LevelData levelData)
    {
        foreach (LevelDataItem item in levelData.items)
        {
            float XPos, YPos;
            XPos = CombinedGrid.origin.x + item.xPos * CombinedGrid.step;
            YPos = CombinedGrid.origin.y + item.yPos * CombinedGrid.step;
            Block block = Instantiate(prefabs[item.prefabName], new Vector3(XPos, YPos + offset, 0f), Quaternion.identity).GetComponent<Block>();
            blocks.Add(block);
            foreach (var blk in blocks) {
                foreach (Element element in blk.elements) {
                    element.SetCell();
                }
            }
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
        LevelManager.Instance.LevelDestroyer();
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

    void PreparePlayerStart() 
    {
        Cell cell = CombinedGrid.cells[2, 3];
    
        if (cell.Element != null)
        {
            Debug.Log(cell.Element.myBlock.name + " under player got destroyed");
            cell.Element.myBlock.SelfDestroy();
        }
        else
        {
            Debug.Log("cell has no gameobject");
        }
        playerElement.SetCell();
    }
    private void SpawnPremadeLevel()
    {
        SpawnLevel(0f, LevelManager.Instance.premadeLevels["1"]);
    }

    private void TestGameSetup()
    {
        cameraScript.enabled = false;
        SpawnPremadeLevel();
    }
}
