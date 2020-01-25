using UnityEngine;
using System.Collections.Generic;
using System;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public GameObject[] blockPrefabs;

    public CameraS cameraScript;
    public Player Player { get; set; }
    public Vector2 playerSpawnCellCoordinates;

    public GridClass CombinedGrid;

    public bool isDesigner;
    public bool isTestMode;
    public bool isHub;

    public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();    

    private Designer designer;

    public int count = 1;

    public delegate void GameScriptEvents();
    public static event GameScriptEvents LevelSpawnFinished;
    public static event GameScriptEvents PlayerDead;

    public static event GameScriptEvents AllDestruction;
    public static event GameScriptEvents LowerDestruction;
    public static event GameScriptEvents CellLevelShift;

    public static event GameScriptEvents ExplosionEnded;

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

        if (isHub)
        {
            CombinedGrid = new GridClass(5, 16, 1, "GameGrid");
        }
        else {
            CombinedGrid = new GridClass(5, 30, 1, "GameGrid");
        }
        

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
            Player.gameObject.SetActive(true);
            designer.background.SetActive(false);

            
        }
        CameraS.CreateLevel += SpawnNewLevel;

        if (isTestMode)
        {
            TestGameSetup();
        }
        else if (isHub) {
            HubSetup();
        }
        else
        {
            NewGameSetup();
        }
    }

    private void NewGameSetup()
    {
        SpawnPlayer();
        SpawnFirstLevel();
        //SpawnSecondLevel();
    }

    private void SpawnPlayer()
    {
        float xPos = CombinedGrid.origin.x + playerSpawnCellCoordinates.x * CombinedGrid.step;
        float yPos = CombinedGrid.origin.y + playerSpawnCellCoordinates.y * CombinedGrid.step;

        Player p = Instantiate(prefabs["Player"], new Vector3(xPos,yPos), Quaternion.identity).GetComponent<Player>();
        Player = p;
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
        LowerDestruction();
        CombinedGrid.AddOffsetToOrigin();
        CombinedGrid.ShiftGrid();
        CellLevelShift();
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

            ISpawnable spawnedObject = Instantiate
                (
                    prefabs[item.prefabName], 
                    new Vector3(XPos, YPos + offset, 0f), 
                    Quaternion.identity
                ).GetComponent<ISpawnable>();
        }
    }

    private void ResetCamera()
    {
        cameraScript.ResetCamera();
    }      

    private void ResetGrid()
    {
        CombinedGrid.ResetGrid();        
    }

    public void GameOver()
    {
        PlayerDead();
        count = 1;
        AllDestruction();
        ResetGrid();
        NewGameSetup();
        ResetCamera();
    }

    private void PreparePlayerStart()
    {

        Cell cell = CombinedGrid.WorldPosToCell(Player.transform.position);        

        if (cell.Element != null)
        {
            Block obstructingBlock = cell.Element.myBlock;
            
            if (obstructingBlock != Player.playerBlock)
            {                
                obstructingBlock.SelfDestroy();
            }
            
        }
        Player.playerBlock.elements[0].SetCell();
    }

    private void SpawnPremadeLevel()
    {
        SpawnLevel(0f, LevelManager.Instance.premadeLevels["1premade"]);
    }

    private void HubSetup()
    {
        cameraScript.enabled = false;
        //Player.GetComponent<Block>().elements[0].SetCell();
        SpawnLevel(0f, LevelManager.Instance.premadeLevels["Hub"]);
        SpawnLevel(0f, LevelManager.Instance.CreateNewLevel());
        SpawnPlayer();
        PreparePlayerStart();
    }

    private void TestGameSetup()
    {
        cameraScript.enabled = false;
        Player.GetComponent<Block>().elements[0].SetCell();
        SpawnPremadeLevel();
    }

    public void ExplosionEnd()
    {
        ExplosionEnded();
    }
}
