using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="Database", menuName ="Database")]
public class Database : ScriptableObject
{
    [Header("Global Settings")]
    public float immovableChanceMultiplier;
    public float spawnNothing;

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

    public List<BlockType> All = new List<BlockType>();
    public List<BlockType> Immovables = new List<BlockType>();
    public List<BlockType> MidWidthOrLess = new List<BlockType>();
    public List<BlockType> LowWidth = new List<BlockType>();

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

        

        All.Add( SingleH);
        All.Add( SingleV);
        All.Add( IM_SingleH);
        All.Add( IM_SingleV);

        All.Add( PairH);
        All.Add( PairV);
        All.Add( IM_PairH);
        All.Add( IM_PairV);

        All.Add( TripleH);
        All.Add( TripleV);
        All.Add( IM_TripleH);
        All.Add( IM_TripleV);

        All.Add( CubeH);
        All.Add( CubeV);
        All.Add( IM_CubeH);
        All.Add( IM_CubeV);


        for (int i = 0; i < All.Count; i++) {

            if (All[i].IsImmovable)
            {
                Immovables.Add(All[i]);
            }

            if (All[i].Width <= 2)
            {
                if (All[i].Width <= 1)
                {
                    LowWidth.Add(All[i]);
                }
                MidWidthOrLess.Add(All[i]);
            }
        }

        foreach (BlockType item in Immovables)
        {
            if (item.ParentBlockType != null)
            {
                item.SpawnChance = item.ParentBlockType.SpawnChance * immovableChanceMultiplier;
            }
        }
    }
}
