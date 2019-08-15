using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    bool CheatMode = false;
    bool markedForDeath= false;

    // розмістити на окремому шарі, щоб дарма колізії зі звичайними блоками не прораховувались

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.GetComponent<Player>() != null)
        {
            Debug.Log("collision met");
            markedForDeath = true;
            PlayerDeath();
        }
    }

    public void CheatButtonActivate (){
        if (!CheatMode) {
            CheatMode = true;
        }
        else{
            CheatMode = false;
            PlayerDeath();
        }
        
    }

    public void GameOver() {
        Game.Instance.GameOver();
    }

    void PlayerDeath() {
        if (!CheatMode && markedForDeath) {
            GameOver();
        }
    }
}
