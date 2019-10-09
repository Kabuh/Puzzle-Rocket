using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class LevelManager : MonoBehaviour
{
    [SerializeField]

    public Dictionary<string, LevelData> premadeLevels { get; private set; }

    public static LevelManager Instance { get; private set; }

    public int[] sizeChancesIftwo = { 45, 15, 10, 15, 5, 10 };
    public int[] sizeChancesIfOne = { 45, 20, 10, 15, 10 };
    public int[] sizeChancesIfNone = { 65, 25, 10 };
    public int[] typeChances = { 30, 10, 60 };

    public float boostersSpawnChance;
    public int[] boostersTypesSpawnChance;
    //public float boosterSpawnChance;
    GridClass CreatorGrid;





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

    public LevelData CreateNewLevel() {
        CreatorGrid = new GridClass(Game.Instance.CombinedGrid.width, Game.Instance.CombinedGrid.halfHeight, Game.Instance.CombinedGrid.step, "CreatorGrid");

        ClearGrid(CreatorGrid);
        RandomLevelCreator();

        LevelData levelData = new LevelData();
        levelData.items = levelDataItems.ToArray();

        levelDataItems.Clear();

        return levelData;
    }


    /// ////////////////////////////////////////

    //main logic body
    public void RandomLevelCreator() {
        int x, y;
        for (y = 0; y < CreatorGrid.height; y++) {
            for (x = 0; x < CreatorGrid.width; x++) {

                int[] Offset = new int[2];

                if (CreatorGrid.cells[x, y].IsEmpty)
                {
                    if (x <= 3 && CreatorGrid.cells[x + 1, y].IsEmpty)
                    {
                        if (x <= 2 && CreatorGrid.cells[x + 2, y].IsEmpty)
                        {

                            CasingType(x, y, ComplexRando(sizeChancesIftwo), ComplexRando(typeChances), out Offset);
                        }
                        else
                        {
                            CasingType(x, y, ComplexRando(sizeChancesIfOne), ComplexRando(typeChances), out Offset);
                        }
                    }
                    else
                    {
                        CasingType(x, y, ComplexRando(sizeChancesIfNone), ComplexRando(typeChances), out Offset);
                    }

                    OffsetMaker(x, y, Offset);
                }

                if ((Game.Instance.count == 1 && x == 2 && y == 3) == true)
                {
                    continue;
                }

                if (SimpleRando(boostersSpawnChance)) {
                    RunBoosterChances(x, y);
                }
            }
        }
    }
    

    void RunBoosterChances(int x, int y) {
            switch (ComplexRando(boostersTypesSpawnChance))
            {
                case 1:
                    levelDataItems.Add(new LevelDataItem("shot_booster", x, y, "booster"));
                    break;
                case 2:
                    levelDataItems.Add(new LevelDataItem("laser_V_booster", x, y, "booster"));
                    break;
                case 3:
                    levelDataItems.Add(new LevelDataItem("laser_H_booster", x, y, "booster"));
                    break;
                case 4:
                    levelDataItems.Add(new LevelDataItem("bomb_booster", x, y, "booster"));
                    break;
                case 5:
                    levelDataItems.Add(new LevelDataItem("bigbomb_booster", x, y, "booster"));
                    break;
                case 6:
                    levelDataItems.Add(new LevelDataItem("teleport_booster", x, y, "booster"));
                    break;
                case 7:
                    levelDataItems.Add(new LevelDataItem("time_stop_booster", x, y, "booster"));
                    break;
                case 8:
                    levelDataItems.Add(new LevelDataItem("coin", x, y, "booster"));
                    break;
                default:
                    break;
            }
    }

    //create free space for big blocks
    private void OffsetMaker(int x, int y, int[] Offset)
    {
        if (Offset != null)
        {
            for (int k = 0; k <= Offset[1]; k++)
            {
                for (int l = 0; l <= Offset[0]; l++)
                {
                    if (k > 0 || l > 0)
                    {
                        int newX = x + l;
                        int newY = y + k;
                        if (newY >= CreatorGrid.height)
                        {

                        }
                        else
                        {
                            CreatorGrid.cells[x + l, y + k].IsEmpty = false;
                        }
                    }
                }
            }
        }
    }

    //creation of easy to use block indexes
    Dictionary<int, string> FigureType = new Dictionary<int, string>
        {
            { 110, "Single" },
            { 120, "Di Ver" },
            { 130, "Tri Ver" },
            { 210, "Di Hor" },
            { 220, "Cube" },
            { 310, "Tri Hor" },
            
            { 111, "Immovable" },
            { 121, "IM_Di Ver" },
            { 131, "IM_Tri Ver" },
            { 211, "IM_Di Hor" },
            { 221, "IM_Cube" },
            { 311, "IM_Tri Hor" }
        };

    //choosing a block figure
    void GenerateFigure(int type, bool isImmovable, int currentIndexX, int currentIndexY) {
        if (isImmovable) {
            type++;
        }

        GenerateBlock(FigureType[type], currentIndexX, currentIndexY);
    }

    //adding block type to level data
    void GenerateBlock(string name, int x, int y) {
        levelDataItems.Add(new LevelDataItem(name, x, y, "block"));
    }

    //no big blocks should protrude trough the grid
    int CeilingSwitch(int freeLines, int casing) {
        if (freeLines == 1) {
            if (casing == 3) {
                return 2;
            }
        }
        if (freeLines == 0) {
            if (casing == 3 || casing == 2)
            {
                return 1;
            }
            else if (casing == 5) {
                return 4;
            }
        }
        return casing;
    }

    //interpreting chance calculation into block type
    void CasingType(int x, int y, int Size, int Type, out int[] CellOffsetI) {
        int[] OffsetArray = new int[2];
        switch (Type) {
            case 1:
                CasingFigure(x, y ,CeilingSwitch(CreatorGrid.height - y - 1 , Size), false, out OffsetArray);
                break;
            case 2:
                CasingFigure(x, y, CeilingSwitch(CreatorGrid.height - y - 1, Size), true, out OffsetArray);
                break;
            case 3:
                break;
            default:
                Debug.Log("Unknown Rando case");
                OffsetArray = new int[] { 0, 0 };
                break;
        }
        CellOffsetI = OffsetArray;
    }

    //interpreting chance calculation into block shape
    void CasingFigure(int x, int y, int RandoResult, bool isImmovable ,out int[] CellOffset) {
        int typeIndex;

        switch (RandoResult) {
            case 1:
                typeIndex = 110;
                CellOffset = new int[] { 0, 0 };
                break;
            case 2:
                typeIndex = 120;
                CellOffset = new int[] { 0, 1 };
                break;
            case 3:
                typeIndex = 130;
                CellOffset = new int[] { 0, 2 };
                break;
            case 4:
                typeIndex = 210;
                CellOffset = new int[] { 1, 0 };
                break;
            case 5:
                typeIndex = 220;
                CellOffset = new int[] { 1, 1 };
                break;
            case 6:
                typeIndex = 310;
                CellOffset = new int[] { 2, 0 };
                break;
            default:
                typeIndex = 110;
                Debug.Log("Unknown Rando case");
                CellOffset = new int[] { 0, 0 };
                break;
        }

        GenerateFigure(typeIndex, isImmovable, x, y);
    }

    //creating a random answer from binary pool
    bool SimpleRando(float chance)
    {
        float x = Random.Range(0, 100);
        if (x <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //creating a random answer from multi-answer pool
    int ComplexRando(int[] valueLib) {
        float x = Random.Range(0, 100);
        int sum = 0;
        for (int i = 0; i < valueLib.Length; i++) {
            sum += valueLib[i];
            if (sum > 100 || sum < 0) {
                return -1;
            }
            if (x < sum)
            {
                return ++i;
            }
        }
        return -1;
    }

    void ClearGrid(GridClass grid) {
        grid.ResetGrid();
    }
}
