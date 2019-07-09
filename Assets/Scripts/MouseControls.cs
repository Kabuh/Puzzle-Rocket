using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour
{
    public float minSwipeDistance;

    public GameObject canvas;

    Vector3 startTouchPos;
    Vector3 endTouchPos;

    float swipeAngle = 0.2f;

    private void LateUpdate()
    {
        if(Input.GetMouseButtonDown(0)) //start of swipe
        {
            startTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if(Input.GetMouseButtonUp(0)) //end of swipe
        {
            endTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector3.Distance(endTouchPos, startTouchPos) >= minSwipeDistance)
                OnSwipe();
        }

        if (Input.GetKey("t")) {
            if (Input.GetKey("g"))
                {
                    canvas.SetActive(true);
                }
            }
    }


    void OnSwipe()
    {
        Vector3 diff = endTouchPos - startTouchPos;

        swipeAngle = Vector3.SignedAngle(new Vector3(1f,1f,0f),diff,Vector3.forward); // direction check

        if (swipeAngle>=0) // half-check
        {
            if(swipeAngle>=90)
            {
                TryTouchBlock(startTouchPos)?.Move(Directions.Left); // quarter-checks
            }
            else
            {
                TryTouchBlock(startTouchPos)?.Move(Directions.Up);
            }
        }
        else
        {
            if(swipeAngle<-90)
            {
                TryTouchBlock(startTouchPos)?.Move(Directions.Down);
            }
            else
            {
                TryTouchBlock(startTouchPos)?.Move(Directions.Right);
            }
        }

    }

    Block TryTouchBlock(Vector3 pos) //send info about collision
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(startTouchPos, Vector2.zero);
        if(raycastHit2D)
        {
            return raycastHit2D.collider.GetComponent<Block>();
        }
        return null;
    }
}
