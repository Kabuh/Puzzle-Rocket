using UnityEngine;

public class Element : MonoBehaviour
{
    public Cell Cell { get; set; }

    private GridClass gridClass;

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
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.Log(transform.position);
        }
    }

    public void ShiftCell(Directions dir)                   //set properties in destination lacation
    {
        Cell = Cell.GridClass.GetNeighbour(dir, Cell);
    }

    public Cell GetNeighbourCell(Directions dir)            //read proposed destination data
    {
        return Cell.GridClass.GetNeighbour(dir, Cell);        
    }

    public void ChangeCellLevel()                           //something about seamless level switch???
    {
        Cell = gridClass.cells[Cell.XPos, Cell.YPos - 8];
        Cell.IsEmpty = false;
    }

}
