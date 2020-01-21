using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public Dictionary<string, LevelData> premadeLevels { get; private set; }

    //randomizer data
    private GridClass CreatorGrid;
    private int[][] AllCellCoordinates;
    List<int[]> CoordinatesData;

    float[] allBlockTypesChanceAggregate;
    float[] MidBlockTypesChanceAggregate;
    float[] ThinBlockTypesChanceAggregate;


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

        //?
        Game.AllDestruction += GameReset;
    }

    public void LoadLevels()
    {
        premadeLevels = new Dictionary<string, LevelData>();

        string[] files = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Levels"));

        if (files == null) {
            return;
        }

        foreach (var item in files)
        {
            if (Path.GetExtension(item) != ".json") {
                continue;
            }
            LevelData levelData = JsonUtility.FromJson<LevelData>(File.ReadAllText(item));

            string levelName = Path.GetFileNameWithoutExtension(item);

            premadeLevels.Add(levelName, levelData);
        }
    }

    List<LevelDataItem> levelDataItems = new List<LevelDataItem>();

    public LevelData CreateNewLevel()
    {
        CreatorGrid = new GridClass(Game.Instance.CombinedGrid.width, Game.Instance.CombinedGrid.height, Game.Instance.CombinedGrid.step, "CreatorGrid");
        

        ClearGrid(CreatorGrid);
        if (Game.Instance.isHub)
        {
            HubLevelCreator();
        }
        else
        {
            CreatorPseudoAwake();
            RandomLevelCreator();
        }

        LevelData levelData = new LevelData();
        levelData.items = levelDataItems.ToArray();

        levelDataItems.Clear();
        return levelData;
    }

    private void CreatorPseudoAwake()
    {
        AllCellCoordinates = new int[CreatorGrid.width * CreatorGrid.height][];

        int index = 0;
        for (int l = 0; l < CreatorGrid.height; l++)
        {
            for (int k = 0; k < CreatorGrid.width; k++)
            {
                AllCellCoordinates[index] = new int[2] { k, l };
                index++;
            }
        }

        CoordinatesData = new List<int[]>(AllCellCoordinates);

        allBlockTypesChanceAggregate = new float[BlockType.All.Count];
        for (int i = 0; i < allBlockTypesChanceAggregate.Length; i++)
        {
            allBlockTypesChanceAggregate[i] = BlockType.All[i].SpawnChance;
        }

        MidBlockTypesChanceAggregate = new float[BlockType.MidWidthOrLess.Count];
        for (int i = 0; i < MidBlockTypesChanceAggregate.Length; i++)
        {
            MidBlockTypesChanceAggregate[i] = BlockType.MidWidthOrLess[i].SpawnChance;
        }

        ThinBlockTypesChanceAggregate = new float[BlockType.LowWidth.Count];
        for (int i = 0; i < ThinBlockTypesChanceAggregate.Length; i++)
        {
            ThinBlockTypesChanceAggregate[i] = BlockType.LowWidth[i].SpawnChance;
        }
    }

    public void HubLevelCreator() {
        levelDataItems.Add(new LevelDataItem("ChoiseBlock", 0, CreatorGrid.halfHeight - 3, "UI"));
        levelDataItems.Add(new LevelDataItem("ChoiseBlock", CreatorGrid.width - 2, CreatorGrid.halfHeight - 3, "UI"));
    }

    void RandomLevelCreator() {
        foreach (int[] item in CoordinatesData)
        {
            GenerateBlock(item);
        }
    }

    private void GenerateBlock(int[] item)
    {
        string blockName = "";
        switch (item[0]) {
            case 0:
            case 1:
            case 2:
                blockName = BlockType.All[SpawnerTools.ComplexRando(allBlockTypesChanceAggregate, 0)].Name;
                break;
            case 3:
                blockName = BlockType.All[SpawnerTools.ComplexRando(MidBlockTypesChanceAggregate, 0)].Name;
                break;
            case 4:
                blockName = BlockType.All[SpawnerTools.ComplexRando(MidBlockTypesChanceAggregate, 0)].Name;
                break;

            default:
                Debug.Log("coordinate data isn't valid : LevelManager");
                break;
        }

        levelDataItems.Add(new LevelDataItem(blockName, item[0], item[1], "Block"));
    }

    void ClearGrid(GridClass grid) {
        grid.ResetGrid();
    }

    void GameReset() {

    }
}
