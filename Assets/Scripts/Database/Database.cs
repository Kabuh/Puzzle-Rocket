using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="Database", menuName ="Database")]
public class Database : ScriptableObject
{
    [Header("Global Settings")]
    public static float immovableChanceMultiplier;

    #region Blocks Settings

    [Header("Blocks Settings")]
    public BlockType SingleH;
    public BlockType SingleV;
    public BlockType IM_SingleH;
    public BlockType IM_SingleV;

    public BlockType PairH;
    public BlockType PairV;
    public BlockType IM_PairH;
    public BlockType IM_PairV;

    public BlockType TripleH;
    public BlockType TripleV;
    public BlockType IM_TripleH;
    public BlockType IM_TripleV;

    public BlockType CubeH;
    public BlockType CubeV;
    public BlockType IM_CubeH;
    public BlockType IM_CubeV;

    #endregion

    

    #region Boosters Settings

    [Header("Boosters Settings")]
    public BoosterTypes Bomb;
    public BoosterTypes BigBomb;
    public BoosterTypes LaserH;
    public BoosterTypes LaserV;
    public BoosterTypes TimeSlow;
    public BoosterTypes TimeStop;
    public BoosterTypes Missile;
    public BoosterTypes Shrink;
    public BoosterTypes Breaker;
    public BoosterTypes Teleport;
    public BoosterTypes ImmovableSwitch;
    public BoosterTypes ImmovalbeDestroyer;

    #endregion

    private void Awake()
    {
        IM_SingleH.ParentBlockType = SingleH;
        IM_SingleV.ParentBlockType = SingleH;

        IM_PairH.ParentBlockType = PairH;
        IM_PairV.ParentBlockType = PairV;

        IM_TripleH.ParentBlockType = TripleH;
        IM_TripleV.ParentBlockType = TripleV;

        IM_CubeH.ParentBlockType = CubeH;
        IM_CubeV.ParentBlockType = CubeV;
    }

    public static void PopulateLevelManagerData() {
        Debug.Log(BlockType.All.Count);
    }
}
