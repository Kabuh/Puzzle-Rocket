using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterObject : MonoBehaviour, ISpawnable
{
    public BoosterType boosterType;

    public int xIndex;
    public int yIndex;
    public Cell myCell;

    Block playerBlock;

    private void Awake()
    {
        SetCell();

        Game.LowerDestruction += DestroyIfLower;
        Game.AllDestruction += SelfDestroy;
        Game.CellLevelShift += ChangeCellsLevel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerBlock = collision.gameObject.GetComponent<Block>();
        if (playerBlock != null)
        {
            Inventory.Instance.TryAddBooster(BoosterManager.Instance.boosters[boosterType], myCell);
        }

        SelfDestroy();
    }

    public void SetCell()
    {
        myCell = myCell = Game.Instance.CombinedGrid.WorldPosToCell(transform.position);
        xIndex = myCell.XPos;
        yIndex = myCell.YPos;
    }

    public Cell GetCell()
    {
        return this.myCell;
    }    

    public void SelfDestroy()
    {
        Game.LowerDestruction -= DestroyIfLower;
        Game.AllDestruction -= SelfDestroy;
        Game.CellLevelShift -= ChangeCellsLevel;

        Destroy(this.gameObject);
    }

    public bool ShouldBeDestroyed()
    {
        if (myCell.YPos <= 7)
            return true;
        return false;
    }

    public void ChangeCellsLevel()
    {
        myCell = Game.Instance.CombinedGrid.cells[myCell.XPos, myCell.YPos - 8];
    }

    public void DestroyIfLower()
    {
        if(ShouldBeDestroyed())
        {
            SelfDestroy();
        }
    }
}
