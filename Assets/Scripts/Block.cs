using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
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

    private bool isMoving;
    

    public void ResetElementsCells()
    {
        foreach (var item in elements)
        {
            item.ChangeCellLevel();
        }
    }

    public void Move(Directions dir)                        //setting movement direction
    {
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
        StartCoroutine(Moving(destination));
    }

    IEnumerator Moving(Vector3 destination)             //actual movement coroutine
    {
        isMoving = true;
        while (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            if (transform.position == destination)
                isMoving = false;
            yield return null;
        }
    }

    public void StopMoving()                            //switch that stops
    {
        isMoving = false;
    }

    private bool NeighboursAreEmpty(Element[] elements, Directions dir)         //empty checker
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

    private void ChangeElements(Element[] elementsFree, Element[] elementsFill, Directions dir)         //
    {   
        foreach(var item in elementsFree)
        {
            item.Cell.IsEmpty = true;
        }

        foreach(var item in elementsFill)
        {
            Cell cell = item.GetNeighbourCell(dir);
            if (cell != null)
                cell.IsEmpty = false;
        }

        foreach (var item in elements)
        {
            item.ShiftCell(dir);
        }
    }

    public bool ShouldBeDestroyed()
    {
        foreach(Element e in elements)
        {
            if (e.Cell.YPos <= 7)
                return true;
        }
        return false;
    }

    // якщо в блоці хоча б один елемент лишається, то не записуємо його у список на видалення
}
