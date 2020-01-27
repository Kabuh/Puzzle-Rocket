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

        Game.AllDestruction += SelfDestroy;
        sprite = artChild.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerBlock = collision.gameObject.GetComponent<Block>();
        if (playerBlock != null)
        {
            Inventory.Instance.TryAddBooster(boosterType, myCell, sprite);
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
        Game.AllDestruction -= SelfDestroy;

        Destroy(this.gameObject);
    }    

    public void ChangeCellsLevel()
    {
        myCell = Game.Instance.CombinedGrid.cells[myCell.XPos, myCell.YPos - 8];
    }    
}
