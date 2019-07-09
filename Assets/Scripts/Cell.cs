public class Cell
{  
    public int XPos { get; set; }
    public int YPos { get; set; }
    public bool IsEmpty { get; set; }
    public GridClass GridClass { get; set; }
    public Element Element { get; set; }    

    public Cell(int xPos, int yPos, bool isEmpty, GridClass grid)
    {
        GridClass = grid;
        XPos = xPos;
        YPos = yPos;
        IsEmpty = isEmpty;
    }
}
