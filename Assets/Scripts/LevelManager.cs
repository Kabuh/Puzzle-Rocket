using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


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

    int[] SkipOffset = new int[] { 0, 0 };


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

        allBlockTypesChanceAggregate = new float[DatabaseProvider.Asset.All.Count];
        for (int i = 0; i < allBlockTypesChanceAggregate.Length; i++)
        {
            allBlockTypesChanceAggregate[i] = DatabaseProvider.Asset.All[i].SpawnChance;
        }

        MidBlockTypesChanceAggregate = new float[DatabaseProvider.Asset.MidWidthOrLess.Count];
        for (int i = 0; i < MidBlockTypesChanceAggregate.Length; i++)
        {
            MidBlockTypesChanceAggregate[i] = DatabaseProvider.Asset.MidWidthOrLess[i].SpawnChance;
        }

        ThinBlockTypesChanceAggregate = new float[DatabaseProvider.Asset.LowWidth.Count];
        for (int i = 0; i < ThinBlockTypesChanceAggregate.Length; i++)
        {
            ThinBlockTypesChanceAggregate[i] = DatabaseProvider.Asset.LowWidth[i].SpawnChance;
        }
    }

    public void HubLevelCreator() {
        levelDataItems.Add(new LevelDataItem("ChoiseBlock", 0, CreatorGrid.halfHeight - 3, "UI"));
        levelDataItems.Add(new LevelDataItem("ChoiseBlock", CreatorGrid.width - 2, CreatorGrid.halfHeight - 3, "UI"));
    }

    void RandomLevelCreator() {
        for (int i = 0; i < CoordinatesData.Count; i++) {
            GenerateBlock(CoordinatesData[i]);
        }
    }

    string blockName = "";
    BlockType chosenBlock = null;
    int projectedSpace;

    private void GenerateBlock(int[] item)
    {
        
        projectedSpace = item[0];
        if (SpawnerTools.BinaryRandom(DatabaseProvider.Asset.spawnNothing) == false) {
            BlockCasing(projectedSpace, item);



        }
        
    }

    void BlockCasing(int index, int[] item) {
        switch (index)
        {
            case 0:
            case 1:
            case 2:
                chosenBlock = DatabaseProvider.Asset.All[SpawnerTools.ComplexRando(allBlockTypesChanceAggregate, 0)];
                break;
            case 3:
                chosenBlock = DatabaseProvider.Asset.MidWidthOrLess[SpawnerTools.ComplexRando(MidBlockTypesChanceAggregate, 0)];
                break;
            case 4:
                chosenBlock = DatabaseProvider.Asset.LowWidth[SpawnerTools.ComplexRando(ThinBlockTypesChanceAggregate, 0)];
                break;

            default:
                Debug.LogError("coordinate data isn't valid : LevelManager");
                break;
        }

        blockName = chosenBlock.BlockObject.name;

        List<int> CoordinatesToDestroy = new List<int>();

        for (int y = 1; y <= chosenBlock.Height; y++)
        {
            for (int x = 1; x <= chosenBlock.Width; x++)
            {
                if (x + y > 2)
                {
                    int[] Offset = new int[] { x + item[0] - 1, y + item[1] - 1 };
                    int listIndex;
                    listIndex = CoordinatesData.FindIndex(e => e.SequenceEqual(Offset));
                    if (listIndex >= 0)
                    {
                        Debug.Log(CoordinatesData[listIndex][0] + "," + CoordinatesData[listIndex][1] + " : index removed");
                        CoordinatesToDestroy.Add(listIndex);
                    }
                    else
                    {
                        CoordinatesToDestroy.Clear();
                        
                        index++;
                        if (index < 5) {
                            BlockCasing(index, item);
                        }     
                    }
                }
            }
        }

        for (int adress = CoordinatesToDestroy.Count-1; adress >= 0; adress--) {
            CoordinatesData.RemoveAt(CoordinatesToDestroy[adress]);
            CoordinatesData.TrimExcess();
        }
        

        levelDataItems.Add(new LevelDataItem(blockName, item[0], item[1], "Block"));
    }

    void ClearGrid(GridClass grid) {
        grid.ResetGrid();
    }

    void GameReset() {
        
    }
}
