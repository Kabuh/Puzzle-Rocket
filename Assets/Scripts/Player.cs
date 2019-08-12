using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Block playerBlock;


    private void Start()                        //starship block
    {
        playerBlock = GetComponent<Block>();
    }

    public void ReassignPlayerElement()         //reassign player to another ship
    {
        playerBlock.ResetElementsCells();
    }

    public void ResetPlayer()                   //set player block to start location
    {
        playerBlock.StopMoving();
        transform.position = new Vector3(0f, -0.5f, 0f);
        playerBlock.elements[0].SetCell();
    }    
}
