using UnityEngine;

public class Element : MonoBehaviour
{
    public Cell myCell;

    private GridClass gridClass;

    public Vector2 vectorData;
    public bool isempty;

    public Block myBlock;

    private void Start()
    {
        //gridClass = Game.Instance.CombinedGrid;
        //SetCell();
    }    

    public void SetCell()
    {
        gridClass = Game.Instance.CombinedGrid;
        if (gridClass != null)
        {
            myCell = gridClass.WorldPosToCell(transform.position);
            myCell.Element = this;
            myCell.IsEmpty = false;
            vectorData = new Vector2(myCell.XPos, myCell.YPos);
            isempty = myCell.IsEmpty;
            try
            {
                
            }
            catch (System.IndexOutOfRangeException)
            {
                Debug.Log(transform.position);
            }
        }
        else {
            Debug.Log("lost grid");
        }
        
    }
    
    /*
    public void ShiftCell(Directions dir)                   //set properties in destination location
    {
        Cell = Cell.GridClass.GetNeighbour(dir, Cell);
    }
    */

    public void ShiftCellByPos(Vector2 cellPos) {           //teleport properties
        myCell = myCell.GridClass.WorldPosToCell(cellPos);
    }

    //read proposed destination data
    public Cell GetNeighbourCell(Directions dir)            
    {
        for (int jump = 1; jump <= myCell.GridClass.width; jump++) {
            Cell cell = myCell.GridClass.GetNeighbour(dir, myCell, jump);
            if (cell != null) {
                if (!cell.IsEmpty)
                {
                    if (this.transform.parent != cell?.Element?.transform.parent)
                    {
                        //Debug.Log(dir.ToString() + " neighnour is taken");
                        return cell;
                    }
                }
                else
                {
                    //Debug.Log(dir.ToString() + " neighnour is empty");
                    return cell;
                }
            }
        }
        return null;
    }

    public Cell WorldPosToCell(Vector2 worldPos) {
        return myCell.GridClass.WorldPosToCell(worldPos);
    }

    public void ChangeCellLevel()                           //something about seamless level switch???
    {
        myCell = gridClass.cells[myCell.XPos, myCell.YPos - 8];
        myCell.IsEmpty = false;
    }

}
