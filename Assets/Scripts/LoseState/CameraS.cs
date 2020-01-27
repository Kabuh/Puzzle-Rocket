﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraS : MonoBehaviour
{
    public static CameraS Instance { get; private set; }

    private void Awake()
    {
        #region Singleton

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        #endregion
    }

    public float defaultSpeed = 0.5f;
    float stopSpeed = 0;
    public float speed;
    public int cameraStartCellIndex = 4;

    int currentLevel = 1;
    int gridHeight;
    bool camCheat = false;
    Vector2 origin;
    
    private void Start()
    {
        speed = defaultSpeed;
        gridHeight = Game.Instance.CombinedGrid.height;
        origin = Game.Instance.CombinedGrid.origin;
        ResetPosition();
    }

    private void Update()
    {
        if (transform.position.y <= origin.y + Game.Instance.CombinedGrid.step * (gridHeight-5))
        {
            transform.Translate(0f, speed * Time.deltaTime, 0f);
        }
        //if (transform.position.y >= currentLevel * gridHeight)
        //{
        //    CreateLevel();
        //    currentLevel++;
        //}
    }

    public void ResetCamera()
    {
        currentLevel = 1;
        ResetPosition();
    }

    private void ResetPosition()
    {
        transform.position = new Vector3(0f, origin.y + Game.Instance.CombinedGrid.step * cameraStartCellIndex, -10f);
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

    public void TimeStopBooster(float pause) {
        StartCoroutine(TemporaryTimeStop(pause));
    }

    IEnumerator TemporaryTimeStop(float pause) {
        speed = stopSpeed;
        yield return new WaitForSeconds(pause);
        speed = defaultSpeed;
    }
}
