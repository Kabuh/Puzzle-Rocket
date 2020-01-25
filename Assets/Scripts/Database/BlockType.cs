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

    //public BlockType(string name, BlockType parentBlockType, int height, int width, float spawnChance, bool isVertical, bool isImmovable, GameObject blockObject)
    //{
    //    Debug.Log("boop");
    //    Name = name;
    //    ParentBlockType = parentBlockType;
    //    Height = height;
    //    Width = width;
    //    if (width <= 2) {
    //        if (width <= 1)
    //        {
    //            LowWidth.Add(this);
    //        }
    //        MidWidthOrLess.Add(this);
    //    }

    //    SpawnChance = spawnChance;
    //    IsVertical = isVertical;
    //    IsImmovable = isImmovable;
    //    if (IsImmovable == true) {
    //        SpawnChance = parentBlockType.SpawnChance * Database.immovableChanceMultiplier;
    //        Immovables.Add(this);
    //    }
    //    BlockObject = blockObject;

    //    All.Add(this);
    //}
}
