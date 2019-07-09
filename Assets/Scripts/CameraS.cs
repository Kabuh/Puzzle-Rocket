using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraS : MonoBehaviour
{
    public float speed = 0.5f;
    int currentLevel = 1;

    private void Update()
    {
        transform.Translate(0f, speed*Time.deltaTime, 0f);
        if(transform.position.y>=currentLevel*8f)
        {
            Game.Instance.SpawnNewLevel(++currentLevel);
        }
    }

    public void ResetCamera()
    {
        currentLevel = 1;
        transform.position = new Vector3(0f,0f,-10f);
    }    
}
