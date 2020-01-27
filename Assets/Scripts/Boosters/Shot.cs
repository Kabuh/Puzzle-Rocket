using UnityEngine;

[CreateAssetMenu(fileName = "Shot", menuName = "Boosters/Shot")]
public class Shot : BoosterType
{    
    protected Block playerBlock;
    Block targetBlock;

    public override void Activate(Cell cell)
    {
        playerBlock = Game.Instance.Player.playerBlock;
        GetTargetBlock(cell);
        Explode(cell);
    }

    void GetTargetBlock(Cell myCell)
    {
        Block newTarget = null;
        for (int y = myCell.YPos+1; y < myCell.GridClass.height; y++)
        {
            if (!myCell.GridClass.cells[myCell.XPos, y].IsEmpty)
            {
                if (myCell.GridClass.cells[myCell.XPos, y].Element.myBlock != playerBlock)
                {
                    newTarget = myCell.GridClass.cells[myCell.XPos, y].Element?.myBlock;
                    break;
                }
            }
        }

        if (newTarget != null)
        {
            targetBlock = newTarget;
        }
    }

    void Explode(Cell cell)
    {
        if (targetBlock != null)
        {
            AnimationFX.Instance.PlayShootingAnimation(cell.GridClass.GetCellWorldPosition(cell), targetBlock);
        }
        else
        {
            AnimationFX.Instance.PlayFakeShooting(cell.GridClass.GetCellWorldPosition(cell));
        }

    }
}
