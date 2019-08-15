using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private string levelName;
    private int levelCounter;

    public Dictionary<int, LevelData> levels { get; private set; }

    public static LevelManager Instance { get; private set; }

    public int[] sizeChancesIftwo = { 45, 15, 10, 15, 5, 10 };
    public int[] sizeChancesIfOne = { 45, 20, 10, 15, 10 };
    public int[] sizeChancesIfNone = { 65, 25, 10 };
    public int[] typeChances = { 30, 10, 60 };
    GridClass CreatorGrid;





    private void Awake()
    {
        levelCounter = 1;

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

        Game.CreateData += CreateNewLevel;
    }

    List<LevelDataItem> levelDataItems = new List<LevelDataItem>();

    void CreateNewLevel() {
        CreatorGrid = new GridClass(Game.Instance.CombinedGrid.width, Game.Instance.CombinedGrid.halfHeight, Game.Instance.CombinedGrid.step, "CreatorGrid");

        levelName = "1";

        ClearGrid(CreatorGrid);
        RandomLevelCreator();

        LevelData levelData = new LevelData();
        levelData.items = levelDataItems.ToArray();

        levels = new Dictionary<int, LevelData>();

        levels.Add(levelCounter, levelData);
        levelDataItems.Clear();
        levelCounter++;
    }

    public void LevelDestroyer() {
        levelDataItems.Clear();
        levels.Clear();
        levelCounter = 1;
    }

    /// ////////////////////////////////////////


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
            }
        }
    }

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




    void GenerateFigure(int type, bool isImmovable, int currentIndexX, int currentIndexY) {
        if (isImmovable) {
            type++;
        }

        GenerateBlock(FigureType[type], currentIndexX, currentIndexY);
    }

    void GenerateBlock(string name, int x, int y) {
        levelDataItems.Add(new LevelDataItem(name, x, y));
    }

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

    void CasingFigure(int x, int y, int RandoResult, bool isImmovable ,out int[] CellOffset) {
        int typeIndex;

        switch (RandoResult) {
            case 1:
                typeIndex = 110;
                //GenerateSingle(isImmovable, x, y);
                CellOffset = new int[] { 0, 0 };
                break;
            case 2:
                typeIndex = 120;
                //GenerateDouble(isImmovable, x, y, Directions.Vertical);
                CellOffset = new int[] { 0, 1 };
                break;
            case 3:
                typeIndex = 130;
                //GenerateTriple(isImmovable, x, y, Directions.Vertical);
                CellOffset = new int[] { 0, 2 };
                break;
            case 4:
                typeIndex = 210;
                //GenerateDouble(isImmovable, x, y, Directions.Horizontal);
                CellOffset = new int[] { 1, 0 };
                break;
            case 5:
                typeIndex = 220;
                //GenerateCube(isImmovable, x, y);
                CellOffset = new int[] { 1, 1 };
                break;
            case 6:
                typeIndex = 310;
                //GenerateTriple(isImmovable, x, y, Directions.Horizontal);
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

    /*
    public void LoadLevels() // level reader
    {
        levels = new Dictionary<string, LevelData>();

        //generating levels layout save directory
        string[] files = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Levels"));

        if (files == null) {
            return;
        }

        //parser
        foreach (var item in files)
        {
            //file extension checker?
            if (Path.GetExtension(item) != ".json") {
                continue;
            }

            //level data class creating new object - what is lavel data class?
            LevelData levelData = JsonUtility.FromJson<LevelData>(File.ReadAllText(item));

            //just a level name, no fuss
            string levelName = Path.GetFileNameWithoutExtension(item);

            //adding new data to all levels stack - THERE IS A STACK
            levels.Add(levelName, levelData);
        }
    }
    */


    /*
    void GenerateTriple (bool isImmovable, int currentIndexX, int currentIndexY, Directions dir) {
        if (isImmovable)
        {
            if (dir == Directions.Horizontal)
            {
                GenerateBlock("IM_Tri Hor", currentIndexX, currentIndexY);
            }
            if (dir == Directions.Vertical)
            {
                GenerateBlock("IM_Tri Ver", currentIndexX, currentIndexY);
            }
        }
        else {
            if (dir == Directions.Horizontal)
            {
                GenerateBlock("Tri Hor", currentIndexX, currentIndexY);
            }
            if (dir == Directions.Vertical)
            {
                GenerateBlock("Tri Ver", currentIndexX, currentIndexY);
            }
        }
    }

    void GenerateDouble (bool isImmovable, int currentIndexX, int currentIndexY, Directions dir) {
        if (isImmovable)
        {
            if (dir == Directions.Horizontal)
            {
                GenerateBlock("IM_Di Hor", currentIndexX, currentIndexY);
            }
            if (dir == Directions.Vertical)
            {
                GenerateBlock("IM_Di Ver", currentIndexX, currentIndexY);
            }
        }
        else {
            if (dir == Directions.Horizontal)
            {
                GenerateBlock("Di Hor", currentIndexX, currentIndexY);
            }
            if (dir == Directions.Vertical)
            {
                GenerateBlock("Di Ver", currentIndexX, currentIndexY);
            }
        }
    }
    void GenerateCube   (bool isImmovable, int currentIndexX, int currentIndexY) {
        if (isImmovable)
        {
            GenerateBlock("IM_Cube", currentIndexX, currentIndexY);
        }
        else {
            GenerateBlock("Cube", currentIndexX, currentIndexY);
        }
    }
    void GenerateSingle (bool isImmovable, int currentIndexX, int currentIndexY) {
        if (isImmovable)
        {
            GenerateBlock("Immovable", currentIndexX, currentIndexY);
        }
        else {
            GenerateBlock("Single", currentIndexX, currentIndexY);
        }    
    }
    */
}
