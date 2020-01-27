using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour, ISpawnable
{
    public float moveSpeed;    
    [Header("Gameplay parameters")]
    public bool immovable;
    [Header("Elements")]
    public Element[] elements;
    public Element[] uElements;
    public Element[] lElements;
    public Element[] rElements;
    public Element[] dElements;

    private Vector3 destination;
    private Vector3 inputAndPivotdiff;

    public bool IsPlayer { get; set; }
    private bool isMoving;

    public GameObject artChild;
    private Sprite sprite;

    private Axis currentAxis;
    private float maxYposition;
    private float minYposition;
    private float minXposition;
    private float maxXposition;
    private Element[] currentElementsGroup;

    private Vector3 closestCellPosition;

    private void Awake()
    {
        foreach (Element element in elements)
        {
            element.SetCell();
        }
        foreach (var item in elements)
        {
            item.myBlock = this;
        }

        Game.AllDestruction += SelfDestroy;

        sprite = artChild.GetComponent<SpriteRenderer>().sprite;
    }

    private void Start()
    {
        // we don't need to calculate it for the first time because block is already
        // spawned on its closest cell
        closestCellPosition = transform.position;
    }

    public void CalculateInputAndPivotDiff(Vector3 input)
    {
        inputAndPivotdiff = input - transform.position;
    }

    public void SetAxis(Axis axis)
    {
        currentAxis = axis;
    }

    public void CalculateMovementConstraints()
    {
        switch(currentAxis)
        {
            case Axis.Horizontal:
                CalculateHorizontalConstraints();
                break;
            case Axis.Vertical:
                CalculateVerticalConstraints();
                break;
        }
    }

    public void UpdatePosition(Vector3 input)
    {
        Vector3 newPos = input - inputAndPivotdiff;
        switch (currentAxis)
        {
            case Axis.Horizontal:
                transform.position = new Vector3(Mathf.Clamp(newPos.x, minXposition, maxXposition), transform.position.y);
                break;
            case Axis.Vertical:
                transform.position = new Vector3(transform.position.x,Mathf.Clamp(newPos.y, minYposition, maxYposition));
                break;
        }

        // this may cost a lot?
        ReassignCells();
    }

    public void MoveToPosition(Vector3 pos)
    {
        // possibly needs isMoving check
        transform.position = pos;
        ReassignCells();
    }

    public void SnapToClosestCell()
    {
        StartCoroutine(Moving(closestCellPosition));
    }

    //actual movement coroutine
    IEnumerator Moving(Vector3 destination){
        isMoving = true;
        while (isMoving){
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            if (transform.position == destination) {
                isMoving = false;
            }
            yield return null;
        }
    }


    //switch that stops
    public void StopMoving()                            
    {
        isMoving = false;
    }

    public void SelfDestroy()
    {
        foreach (var item in elements)
        {
            item.myCell.IsEmpty = true;
            item.myCell.Element = null;
        }

        Game.AllDestruction -= SelfDestroy;

        Destroy(gameObject);
    }

    public void Shrink()
    {

    }

    public void BreakUp()
    {
        
    }

    // NEW METHODS BELOW

    private int GetFreeLinesCount(Directions direction)
    {
        currentElementsGroup = GetElementsByDirection(direction);
        int freeLinesCount = -1;
        int currentLine = 0;
        bool isLineFilled = false;
        do
        {
            currentLine++;
            freeLinesCount++;
            for (int i = 0; i < currentElementsGroup.Length; i++)
            {
                if (!(currentElementsGroup[i].GetNeighbourCell(direction, currentLine)?.IsEmpty ?? false))
                {
                    isLineFilled = true;
                    break;
                }
            }

            if (freeLinesCount > 16)
            {
                throw new System.ApplicationException("endless loop");               
            }
        }
        while (!isLineFilled);

        return freeLinesCount;
    }

    private Element[] GetElementsByDirection(Directions direction)
    {
        switch(direction)
        {
            case Directions.Down:
                return dElements;
            case Directions.Left:
                return lElements;
            case Directions.Right:
                return rElements;
            case Directions.Up:
                return uElements;
            default:
                return null;
        }
    }

    private void CalculateHorizontalConstraints()
    {
        int freeLeftLinesCount = GetFreeLinesCount(Directions.Left);
        int freeRightLinesCount = GetFreeLinesCount(Directions.Right);

        minXposition = closestCellPosition.x - Game.Instance.CombinedGrid.step * freeLeftLinesCount;
        maxXposition = closestCellPosition.x + Game.Instance.CombinedGrid.step * freeRightLinesCount;
    }

    private void CalculateVerticalConstraints()
    {
        int freeUpLinesCount = GetFreeLinesCount(Directions.Up);
        int freeDownLinesCount = GetFreeLinesCount(Directions.Down);

        minYposition = closestCellPosition.y - Game.Instance.CombinedGrid.step * freeDownLinesCount;
        maxYposition = closestCellPosition.y + Game.Instance.CombinedGrid.step * freeUpLinesCount;        
    }

    private void ReassignCells()
    {
        closestCellPosition = Game.Instance.CombinedGrid.GetClosestCellWorldPosition(transform.position);
        foreach (var item in elements)
        {
            item.ReassignCell();
        }
    }     
    
    // якщо в блоці хоча б один елемент лишається, то не записуємо його у список на видалення
}
