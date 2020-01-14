using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class LevelManager : MonoBehaviour
{
    public Dictionary<string, LevelData> premadeLevels { get; private set; }

    public static LevelManager Instance { get; private set; }

    Dictionary<string, int> BlockIndex = new Dictionary<string, int>
        {
            {"Single"   , 1},            
            {"Di Ver"   , 2},
            {"Tri Ver"  , 3},
            {"Di Hor"   , 4},
            {"Cube"     , 5},
            {"Tri Hor"  , 6},
            {"SingleV"  , 7},
            {"CubeV"    , 8},

            {"Immovable"    , 9 },
            {"IM_Di Ver"    , 10},
            {"IM_Tri Ver"   , 11},
            {"IM_Di Hor"    , 12},
            {"IM_Cube"      , 13},
            {"IM_Tri Hor"   , 14},
            {"ImmovableV"   , 15},
            {"IM_CubeV"     , 16}
        };

    int GetBlockIndex(string myString) {
        int bank = BlockIndex[myString] - 1;
        if (bank < 8) {
            return bank;
        }
        return bank - 8;
    }

    [Header("Cube Chances")]
    [SerializeField] private float[] defChancesToSpawnDifferentType = { 45, 15, 10, 15, 5, 10, 45, 5};
    [SerializeField] private float[] actualChancesToSpawnDifferentType = { 0, 0, 0, 0, 0, 0, 0, 0 };
    [SerializeField] private float defChanceToSpawnAnything = 60.0f;
    [SerializeField] private float defChanceToMakeCubeImmovable = 25.0f;

    [Header("Booster Chances")]
    [SerializeField] private float defBoostersSpawnChance = 20.0f;
    [SerializeField] private float[] defBoostersTypesSpawnChance = { 15, 5, 10, 5, 5, 5, 5, 50 };

    
    private float actualChanceToSpawnAnything = -1;
    private float actualChanceToMakeCubeImmovable = -1;
    private float actualBoostersSpawnChance = -1;
    private float[] actualBoostersTypesSpawnChance = null;

    private float[] spawnChanceStrategy = new float[] { 50, 20, 50, 50, 80, 80, 80, 80 };
    private float[] spawnChanceStrategyBoosters = new float[] { 30, 50, 30, 10, 30, 50, 30, 10 };

    private int[] groupToBoost = new int[] {0, 2, 1, 1, 1, 2, 2, 2};

    //[SerializeField] private int lineCounter = 0;
    //[SerializeField] private int strategyStep = 100;

    private float currentGoalSpawnChance;
    private float currentGoalSpawnChanceBoosters;

    #region Properties
    private float[] ChancesToSpawnDifferentType {
        get {
            //if (actualChancesToSpawnDifferentType == null) {
            //    return defChancesToSpawnDifferentType;
            //}
            return actualChancesToSpawnDifferentType;
        }
        set
        {
            actualChancesToSpawnDifferentType = value;
        }
    }

    private float ChanceToSpawnAnything
    {
        get
        {
            if (actualChanceToSpawnAnything == -1)
            {
                return defChanceToSpawnAnything;
            }
            return actualChanceToSpawnAnything;
        }
        set
        {
            actualChanceToSpawnAnything = value;
        }
    }

    private float ChanceToMakeCubeImmovable
    {
        get
        {
            if (actualChanceToMakeCubeImmovable == -1)
            {
                return defChanceToMakeCubeImmovable;
            }
            return actualChanceToMakeCubeImmovable;
        }
        set
        {
            actualChanceToMakeCubeImmovable = value;
        }
    }
    private float BoostersSpawnChance
    {
        get
        {
            if (actualBoostersSpawnChance == -1)
            {
                return defBoostersSpawnChance;
            }
            return actualBoostersSpawnChance;
        }
        set
        {
            actualBoostersSpawnChance = value;
        }
    }
    private float[] BoostersTypesSpawnChance
    {
        get
        {
            if (actualBoostersTypesSpawnChance == null)
            {
                return defBoostersTypesSpawnChance;
            }
            return actualBoostersTypesSpawnChance;
        }
        set
        {
            actualBoostersTypesSpawnChance = value;
        }
    }
    #endregion

    private GridClass CreatorGrid;

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

        Game.AllDestruction += GameReset;
        defChanceToSpawnAnything = spawnChanceStrategy[0];
        defBoostersSpawnChance = spawnChanceStrategyBoosters[0];
        currentGoalSpawnChance = spawnChanceStrategy[1];
        currentGoalSpawnChanceBoosters = spawnChanceStrategyBoosters[1];

        //CopyArray(defChancesToSpawnDifferentType, actualChancesToSpawnDifferentType);

    }

    void CopyArray(float[] arrayDonor, float[] arrayRecepient) {
        for (int i = 0; i < arrayDonor.Length; i++) {
            arrayRecepient[i] = arrayDonor[i];
        }
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
        CreatorGrid = new GridClass(Game.Instance.CombinedGrid.width, Game.Instance.CombinedGrid.height, Game.Instance.CombinedGrid.step, "CreatorGrid");

        ClearGrid(CreatorGrid);
        if (Game.Instance.isHub)
        {
            HubLevelCreator();
        }
        else {
            RandomLevelCreator();
        }

        LevelData levelData = new LevelData();
        levelData.items = levelDataItems.ToArray();

        levelDataItems.Clear();
        return levelData;
    }

    public void HubLevelCreator() {
        levelDataItems.Add(new LevelDataItem("ChoiseBlock", 0, CreatorGrid.halfHeight - 3, "UI"));
        levelDataItems.Add(new LevelDataItem("ChoiseBlock", CreatorGrid.width - 2, CreatorGrid.halfHeight - 3, "UI"));
    }


    /// ////////////////////////////////////////

    //main logic body
    public void RandomLevelCreator() {
        int IndexFromSaveNode = GetBlockIndex(SaveNode.PlayerBlockChoise);
        if (IndexFromSaveNode < 8)
        {
            actualChancesToSpawnDifferentType[IndexFromSaveNode] = defChancesToSpawnDifferentType[IndexFromSaveNode];
        }
        else {
            Debug.Log(IndexFromSaveNode);
        }
        

        int x, y;
        //int strategyCounter = 1;
        //int internalCounter = 1;
        //float currentChangePartition = 0f;
        //float valueOne = 0f;
        for (y = 0; y < CreatorGrid.height; y++) {
            //adaptive chances
            /*

            lineCounter++;
            
            if (((float)lineCounter / (strategyStep * (float)strategyCounter)) >= (float)strategyCounter) {
                strategyCounter = Mathf.Min(++strategyCounter, spawnChanceStrategy.Length - 1);
                currentGoalSpawnChance = spawnChanceStrategy[strategyCounter];
                currentGoalSpawnChanceBoosters = spawnChanceStrategyBoosters[strategyCounter];
                if (internalCounter == strategyCounter)
                {
                    valueOne = (actualChancesToSpawnDifferentType[groupToBoost[strategyCounter] * 2] + actualChancesToSpawnDifferentType[groupToBoost[strategyCounter] * 2 + 1]);
                    internalCounter++;
                }
            }

            currentChangePartition = ((float)lineCounter - (((float)strategyCounter - 1) * (float)strategyStep)) / (float)strategyStep;
            ChanceToSpawnAnything = Mathf.Lerp(spawnChanceStrategy[strategyCounter - 1], currentGoalSpawnChance, currentChangePartition);
            BoostersSpawnChance = Mathf.Lerp(spawnChanceStrategyBoosters[strategyCounter - 1], currentGoalSpawnChanceBoosters, currentChangePartition);
            */        

            //non-finished part of adaptive below

            //float deltaPartOne = Mathf.Lerp(valueOne, 80.0f, currentChangePartition);
            //float deltaPartTwo = (actualChancesToSpawnDifferentType[groupToBoost[strategyCounter] * 2] + actualChancesToSpawnDifferentType[groupToBoost[strategyCounter] * 2 + 1]);
            //float delta = deltaPartOne - deltaPartTwo ;
            //delta = Mathf.Floor(delta * 100) / 100;
            //RunChoiseShifting(actualChancesToSpawnDifferentType, groupToBoost[strategyCounter], delta);

            for (x = 0; x < CreatorGrid.width; x++) {

                int[] Offset = new int[2];

                if (CreatorGrid.cells[x, y].IsEmpty)
                {
                    if (SpawnerTools.BinaryRandom(ChanceToSpawnAnything))
                    {
                        if (x <= 3 && CreatorGrid.cells[x + 1, y].IsEmpty)
                        {
                            if (x <= 2 && CreatorGrid.cells[x + 2, y].IsEmpty)
                            {
                                CasingFigure(x, y, CeilingSwitch(CreatorGrid.height - y - 1, SpawnerTools.ComplexRando(ChancesToSpawnDifferentType, 0)), SpawnerTools.BinaryRandom(ChanceToMakeCubeImmovable), out Offset);
                            }
                            else
                            {
                                CasingFigure(x, y, CeilingSwitch(CreatorGrid.height - y - 1, SpawnerTools.ComplexRando(ChancesToSpawnDifferentType, 1)), SpawnerTools.BinaryRandom(ChanceToMakeCubeImmovable), out Offset);
                            }
                        }
                        else
                        {
                            CasingFigure(x, y, CeilingSwitch(CreatorGrid.height - y - 1, SpawnerTools.ComplexRando(ChancesToSpawnDifferentType, 3)), SpawnerTools.BinaryRandom(ChanceToMakeCubeImmovable), out Offset);
                        }

                        OffsetMaker(x, y, Offset);
                    }
                }

                if ((Game.Instance.count == 1 && x == 2 && y == 3) == true)
                {
                    continue;
                }

                if (SpawnerTools.BinaryRandom(BoostersSpawnChance)) {
                    RunBoosterChances(x, y);
                }
            }
        }
    }

    #region Cube Type Adaptive Mechanics

    //ad hoc solution for adaptive random generator(needs rework)
    //void RunChoiseShifting(float[] array, int groupToBoost, float value)
    //{
    //    groupToBoost = groupToBoost * 2;
    //    float groupRatioOne = SpawnerTools.GetRatio(array[groupToBoost], array[groupToBoost + 1]);

    //    float groupRatioTwo = SpawnerTools.GetRatio(array[GetGroupsThatAreLeft(groupToBoost)[0]], array[GetGroupsThatAreLeft(groupToBoost)[0] + 1]);
    //    float groupRatioThree = SpawnerTools.GetRatio(array[GetGroupsThatAreLeft(groupToBoost)[1]], array[GetGroupsThatAreLeft(groupToBoost)[1] + 1]);

    //    array[groupToBoost] += value * groupRatioOne;
    //    array[groupToBoost + 1] += value * (1 - groupRatioOne);

    //    float intrGroupRatio;
    //    intrGroupRatio = SpawnerTools.GetGroupRatio(array[GetGroupsThatAreLeft(groupToBoost)[0]], array[GetGroupsThatAreLeft(groupToBoost)[0] + 1], array[GetGroupsThatAreLeft(groupToBoost)[1]], array[GetGroupsThatAreLeft(groupToBoost)[1] + 1]);

    //    float valuePartForGroup = value * intrGroupRatio;
    //    array[GetGroupsThatAreLeft(groupToBoost)[0]] -= valuePartForGroup * groupRatioTwo;
    //    array[GetGroupsThatAreLeft(groupToBoost)[0]+1] -= valuePartForGroup * (1 - groupRatioTwo);
    //    array[GetGroupsThatAreLeft(groupToBoost)[1]] -= (value - valuePartForGroup) * groupRatioThree;
    //    array[GetGroupsThatAreLeft(groupToBoost)[1]+1] -= (value - valuePartForGroup) * (1 - groupRatioTwo);
    //}

    //int[] GetGroupsThatAreLeft(int chosenGroup) {
    //    if (chosenGroup == 0) {
    //        return new int []{2, 4};
    //    } else if (chosenGroup == 2)
    //    {
    //        return new int[] { 0, 4 };
    //    }
    //    return new int[] { 0, 2};
    //}
    #endregion


    void RunBoosterChances(int x, int y) {
            switch (SpawnerTools.ComplexRando(BoostersTypesSpawnChance, 0))
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
                        if (newY < CreatorGrid.height)
                        {
                            try
                            {
                                CreatorGrid.cells[x + l, y + k].IsEmpty = false;
                            }
                            catch {
                                Debug.Log("Cells: " + (x + l) +","+ (y + k) + "doesn't exist");
                            }
                            
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
            { 112, "SingleV" },
            { 120, "Di Ver" },
            { 130, "Tri Ver" },
            { 210, "Di Hor" },
            { 220, "Cube" },
            { 222, "CubeV" },
            { 310, "Tri Hor" },
            
            { 111, "Immovable" },
            { 113, "ImmovableV" },
            { 121, "IM_Di Ver" },
            { 131, "IM_Tri Ver" },
            { 211, "IM_Di Hor" },
            { 221, "IM_Cube" },
            { 223, "IM_CubeV" },
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
            case 7:
                typeIndex = 112;
                CellOffset = new int[] { 0, 0 };
                break;
            case 8:
                typeIndex = 222;
                CellOffset = new int[] { 1, 1 };
                break;

            default:
                typeIndex = 110;
                Debug.LogError("Unknown Rando case");
                CellOffset = new int[] { 0, 0 };
                break;
        }
        GenerateFigure(typeIndex, isImmovable, x, y);
    }

    void ClearGrid(GridClass grid) {
        grid.ResetGrid();
        
    }

    void GameReset() {
        //lineCounter = 0;

        actualChanceToSpawnAnything = -1;
        actualChanceToMakeCubeImmovable = -1;
        actualBoostersSpawnChance = -1;
        actualBoostersTypesSpawnChance = null;

        currentGoalSpawnChance = spawnChanceStrategy[1];
        currentGoalSpawnChanceBoosters = spawnChanceStrategyBoosters[1];

        CopyArray(defChancesToSpawnDifferentType, actualChancesToSpawnDifferentType);
    }
}
