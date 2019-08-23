using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Booster
{
    List<Block> BlocksToDestroy = new List<Block>();
    Block playerBlock;

    bool ExplosionOnPickUp = true;

    void BombActivate(int newExplosionOriginX, int newExplosionOriginY) {
        if (ExplosionOnPickUp)
        {
            GetSurroundingBlocks(xIndex, yIndex);
        }
        else {
            GetSurroundingBlocks(newExplosionOriginX, newExplosionOriginY);
        }
        BlocksToDestroy.Remove(playerBlock);
        Explode();
    }

    //get blocks pull
    void GetSurroundingBlocks(int OriginX, int OriginY) {
        for (int x = -1; x < 2; x++) {
            for (int y = -1; y < 2; y++) {
                if ((x == 0 && y == 0) == false) {
                    if ((OriginX + x >= 0 && OriginX + x < myCell.GridClass.width) && (OriginY + y >= 0 && OriginY + y < myCell.GridClass.height)) {
                        Cell cell = myCell.GridClass.cells[OriginX + x, OriginY + y];
                        Block block = cell.Element?.myBlock;
                        if (block)
                            BlocksToDestroy.Add(block);
                    }
                }
            }
        }
    }

    //destroy if they are not destoyed by level already
    void Explode() {
        foreach (Block item in BlocksToDestroy) {
            item?.SelfDestroy();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerBlock = collision.gameObject.GetComponent<Block>();
        if (playerBlock != null) {
            if (ExplosionOnPickUp)
            {
                GetSurroundingBlocks(xIndex, yIndex);
            }
            BlocksToDestroy.Remove(playerBlock);
            Explode();
            SelfDestroy();
        }
        
    }

}
