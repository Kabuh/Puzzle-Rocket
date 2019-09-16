using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterObject : MonoBehaviour
{
    public BoosterType boosterType;

    public int xIndex;
    public int yIndex;
    public Vector2 position;
    public Cell myCell;

    Block playerBlock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerBlock = collision.gameObject.GetComponent<Block>();
        if (playerBlock != null)
        {
            Inventory.Instance.TryAddBooster(BoosterManager.Instance.boosters[boosterType], myCell);
        }
        SelfDestroy();
    }

    public void SetCell() {
        position = transform.position;
        myCell = myCell = Game.Instance.CombinedGrid.WorldPosToCell(transform.position);
        xIndex = myCell.XPos;
        yIndex = myCell.YPos;
    }

    public Cell GetCell() {
        return this.myCell;
    }

    public void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}
