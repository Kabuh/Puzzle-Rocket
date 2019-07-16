using UnityEngine;

public class Element : MonoBehaviour
{
    public Cell Cell { get; set; }

    private GridClass gridClass;

    public Vector2 vectorData;
    public bool isempty;

    private void Start()
    {
        gridClass = Game.Instance.CombinedGrid;
        SetCell();
    }    

    public void SetCell()
    {
        try
        {
            Cell = gridClass.WorldPosToCell(transform.position);
            Cell.Element = this;
            Cell.IsEmpty = false;
            vectorData = new Vector2(Cell.XPos, Cell.YPos);
            isempty = Cell.IsEmpty;

        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.Log(transform.position);
        }
    }
    
    /*
    public void ShiftCell(Directions dir)                   //set properties in destination location
    {
        Cell = Cell.GridClass.GetNeighbour(dir, Cell);
    }
    */

    public void ShiftCellByPos(Vector2 cellPos) {           //teleport properties
        Cell = Cell.GridClass.WorldPosToCell(cellPos);
    }

    //read proposed destination data
    public Cell GetNeighbourCell(Directions dir)            
    {
        for (int jump = 1; jump <= Cell.GridClass.width; jump++) {
            Cell cell = Cell.GridClass.GetNeighbour(dir, Cell, jump);
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
        return Cell.GridClass.WorldPosToCell(worldPos);
    }

    public void ChangeCellLevel()                           //something about seamless level switch???
    {
        Cell = gridClass.cells[Cell.XPos, Cell.YPos - 8];
        Cell.IsEmpty = false;
    }

}
