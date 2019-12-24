using UnityEngine;

public class BoosterObject : MonoBehaviour, ISpawnable
{
    public BoosterType boosterType;

    public int xIndex;
    public int yIndex;
    public Cell myCell;
    public GameObject artChild;
    private Sprite sprite;

    Block playerBlock;

    private void Awake()
    {
        SetCell();

        Game.LowerDestruction += DestroyIfLower;
        Game.AllDestruction += SelfDestroy;
        Game.CellLevelShift += ChangeCellsLevel;
        sprite = artChild.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerBlock = collision.gameObject.GetComponent<Block>();
        if (playerBlock != null)
        {
            Inventory.Instance.TryAddBooster(BoosterManager.Instance.boosters[boosterType], myCell, sprite);
        }

        SelfDestroy();
    }

    public void SetCell()
    {
        myCell = Game.Instance.CombinedGrid.WorldPosToCell(transform.position);
        xIndex = myCell.XPos;
        yIndex = myCell.YPos;
    }

    public Cell GetCell()
    {
        return this.myCell;
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

    public void ChangeCellsLevel()
    {
        myCell = Game.Instance.CombinedGrid.cells[myCell.XPos, myCell.YPos - 8];
    }

    public void DestroyIfLower()
    {
        if(ShouldBeDestroyed())
        {
            SelfDestroy();
        }
    }
}
