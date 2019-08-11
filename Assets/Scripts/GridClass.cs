using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridClass
{
    public string name;
    public Cell[,] cells;
    public int width;
    public int height;

    public float step;
    public Vector2 origin;

    public int halfHeight;

    public GridClass(int width, int height, float step, string name)
    {
        this.name = name;
        this.width = width;
        this.height = height;
        this.step = step;
        cells = new Cell[width, height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                cells[j, i] = new Cell(j, i, true, this);
            }
        }

        halfHeight = height / 2;

        SetOrigin();
    }

    
    public Cell GetNeighbour(Directions direction, Cell origin, int jump) 
    {
        int xPos = origin.XPos;
        int yPos = origin.YPos;
        switch (direction)                  // get neighbour coordinates
        {
            case Directions.Up:
                yPos += jump;
                break;
            case Directions.Left:
                xPos -= jump;
                break;
            case Directions.Right:
                xPos += jump;
                break;
            case Directions.Down:
                yPos -= jump;
                break;
        }
    

        if (xPos >= 0 && xPos < width)      // read cell by coordinates
        {            
            if (yPos >= 0 && yPos < height)
            {
                return cells[xPos, yPos];
            }
        }

        return null;
    }

    public Cell WorldPosToCell(Vector2 worldPos)    //read cell by vector2
    {
        Vector2 diff = worldPos - origin;
        int x = (int)(diff.x / step);
        int y = (int)(diff.y / step);
        return cells[x, y];
    }

    public Vector2 CellCoordToVect(int x, int y) {
        Vector2 holder;
        holder.x = x * step + origin.x;
        holder.y = y * step + origin.y;
        return holder;
    }

    public void ResetGrid()                         //rebuild grid with same paste data
    {
        foreach (Cell cell in cells)
        {
            cell.IsEmpty = true;
        }
        SetOrigin();
    }

    public void ShiftGrid()                         //delete grid
    {
        foreach (Cell cell in cells)
        {
            cell.IsEmpty = true;
        }        
    }

    private void SetOrigin()                        // create position data
    {        
        origin = new Vector2(0.5f * (step - width * step), 0.5f * (step - halfHeight * step));
    }

    public void AddOffsetToOrigin()                 //auto-offset
    {
        origin += new Vector2(0f, halfHeight);
    }
}
