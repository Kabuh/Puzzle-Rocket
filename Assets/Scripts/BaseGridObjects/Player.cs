using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Block playerBlock;

    private void Awake()                        //starship block
    {
        playerBlock = GetComponent<Block>();
        playerBlock.IsPlayer = true;
    }    
}
