using UnityEngine;

public class Coin : MonoBehaviour, ISpawnable
{
    private Cell myCell;

    private void Awake()
    {
        SetCell();

        Game.LowerDestruction += DestroyIfLower;
        Game.AllDestruction += SelfDestroy;
        Game.CellLevelShift += ChangeCellsLevel;
    }

    private void SetCell()
    {
        myCell = myCell = Game.Instance.CombinedGrid.WorldPosToCell(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player playerBlock = collision.gameObject.GetComponent<Player>();
        if (playerBlock != null)
        {
            Inventory.Instance.AddCoins(1);
        }

        SelfDestroy();
    }

    public void ChangeCellsLevel()
    {
        myCell = Game.Instance.CombinedGrid.cells[myCell.XPos, myCell.YPos - 8];
    }

    public void DestroyIfLower()
    {
        if (ShouldBeDestroyed())
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy()
    {
        Game.LowerDestruction -= DestroyIfLower;
        Game.AllDestruction -= SelfDestroy;
        Game.CellLevelShift -= ChangeCellsLevel;

        Destroy(this.gameObject);
    }

    public bool ShouldBeDestroyed()
    {
        if (myCell.YPos <= 7)
            return true;
        return false;
    }
}
