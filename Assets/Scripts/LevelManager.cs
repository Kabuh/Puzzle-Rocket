using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string levelName;
    private int levelCounter;

    public Dictionary<int, LevelData> levels { get; private set; }

    public static LevelManager Instance { get; private set; }

    int[] sizeChancesIftwo = { 45, 15, 10, 15, 5, 10 };
    int[] sizeChancesIfOne = { 45, 20, 10, 15, 10 };
    int[] sizeChancesIfNone = { 65, 25, 10 };
    int[] typeChances = { 30, 10, 60 };
    GridClass CreatorGrid;
    public static string ArrayToString(int[] arr)
    {
        List<object> lst = new List<object>();
        object[] obj = new object[arr.Length];
        System.Array.Copy(arr, obj, arr.Length);

        lst.AddRange(obj);
        return string.Join(",", lst.ToArray());
    }

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


        //LoadLevels();
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

        Debug.Log("level 1 created with key " + levelCounter);
        levelDataItems.Clear();
        levelCounter++;
    }

    public void LevelDestroyer() {
        levelDataItems.Clear();
        levels.Clear();
        levelCounter = 1;
    }

    /// ////////////////////////////////////////

    public List<Text> testerUIprompts = new List<Text>();
    public List<Text> testerUIcheckSums = new List<Text>();
    public Text ConsoleUI;


    //public Text promptOne;
    //public Text CheckSum;

    int[] Parser(Text prompt)
    {
        string textLine = prompt.text;
        string holderString = "";
        int currentIndex = 0;
        List<int> chanceNumbers = new List<int>();
        for (int i = 0; i < textLine.Length; i++) {
            string k = textLine[i].ToString();
            if (k == " ")
            {
                currentIndex = 0;

                chanceNumbers.Add(int.Parse(holderString));
                holderString = "";
            }
            else {
                holderString = holderString.Insert(currentIndex, k);
                currentIndex++;
            }
        }

        return chanceNumbers.ToArray();
    }

    void CheckSumCheck(Text prompt, Text CheckSum) {
        CheckSum.text = "CheckSum:" + Sum(Parser(prompt));
    }

    int Sum(int[] array) {
        int total = 0;
        foreach (int num in array) {
            total += num;
        }
        return total;
    }



    public void MakeNewChances() {
        NullCheckerEqual(Parser(testerUIprompts[0]), sizeChancesIftwo);
        NullCheckerEqual(Parser(testerUIprompts[1]), sizeChancesIfOne);
        NullCheckerEqual(Parser(testerUIprompts[2]), sizeChancesIfNone);
        NullCheckerEqual(Parser(testerUIprompts[3]), typeChances);

        testField.text = "New chances are: \n " + ArrayToString(typeChances) + "| \n" + ArrayToString(sizeChancesIftwo) + "| /n" + ArrayToString(sizeChancesIfOne) + "| /n" + ArrayToString(sizeChancesIfNone);
    }

    void NullCheckerEqual(int[] newData, int[]dataSlot) {
        if (newData != null )
        { //&& newData.Length == dataSlot.Length
            if (newData.Length == dataSlot.Length)
            {
                System.Array.Copy(newData, dataSlot, newData.Length);
                ConsoleUI.text += "\n chances created succesfuly ";
            }
            else {
                ConsoleUI.text += "\n check array length";
            }
            
        }
    }




    public Text testField;

    private void Update()
    {
        if (Input.GetKeyDown("g")) {
            testField.text = "";
            CreateNewLevel(); ;
        }
        for (int i = 0; i < testerUIprompts.Count; i++)
        {
            if (testerUIprompts[i].text.Length > 0)
            {
                CheckSumCheck(testerUIprompts[i], testerUIcheckSums[i]);
            }
        }
        
    }

    
    public void TestIsEmpty() {
        foreach (var cell in CreatorGrid.cells) {
            if (cell.IsEmpty)
            {
                testField.text += "0 ";
            }
            else {
                testField.text += "1 ";
            } 
        }
    }

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

                    if (Offset != null)
                    {
                        for (int k = 0; k <= Offset[1]; k++)
                        {
                            for (int l = 0; l <= Offset[0]; l++)
                            {
                                if (k == 0 && l == 0) {
                                    
                                }
                                else
                                {
                                    int newX = x + l;
                                    int newY = y + k;
                                    if (newY >= CreatorGrid.height)
                                    {
                                        
                                    }
                                    else {
                                        CreatorGrid.cells[x + l, y + k].IsEmpty = false;
                                    }
                                }
                            }
                        }
                    }
                }
                else {
                    testField.text += "T_";
                }   
            }
            testField.text += "\n";
        }
    }

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
        
        
        testField.text += "3_";   }
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
        
        testField.text += "2_";
        }
    void GenerateCube   (bool isImmovable, int currentIndexX, int currentIndexY) {
        if (isImmovable)
        {
            GenerateBlock("IM_Cube", currentIndexX, currentIndexY);
        }
        else {
            GenerateBlock("Cube", currentIndexX, currentIndexY);
        }
        testField.text += "C_"; }
    void GenerateSingle (bool isImmovable, int currentIndexX, int currentIndexY) {
        if (isImmovable)
        {
            GenerateBlock("Immovable", currentIndexX, currentIndexY);
        }
        else {
            GenerateBlock("Single", currentIndexX, currentIndexY);
        }
        
        testField.text += "1_"; }
    void GenerateEmpty  () { testField.text += "[_"; }

    void GenerateBlock(string name, int x, int y) {
        levelDataItems.Add(new LevelDataItem(name, x, y));
    }

    /*
    bool Rando(int percentage) {
        float x = Random.Range(0, 100);
        if (x < percentage)
        {
            return true;
        }
        else {
            return false;
        }
    }
    */

    void CasingType(int x, int y, int Size, int Type, out int[] CellOffsetI) {
        int[] OffsetArray = new int[2];
        switch (Type) {
            case 1:
                CasingFigure(x, y ,Size, false, out OffsetArray);
                break;
            case 2:
                CasingFigure(x, y, Size, true, out OffsetArray);
                break;
            case 3:
                GenerateEmpty();
                break;
            default:
                Debug.Log("Unknown Rando case");
                OffsetArray = new int[] { 0, 0 };
                break;
        }
        CellOffsetI = OffsetArray;
    }

    void CasingFigure(int x, int y, int RandoResult, bool isImmovable ,out int[] CellOffset) {
        switch (RandoResult) {
            case 1:
                GenerateSingle(isImmovable, x, y);
                CellOffset = new int[] { 0, 0 };
                break;
            case 2:
                GenerateDouble(isImmovable, x, y, Directions.Vertical);
                CellOffset = new int[] { 0, 1 };
                break;
            case 3:
                GenerateTriple(isImmovable, x, y, Directions.Vertical);
                CellOffset = new int[] { 0, 2 };
                break;
            case 4:
                GenerateDouble(isImmovable, x, y, Directions.Horizontal);
                CellOffset = new int[] { 1, 0 };
                break;
            case 5:
                GenerateCube(isImmovable, x, y);
                CellOffset = new int[] { 1, 1 };
                break;
            case 6:
                GenerateTriple(isImmovable, x, y, Directions.Horizontal);
                CellOffset = new int[] { 2, 0 };
                break;
            default:
                Debug.Log("Unknown Rando case");
                CellOffset = new int[] { 0, 0 };
                break;
        } 
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
}
