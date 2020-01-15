using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Database", menuName ="Database")]
public class DatabaseAsset : ScriptableObject
{
    //block related
    public static GameObject[] blockPrefabs = new GameObject[16];
    public static float[] blockChances = new float[8];
    public static float immovableChanceMultiplier;

    //booster related
    public static GameObject[] boosterPrefabs = new GameObject[12];
    public static float[] boosterChances = new float[12];
    public static int[] maxInInventoryARR = new int[12];

    public struct BlockType
    {
        public string name;
        public int height;
        public int width;
        public float SpawnChance;
        public bool isVertical;
        GameObject blockObject;

        public BlockType(string n, int h, int w, float sc, bool isV, GameObject bO)
        {
            name = n;
            height = h;
            width = w;
            SpawnChance = sc;
            isVertical = isV;
            blockObject = bO;
        }
    }

    public static BlockType SingleH = new BlockType("SingleH", 1, 1, blockChances[0], false, blockPrefabs[0]);
    public static BlockType SingleV = new BlockType("SingleV", 1, 1, blockChances[1], false, blockPrefabs[1]);
    public static BlockType IM_SingleH = new BlockType("IM_SingleH", 1, 1, blockChances[0] * immovableChanceMultiplier, true, blockPrefabs[2]);
    public static BlockType IM_SingleV = new BlockType("IM_SingleV", 1, 1, blockChances[1] * immovableChanceMultiplier, true, blockPrefabs[3]);

    public static BlockType PairH = new BlockType("PairH", 1, 2, blockChances[2], false, blockPrefabs[4]);
    public static BlockType PairV = new BlockType("PairV", 2, 1, blockChances[3], false, blockPrefabs[5]);
    public static BlockType IM_PairH = new BlockType("IM_PairH", 1, 2, blockChances[2] * immovableChanceMultiplier, true, blockPrefabs[6]);
    public static BlockType IM_PairV = new BlockType("IM_PairV", 2, 1, blockChances[3] * immovableChanceMultiplier, true, blockPrefabs[7]);

    public static BlockType TripleH = new BlockType("PairH", 1, 3, blockChances[4], false, blockPrefabs[8]);
    public static BlockType TripleV = new BlockType("PairH", 3, 1, blockChances[5], false, blockPrefabs[9]);
    public static BlockType IM_TripleH = new BlockType("PairH", 1, 3, blockChances[4] * immovableChanceMultiplier, true, blockPrefabs[10]);
    public static BlockType IM_TripleV = new BlockType("PairH", 3, 1, blockChances[5] * immovableChanceMultiplier, true, blockPrefabs[11]);

    public static BlockType CubeH = new BlockType("PairH", 2, 2, blockChances[6], false, blockPrefabs[12]);
    public static BlockType CubeV = new BlockType("PairH", 2, 2, blockChances[7], false, blockPrefabs[13]);
    public static BlockType IM_CubeH = new BlockType("PairH", 2, 2, blockChances[6] * immovableChanceMultiplier, true, blockPrefabs[14]);
    public static BlockType IM_CubeV = new BlockType("PairH", 2, 2, blockChances[7] * immovableChanceMultiplier, true, blockPrefabs[15]);

    //====================================================================================================================================

    public struct BoosterTypes
    {
        public string name;
        public float SpawnChance;
        GameObject blockObject;
        int maxInInventory;

        public BoosterTypes(string nm, float spwnChns, GameObject bstrObj, int max)
        {
            name = nm;
            SpawnChance = spwnChns;
            blockObject = bstrObj;
            maxInInventory = max;
        }
    }

    public static BoosterTypes Bomb = new BoosterTypes(
        "Bomb",              
        boosterChances[0], boosterPrefabs[0], maxInInventoryARR[0]);
    public static BoosterTypes BigBomb = new BoosterTypes(
        "BigBomb",        
        boosterChances[1], boosterPrefabs[1], maxInInventoryARR[1]);
    public static BoosterTypes LaserH = new BoosterTypes(
        "LaserH",          
        boosterChances[2], boosterPrefabs[2], maxInInventoryARR[2]);
    public static BoosterTypes LaserV = new BoosterTypes(
        "LaserV",          
        boosterChances[3], boosterPrefabs[3], maxInInventoryARR[3]);
    public static BoosterTypes TimeSlow = new BoosterTypes(
        "TimeSlow",      
        boosterChances[4], boosterPrefabs[4], maxInInventoryARR[4]);
    public static BoosterTypes TimeStop = new BoosterTypes(
        "TimeStop",      
        boosterChances[5], boosterPrefabs[5], maxInInventoryARR[5]);
    public static BoosterTypes Missile = new BoosterTypes(
        "Missile",                            
        boosterChances[6], boosterPrefabs[6], maxInInventoryARR[6]);
    public static BoosterTypes Shrink = new BoosterTypes(
        "Shrink",                              
        boosterChances[7], boosterPrefabs[7], maxInInventoryARR[7]);
    public static BoosterTypes Breaker = new BoosterTypes(
        "Breaker",                            
        boosterChances[8], boosterPrefabs[8], maxInInventoryARR[8]);
    public static BoosterTypes Teleport = new BoosterTypes(
        "Teleport",                          
        boosterChances[9], boosterPrefabs[9], maxInInventoryARR[9]);
    public static BoosterTypes ImmovableSwitch = new BoosterTypes(
        "ImmovableSwitch",            
        boosterChances[10], boosterPrefabs[10], maxInInventoryARR[10]);
    public static BoosterTypes ImmovalbeDestroyer = new BoosterTypes(
        "ImmovalbeDestroyer",      
        boosterChances[11], boosterPrefabs[11], maxInInventoryARR[11]);
}
