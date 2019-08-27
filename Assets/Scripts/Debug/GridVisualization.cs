using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualization : MonoBehaviour
{
    public GameObject firstMarker;

    public float step;

    GridClass grid;

    GameObject[,] markers;

    private void Start()
    {
        grid = Game.Instance.CombinedGrid;

        markers = new GameObject[grid.width,grid.height];

        markers[0,0] = firstMarker;

        RectTransform fmrt = firstMarker.GetComponent<RectTransform>();

        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                GameObject nextMarker = Instantiate(firstMarker, this.transform);
                nextMarker.GetComponent<RectTransform>().localPosition = new Vector3(fmrt.localPosition.x + i *step, fmrt.localPosition.y + j*step);
                markers[i, j] = nextMarker;
            }            
        }
    }

    private void Update()
    {
        for (int i = 0; i < markers.GetLength(0); i++)
        {
            for (int j = 0; j < markers.GetLength(1); j++)
            {
                if (grid.cells[i, j].IsEmpty)
                    markers[i, j].SetActive(false);
                else
                    markers[i, j].SetActive(true);
            }
        }
    }
}
