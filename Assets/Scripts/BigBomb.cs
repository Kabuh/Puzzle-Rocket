using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBomb : Bomb
{
    public override string BoosterName => "Big Bomb";
    public override int MaxInInventory => 1;

    public override void Activate(Cell cell)
    {
        playerBlock = Game.Instance.Player.playerBlock;
        GetSurroundingBlocks(cell, 5);
        Debug.Log("Line Reached");
        BlocksToDestroy.Remove(playerBlock);
        Explode();
    }
}
