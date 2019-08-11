using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float moveSpeed;    
    [Header("Gameplay parameters")]
    public bool immovable;
    [Header("Elements")]
    public Element[] elements;
    //public Element[] uElements;
    //public Element[] lElements;
    //public Element[] rElements;
    //public Element[] dElements;

    private Vector3 destination;

    private bool isMoving;

    private float clampUP;
    private float clampDOWN;
    private float clampLEFT;
    private float clampRIGHT;
    Vector3 ClampedVector;
    Vector2 currentLocation;

    bool setInCell = true;

    private void Awake()
    {
        foreach (var item in elements) {
            item.myBlock = this;
        }
    }

    public void SetInstantLocation(Vector3 cursorLoc, Directions dir) {
        CleanOriginalElements(elements);
        DoCheck(dir);
        setInCell = false;
        ClampedVector = new Vector3(Mathf.Clamp(cursorLoc.x, clampLEFT, clampRIGHT), Mathf.Clamp(cursorLoc.y, clampDOWN, clampUP), cursorLoc.z);
        transform.position = ClampedVector; 
    }

    public void SetDestinationLocation()
    {
        Vector2 originVect = Game.Instance.CombinedGrid.origin;
        float cellDistances = Game.Instance.CombinedGrid.step;
        currentLocation = transform.position;


        int lowestAmountOfIndexX = (int)((currentLocation.x - originVect.x) / cellDistances);
        float leftoverX = (currentLocation.x - originVect.x) - (cellDistances*lowestAmountOfIndexX);

        if (leftoverX < cellDistances / 2)
        {
            destination.x = originVect.x + (lowestAmountOfIndexX * cellDistances);
        }
        else
        {
            destination.x = originVect.x + (++lowestAmountOfIndexX * cellDistances);
        }

        int lowestAmountOfIndexY = (int)((currentLocation.y - originVect.y) / cellDistances);
        float leftoverY = (currentLocation.y - originVect.y) - (cellDistances * lowestAmountOfIndexY);

        if (leftoverY < cellDistances / 2)
        {
            destination.y = originVect.y + (lowestAmountOfIndexY * cellDistances);
        }
        else
        {
            destination.y = originVect.y + (++lowestAmountOfIndexY * cellDistances);
        }
        StartCoroutine(Moving(destination));
        SetNewElementsLocations(elements, destination);
        foreach (Cell cellitem in Game.Instance.CombinedGrid.cells)
        {
            Debug.Log(cellitem.XPos + " " + cellitem.YPos + " " + cellitem.IsEmpty);
        }
    }


    public void ResetElementsCells()
    {
        foreach (var item in elements)
        {
            item.ChangeCellLevel();
        }
    }

    private void DoCheck(Directions dir) {
        if (setInCell) {
            MovementAllowance(dir);
        }
    }


    public void MovementAllowance(Directions dir) {
        if (immovable) {                        // check if moving is possible
            clampUP = transform.position.y;
            clampDOWN = transform.position.y;
            clampLEFT = transform.position.x;
            clampRIGHT = transform.position.x;
            return;
        }
        switch (dir) {
            case Directions.Horizontal:
                clampUP = transform.position.y;
                clampDOWN = transform.position.y;
                if (!NeighboursAreEmpty(elements, Directions.Left)) {
                    clampLEFT = transform.position.x;
                }else{
                    clampLEFT = transform.position.x - Game.Instance.CombinedGrid.step;
                }
                if (!NeighboursAreEmpty(elements, Directions.Right)) {
                    clampRIGHT = transform.position.x;
                }else{
                    clampRIGHT = transform.position.x + Game.Instance.CombinedGrid.step;
                }
                break;
                

            case Directions.Vertical:
                clampLEFT = transform.position.x;
                clampRIGHT = transform.position.x;
                if (!NeighboursAreEmpty(elements, Directions.Up)) {
                    clampUP = transform.position.y;
                }
                else{
                    clampUP = transform.position.y + Game.Instance.CombinedGrid.step;
                }
                if (!NeighboursAreEmpty(elements, Directions.Down)) {
                    clampDOWN = transform.position.y;
                }
                else{
                    clampDOWN = transform.position.y - Game.Instance.CombinedGrid.step;
                }
                break;
        }
    }
    /*
    public void Move(Directions dir){                        //setting movement direction
    
        if (immovable)
            return;        
        // check if moving is possible
        switch (dir)
        {
            case Directions.Up:
                if (!NeighboursAreEmpty(uElements, dir))
                    return;
                destination = transform.position + new Vector3(0f, Game.Instance.CombinedGrid.step, 0f);
                ChangeElements(dElements, uElements,dir);
                break;
            case Directions.Down:
                if (!NeighboursAreEmpty(dElements, dir))
                    return;
                destination = transform.position - new Vector3(0f, Game.Instance.CombinedGrid.step, 0f);
                ChangeElements(uElements, dElements,dir);
                break;
            case Directions.Left:
                if (!NeighboursAreEmpty(lElements, dir))
                    return;
                destination = transform.position - new Vector3(Game.Instance.CombinedGrid.step,0f, 0f);
                ChangeElements(rElements, lElements,dir);
                break;
            case Directions.Right:
                if (!NeighboursAreEmpty(rElements, dir))
                    return;
                destination = transform.position + new Vector3(Game.Instance.CombinedGrid.step, 0f, 0f);
                ChangeElements(lElements, rElements,dir);
                break;
        }
        
    }
    */


    //actual movement coroutine
    IEnumerator Moving(Vector3 destination){
        isMoving = true;
        while (isMoving){
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            if (transform.position == destination) {
                setInCell = true;
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

    //empty checker
    private bool NeighboursAreEmpty(Element[] elements, Directions dir)         
    {
        for(int i=0;i<elements.Length;i++)
        {
            if (!(elements[i].GetNeighbourCell(dir)?.IsEmpty??false))
            {
                return false;
            }
        }
        return true;
    }

    //clear cell info on departure
    private void CleanOriginalElements(Element[] elem) {
        foreach (var item in elem)
        {
            item.myCell.IsEmpty = true;
        }
    }

    //rewrite cell info on arrival
    private void SetNewElementsLocations(Element[] NewElem, Vector2 worldPos) {
        foreach (var item in NewElem)
        {
            Cell cell = item.WorldPosToCell(worldPos);
            if (cell != null)
            {
                cell.IsEmpty = false;
                item.ShiftCellByPos(worldPos);
            }
        }
    }

    private void ChangeElements(Element[] elementsFree, Element[] elementsFill, Vector2 worldPos)         //rewritre of cell fill info
    {   
        foreach(var item in elementsFree)
        {
            item.myCell.IsEmpty = true;
        }

        foreach(var item in elementsFill)
        {
            Cell cell = item.WorldPosToCell(worldPos);
            if (cell != null) {
                cell.IsEmpty = false;
            }
        }

        foreach (var item in elements)
        {
            item.ShiftCellByPos(worldPos);
        }
    }


    public bool ShouldBeDestroyed()
    {
        foreach(Element e in elements)
        {
            if (e.myCell.YPos <= 7) {
                return true;
            } 
        }
        return false;
    }

    public void SelfDestroy() {
        foreach (var item in elements) {
            item.myCell.IsEmpty = true;
        }
        Destroy(this.gameObject);
    }

    // якщо в блоці хоча б один елемент лишається, то не записуємо його у список на видалення
}
