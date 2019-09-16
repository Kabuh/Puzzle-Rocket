using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBomb : Bomb
{
    public BigBomb(Block playerBlock) : base(playerBlock)
    {
    }

    public override void Activate(Cell cell)
    {
        GetSurroundingBlocks(cell, 5);
        Debug.Log("Line Reached");
        BlocksToDestroy.Remove(playerBlock);
        Explode();
    }
}
