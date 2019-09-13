﻿using System.Collections.Generic;

public class Bomb : Booster
{
    List<Block> BlocksToDestroy;

    public override string BoosterName => "Small Bomb";
    public override int MaxInInventory => 3;

    private Block playerBlock;
    
    public override void Activate(Cell cell)
    {
        playerBlock = Game.Instance.Player.playerBlock;
        GetSurroundingBlocks(cell);
        BlocksToDestroy.Remove(playerBlock);
        Explode();
    }

    //get blocks pull
    void GetSurroundingBlocks(Cell myCell)
    {
        BlocksToDestroy = new List<Block>();

        int OriginX = myCell.XPos;
        int OriginY = myCell.YPos;

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if ((x == 0 && y == 0) == false)
                {
                    if ((OriginX + x >= 0 && OriginX + x < myCell.GridClass.width) && (OriginY + y >= 0 && OriginY + y < myCell.GridClass.height))
                    {
                        Cell cell = myCell.GridClass.cells[OriginX + x, OriginY + y];
                        Block block = cell.Element?.myBlock;
                        if (block)
                        {
                            BlocksToDestroy.Add(block);
                        }
                    }
                }
            }
        }
    }

    //destroy if they are not destoyed by level already
    void Explode()
    {
        foreach (Block item in BlocksToDestroy)
        {
            item?.SelfDestroy();
        }
    }
}
