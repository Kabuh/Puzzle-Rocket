using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public int x;
    public int y;
    public Vector2 position;
    public Cell myCell;
    

    public void SetCell() {
        position = transform.position;
        myCell = myCell = Game.Instance.CombinedGrid.WorldPosToCell(transform.position);
        x = myCell.XPos;
        y = myCell.YPos;
    }

    public Cell GetCell() {
        return this.myCell;
    }

    public void SelfDestroy()
    {
        Destroy(this.gameObject);
    }





}
