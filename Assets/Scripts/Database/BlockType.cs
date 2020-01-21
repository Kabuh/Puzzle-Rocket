using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockType
{
    public string Name;
    public BlockType ParentBlockType;
    public int Height;
    public int Width;
    public float SpawnChance;
    public bool IsVertical;
    public bool IsImmovable;
    public GameObject BlockObject;

    public static List<BlockType> Immovables = new List<BlockType>();
    public static List<BlockType> All = new List<BlockType>();
    public static List<BlockType> MidWidthOrLess = new List<BlockType>();
    public static List<BlockType> LowWidth = new List<BlockType>();



    public BlockType(string name, BlockType parentBlockType, int height, int width, float spawnChance, bool isVertical, bool isImmovable, GameObject blockObject)
    {
        Name = name;
        ParentBlockType = parentBlockType;
        Height = height;
        Width = width;
        if (width <= 2) {
            if (width <= 1)
            {
                LowWidth.Add(this);
            }
            MidWidthOrLess.Add(this);
        }

        SpawnChance = spawnChance;
        IsVertical = isVertical;
        IsImmovable = isImmovable;
        if (IsImmovable == true) {
            SpawnChance = parentBlockType.SpawnChance * Database.immovableChanceMultiplier;
            Immovables.Add(this);
        }
        BlockObject = blockObject;

        All.Add(this);
    }
}
