using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bomb", menuName = "Boosters/Bomb")]
public class Bomb : BoosterType
{
    protected List<Block> BlocksToDestroy;    

    protected Block playerBlock;    
    
    public override void Activate(Cell cell)
    {
        playerBlock = Game.Instance.Player.playerBlock;
        GetSurroundingBlocks(cell,3);
        BlocksToDestroy.Remove(playerBlock);
        Explode();
    }

    //get blocks pull
    protected void GetSurroundingBlocks(Cell myCell, int Range)
    {
        BlocksToDestroy = new List<Block>();

        int halfRange = (Range - 1) / 2;

        int OriginX = myCell.XPos;
        int OriginY = myCell.YPos;

        for (int x = -1 * halfRange; x <= halfRange; x++) {
            for (int y = -1 * halfRange; y <= halfRange; y++) {
                if ((x == 0 && y == 0) == false) {
                    if ((OriginX + x >= 0 && OriginX + x < myCell.GridClass.width) && (OriginY + y >= 0 && OriginY + y < myCell.GridClass.height)) {
                        Cell cell = myCell.GridClass.cells[OriginX + x, OriginY + y];
                        Block block = cell.Element?.myBlock;
                        if (block != null)
                        {
                            if (!BlocksToDestroy.Contains(block))
                                BlocksToDestroy.Add(block);
                        }
                    }
                }
            }
        }
    }

    //destroy if they are not destoyed by level already
    protected void Explode() {
        foreach (Block item in BlocksToDestroy) {
            foreach (Element element in item.elements) {
                AnimationFX.Instance.PlayExplosionFx(element.gameObject.transform.position);
            }
            item?.SelfDestroy();
        }

        Game.Instance.ExplosionEnd();
    }
}
