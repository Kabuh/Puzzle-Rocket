using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserV : LaserH
{
    public override string BoosterName => "Frontal lasers";
    public override int MaxInInventory => 3;

    public override void Activate(Cell cell)
    {
        playerBlock = Game.Instance.Player.playerBlock;
        GetBlocks(cell, Axis.Vertical);
        BlocksToDestroy.Remove(playerBlock);
        Cut(cell, Axis.Vertical);
    }
}
