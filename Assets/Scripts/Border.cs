using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    bool CheatMode = false;

    // розмістити на окремому шарі, щоб дарма колізії зі звичайними блоками не прораховувались
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (!CheatMode)
            {
                Game.Instance.GameOver();
            }

        }
    }

    public void CheatButtonActivate (){
        CheatMode = true;
        Debug.Log("cheat mode activated");
    }
    
}
