using UnityEngine;
using System.Collections.Generic;

public class Teleport : Booster
{
    public override string BoosterName => "Teleport";

    public override int MaxInInventory => 1;

    private Camera cam;
    private GridClass grid;

    public Teleport(Camera cam)
    {
        this.cam = cam;
        grid = Game.Instance.CombinedGrid;
    }

    public override void Activate(Cell cell)
    {
        // get visible cells array

        Cell camCell = grid.WorldPosToCell(cam.transform.position);

        // 5 CAN BE REPLACED WITH FORMULA
        int yStart = Mathf.Clamp(camCell.YPos - 5, 1, 100);
        int yEnd = Mathf.Clamp(camCell.YPos + 5, 1, grid.height);

        // get empty and 1 block cells

        List<Cell> goodCells = new List<Cell>();

        for (int i = yStart; i <= yEnd; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Cell c = grid.cells[j, i];
                if(c.IsEmpty||c.Element.myBlock.elements.Length==1)
                {
                    if (c != cell)
                    {
                        goodCells.Add(c);
                    }
                }
            }            
        }      
        

        // choose random

        Cell randomCell = goodCells[Random.Range(0, goodCells.Count)];

        Block playerBlock = Game.Instance.Player.playerBlock;

        Vector3 playerPos = playerBlock.transform.position;

        // move block if it was

        Block randomBlock;

        if (randomCell.Element != null)
        {
            randomBlock = randomCell.Element.myBlock;
            randomBlock.MoveToPosition(playerPos);
        }

        // place player to newCell

        playerBlock.MoveToPosition(grid.GetCellWorldPosition(randomCell));           
    }
}
