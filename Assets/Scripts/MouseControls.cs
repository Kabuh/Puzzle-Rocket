using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour
{
    public float minSwipeDistance;

    public GameObject canvas;
    Block block;

    private Vector3 startTouchPos;
    private Vector3 endTouchPos;

    bool isBlockReadyToBeMoved;
    bool blockShouldStay;

    float swipeAngle;

    private LayerMask nonBoosterMask = 513; // ПОКИ ЩО НЕ ІСНУЄ ІНШИХ

    private void Start()
    {
        Game.LevelSpawnFinished += RecalculateBlock;
        Game.ExplosionEnded += RecalculateBlock;
        Game.PlayerDead += DiscardBlock;
    }

    private void LateUpdate()
    {
        //initiation of swipe
        if (Input.GetMouseButtonDown(0)) 
        {
            OnTouchStart(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        //duration of swipe
        if (Input.GetMouseButton(0)) 
        {
            OnTouchContinue(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        //end of swipe
        if (Input.GetMouseButtonUp(0) ) 
        {
            OnTouchEnd(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        //cheat enable
        if (Input.GetKey("t"))
        {
            if (Input.GetKey("g"))
            {
                canvas.SetActive(true);
            }
        }
    }   

    //pick block from level
    private Block TryTouchBlock(Vector3 pos)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity, nonBoosterMask);
        if(raycastHit2D)
        {
            return raycastHit2D.collider.GetComponent<Block>();
        }
        return null;
    }

    private void OnTouchStart(Vector3 input)
    {
        block = TryTouchBlock(input);
        startTouchPos = input;
        block?.CalculateInputAndPivotDiff(startTouchPos);
    }

    private void OnTouchContinue(Vector3 input)
    {
        if(block!=null)
        {
            if (isBlockReadyToBeMoved && !blockShouldStay)
            {
                // then move (along axis) with input and update cells
                block.UpdatePosition(input);
            }
            else
            {
                float swipeDistance = Vector3.Distance(input, startTouchPos);
                if (swipeDistance >= minSwipeDistance)
                {
                    // get block ready to be moved
                    Axis axis = CalculateAxis(startTouchPos, input);
                    block.SetAxis(axis);
                    block.CalculateMovementConstraints();
                    isBlockReadyToBeMoved = true;
                    blockShouldStay = false;
                }
                else
                {
                    blockShouldStay = true;
                }
            }
        }        
    }

    private void OnTouchEnd(Vector3 input)
    {
        // trigger snappping
        if (block != null)
        {
            if (!blockShouldStay)
            {
                block.UpdatePosition(input);
                block.SnapToClosestCell();
            }
            // loose block
            block = null;
            isBlockReadyToBeMoved = false;
        }        
    }

    private Axis CalculateAxis(Vector3 start, Vector3 end)
    {
        Vector3 diff = end - start;

        float angle = Vector3.SignedAngle(new Vector3(1f, 1f, 0f), diff, Vector3.forward);

        if (angle >= 0)
        {
            if (angle >= 90)
            {
                return Axis.Horizontal;
            }
            else
            {
                return Axis.Vertical;
            }
        }
        else
        {
            if (angle < -90)
            {
                return Axis.Vertical;
            }
            else
            {
                return Axis.Horizontal;
            }
        }
    }

    private void DiscardBlock()
    {
        block = null;
    }

    private void RecalculateBlock()
    {
        block?.CalculateMovementConstraints();
    }

}
