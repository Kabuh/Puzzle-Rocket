using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Laser Horizontal", menuName = "Boosters/Lazer Horizontal")]
public class LaserH : BoosterType
{
    protected List<Block> BlocksToDestroy;
    protected Block playerBlock;

    public override void Activate(Cell cell)
    {
        playerBlock = Game.Instance.Player.playerBlock;
        GetBlocks(cell, Axis.Horizontal);
        BlocksToDestroy.Remove(playerBlock);
        Cut(cell, Axis.Horizontal);
    }

    protected void GetBlocks(Cell myCell, Axis axis)
    {
        BlocksToDestroy = new List<Block>();

        if (axis == Axis.Horizontal)
        {
            int OriginX = myCell.XPos;

            for (int i = -2; i <= 2; i++)
            {
                if (i != 0)
                {
                    if (OriginX + i >= 0 && OriginX + i < myCell.GridClass.width)
                    {
                        Cell cell = myCell.GridClass.cells[OriginX + i, myCell.YPos];
                        Block block = cell.Element?.myBlock;
                        if (block != null)
                        {
                            BlocksToDestroy.Add(block);
                        }
                    }
                }
            }
        }
        else if (axis == Axis.Vertical)
        {
            int OriginY = myCell.YPos;

            for (int i = -2; i <= 2; i++)
            {
                if (i != 0)
                {
                    if (OriginY + i >= 0 && OriginY + i < myCell.GridClass.height)
                    {
                        Cell cell = myCell.GridClass.cells[myCell.XPos, OriginY + i];
                        Block block = cell.Element?.myBlock;
                        if (block != null)
                        {
                            BlocksToDestroy.Add(block);
                        }
                    }
                }
            }
        }

    }

    protected void Cut(Cell cell, Axis axis)
    {
        AnimationFX.Instance.PlayLaserFx(cell.GridClass.GetCellWorldPosition(cell), axis);
        foreach (Block item in BlocksToDestroy)
        {
            item?.SelfDestroy();
        }
    }
}
