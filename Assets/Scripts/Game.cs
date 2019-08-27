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
    private List<BoosterObject> boosters = new List<BoosterObject>();

    private Designer designer;

    public int count = 1;

    public delegate void GameScriptEvents();
    public static event GameScriptEvents LevelSpawnFinished;
    public static event GameScriptEvents PlayerDead;

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
        CameraS.CreateLevel += SpawnNewLevel;

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
        SpawnSecondLevel();
    }

    private void SpawnFirstLevel()
    {
        if (!isDesigner) {
            SpawnLevel(0f, LevelManager.Instance.CreateNewLevel());
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
            SpawnLevel(CombinedGrid.halfHeight, LevelManager.Instance.CreateNewLevel());
        }
        
        count++;
    }

    public void SpawnNewLevel()
    {
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
            try
            {
                SpawnLevel(CombinedGrid.halfHeight, LevelManager.Instance.CreateNewLevel());
            }
            finally {
                Debug.Log("Level spawn with key "+ count);
            }
        }
        LevelSpawnFinished();
        count++;        
    }

    private void SpawnLevel(float offset, LevelData levelData)
    {
        foreach (LevelDataItem item in levelData.items)
        {
            float XPos, YPos;
            XPos = CombinedGrid.origin.x + item.xPos * CombinedGrid.step;
            YPos = CombinedGrid.origin.y + item.yPos * CombinedGrid.step;

            if (item.type == "block")
            {
                Block block = Instantiate(prefabs[item.prefabName], new Vector3(XPos, YPos + offset, 0f), Quaternion.identity).GetComponent<Block>();
                blocks.Add(block);
                foreach (Element element in block.elements)
                {
                    element.SetCell();
                }
            }
            else
            {
                BoosterObject booster = Instantiate(prefabs[item.prefabName], new Vector3(XPos, YPos + offset, 0f), Quaternion.identity).GetComponent<BoosterObject>();
                boosters.Add(booster);
                booster.SetCell();
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
            item?.SelfDestroy();
        }
        foreach (var item in boosters)
        {
            item?.SelfDestroy();
        }
        blocks.Clear();
        boosters.Clear();
    }

    private void DestroyLowerBlocks()
    {
        List<Block> blockToDestroy = new List<Block>();

        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] != null)
            {
                if (blocks[i].ShouldBeDestroyed())
                {
                    Block block = blocks[i];
                    blockToDestroy.Add(block);
                }
            }
        }

        foreach (var item in blockToDestroy)
        {
            blocks.Remove(item);
            if (item != null) {
                item.SelfDestroy();
            }
        }
    }

    private void ReassignCells()
    {
        foreach(Block b in blocks)
        {
            if(b!=null)
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
        PlayerDead();
        count = 1;
        DestroyAllBlocks();
        ResetGrid();
        ResetPlayer();
        NewGameSetup();
        ResetCamera();
    }

    void PreparePlayerStart() {
        Cell cell = CombinedGrid.WorldPosToCell(player.transform.position);
        

        if (cell.Element != null)
        {
            Block ObstructingBlock = cell.Element.myBlock;
            
            if (ObstructingBlock != player.playerBlock) {
                ObstructingBlock.SelfDestroy();
                Debug.Log(ObstructingBlock.name + " under player got destroyed");
                blocks.Remove(ObstructingBlock);
            }
            
        }
        playerElement.SetCell();
    }
    private void SpawnPremadeLevel()
    {
        SpawnLevel(0f, LevelManager.Instance.premadeLevels["1premade"]);
    }

    private void TestGameSetup()
    {
        cameraScript.enabled = false;
        player.GetComponent<Block>().elements[0].SetCell();
        SpawnPremadeLevel();
    }
}
