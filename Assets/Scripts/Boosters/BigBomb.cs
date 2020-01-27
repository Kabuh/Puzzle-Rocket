using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BigBomb",menuName = "Boosters/BigBomb")]
public class BigBomb : Bomb
{
    public override void Activate(Cell cell)
    {
        playerBlock = Game.Instance.Player.playerBlock;
        GetSurroundingBlocks(cell, 5);
        BlocksToDestroy.Remove(playerBlock);
        Explode();
    }
}
