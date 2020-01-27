using UnityEngine;

public class Element : MonoBehaviour
{
    public Cell myCell;    

    public Block myBlock;      

    public void SetCell()
    {        
        myCell = Game.Instance.CombinedGrid.WorldPosToCell(transform.position);
        myCell.Element = this;
        myCell.IsEmpty = false;             
    }

    public void ReassignCell()
    {
        myCell.IsEmpty = true;
        myCell.Element = null;
        SetCell();
    }

    //read proposed destination data
    public Cell GetNeighbourCell(Directions dir, int jump)            
    {
        return myCell.GridClass.GetNeighbour(dir, myCell, jump);
    }
}
