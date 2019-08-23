using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    public Vector2 position;
    public Cell myCell;
    

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
