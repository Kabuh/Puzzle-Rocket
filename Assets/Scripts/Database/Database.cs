using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="Database", menuName ="Database")]
public class Database : ScriptableObject
{
    [Header("Global Settings")]
    public float immovableChanceMultiplier;

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
    public BoosterType Bomb;
    public BoosterType BigBomb;
    public BoosterType LaserH;
    public BoosterType LaserV;
    public BoosterType TimeSlow;
    public BoosterType TimeStop;
    public BoosterType Shot;
    public BoosterType Shrink;
    public BoosterType Breaker;
    public BoosterType Teleport;
    public BoosterType ImmovableSwitch;
    public BoosterType ImmovalbeDestroyer;

    #endregion
}
