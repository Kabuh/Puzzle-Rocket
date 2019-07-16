using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour
{
    public float minSwipeDistance;

    public GameObject canvas;
    Block gameCube;

    bool GlueAtivate = false;
    Vector3 CursorLoc;


    Vector3 startTouchPos;
    Vector3 currenTouchPos;
    Vector3 endTouchPos;

    float swipeAngle;
    Directions swipeDir;

    private void LateUpdate()
    {
        //initiation of swipe
        if (Input.GetMouseButtonDown(0)) 
        {
            startTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            gameCube = TryTouchBlock(startTouchPos);

            GlueAtivate = true;

        }

        //duration of swipe
        if (Input.GetMouseButton(0)) 
        {
            currenTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        //end of swipe
        if (Input.GetMouseButtonUp(0) ) 
        {
            GlueAtivate = false;
            endTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameCube?.SetDestinationLocation();
        }

        //cheat enable
        if (Input.GetKey("t"))
        {
            if (Input.GetKey("g"))
            {
                canvas.SetActive(true);
            }
        }

        

        //do this each frame if glue active
        if (gameCube != null && MinSwipeReached() && GlueAtivate) {

            TouchGlue();
        }
    }

    bool MinSwipeReached() {
        if (Vector3.Distance(currenTouchPos, startTouchPos) >= minSwipeDistance)
        {
            RealSwipeDir();
            return true;
        }
        return false;
    }

    void TouchGlue()
    {
        CursorLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CursorLoc.z = 0;
        if (CursorLoc != null && gameCube != null)
        {
            gameCube.SetInstantLocation(CursorLoc, swipeDir);
        }
    }

    //getting swipe dirrection
    void RealSwipeDir() {
        {
            Vector3 diff = currenTouchPos - startTouchPos;

            swipeAngle = Vector3.SignedAngle(new Vector3(1f, 1f, 0f), diff, Vector3.forward);       // direction check
            

            if (swipeAngle >= 0)
            {                                                                  // half-check
                if (swipeAngle >= 90)
                {
                    swipeDir = Directions.Horizontal;
                }
                else
                {
                    swipeDir = Directions.Vertical;
                }
            }
            else
            {
                if (swipeAngle <= -90)
                {
                    swipeDir = Directions.Vertical;
                }
                else
                {
                    swipeDir = Directions.Horizontal;
                }
            }
        }



        /*
        void OnSwipe()
        {
            Vector3 diff = endTouchPos - startTouchPos;

            swipeAngle = Vector3.SignedAngle(new Vector3(1f,1f,0f),diff,Vector3.forward);       // direction check

            if (swipeAngle>=0){                                                                  // half-check
                if(swipeAngle>=90){                                                             // quarter-checks
                    TryTouchBlock(startTouchPos)?.Move(Directions.Left); 
                }else{
                    TryTouchBlock(startTouchPos)?.Move(Directions.Up);
                }
            }else{
                if(swipeAngle<-90){
                    TryTouchBlock(startTouchPos)?.Move(Directions.Down);
                }else{
                    TryTouchBlock(startTouchPos)?.Move(Directions.Right);
                }
            }
        }
            */
    }

    //pick block from level
    public Block TryTouchBlock(Vector3 pos)                                   
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(pos, Vector2.zero);
        if(raycastHit2D)
        {
            return raycastHit2D.collider.GetComponent<Block>();
        }
        return null;
    }
}
