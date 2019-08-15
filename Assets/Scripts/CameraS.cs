using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraS : MonoBehaviour
{
    public float defaultSpeed = 0.5f;
    float stopSpeed = 0;
    float speed;

    int currentLevel = 1;
    int gridHeight;
    bool camCheat = false;
    Vector2 origin;

    public delegate void CameraEvents();
    public static event CameraEvents CreateLevel;

    private void Start()
    {
        speed = defaultSpeed;
        gridHeight = Game.Instance.CombinedGrid.halfHeight;
        origin = Game.Instance.CombinedGrid.origin;
    }

    private void Update()
    {
        transform.Translate(0f, speed * Time.deltaTime, 0f);
        if (transform.position.y >= currentLevel * gridHeight)
        {
            CreateLevel();
            currentLevel++;
        }
    }

    public void ResetCamera()
    {
        currentLevel = 1;
        transform.position = new Vector3(0f, 0f, -10f);
    }

    public void StopSpeed() {
        if (!camCheat)
        {
            speed = stopSpeed;
            camCheat = true;
        }
        else {
            speed = defaultSpeed;
            camCheat = false;
        }
    }
}
