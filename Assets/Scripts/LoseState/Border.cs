using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(1);
    }

    void PlayerDeath() {
        if (!CheatMode && markedForDeath) {
            GameOver();
        }
    }
}
