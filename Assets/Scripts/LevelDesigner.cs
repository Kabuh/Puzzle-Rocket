using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDesigner : MonoBehaviour
{
    #region Singleton
    public static LevelDesigner Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    /*
    int levelWidth = 5;
    int levelHeight = 8;
    float gridStep = 1;

    public GameObject tile;
    public GameObject background;
    public GameObject playerPrefab;
    [Header("Block Variants")]
    public GameObject[] blocks;
    [Header("UI Eelements")]
    public Image chosenBlockSprite;

    List<GameObject> blocksToSave;
    GameObject gridObject;

    GameObject chosenBlock;
    GameObject objectToPlace;

    public mGrid Grid { get; set; }
    //public Tile SelectedTile { get; set; }
    public DesignerItem SelectedItem { get; set; }


    //private void Awake()
    //{
    //    blocksToSave = new List<GameObject>();
    //}

    void Start()
    {
        chosenBlockSprite.preserveAspect = true;
    }

    public void CreateNewGrid()
    {
        if (Grid != null)
        {        
            DeleteGrid();
        }
        Grid = new mGrid(levelWidth, levelHeight, gridStep, true);
        DrawGrid();
    }

    public void SetWidth(string width)
    {
        int value = int.Parse(width);
        if (value > 0)
            levelWidth = value;
    }

    public void SetHeight(string height)
    {
        int value = int.Parse(height);
        if (value > 0)
            levelHeight = value;
    }

    public void SetGridStep(string step)
    {
        float value = float.Parse(step);
        if (value > 0)
            gridStep = value;
    }


    public void DeleteGrid()
    { 
        Destroy(gridObject);         
    }  
    
    void DrawGrid()
    {
        gridObject = new GameObject("Grid");
        gridObject.layer = 8;
        for (int i = 0; i < Grid.height; i++)
        {
            for (int j = 0; j < Grid.width; j++)
            {                
                GameObject tileI = Instantiate(tile, new Vector3(Grid.origin.x + Grid.step * j, Grid.origin.y + Grid.step * i, 0f), Quaternion.identity,gridObject.transform);
                if((j&1)==1 && (i&1)!=1 || (j&1)!=1 && (i&1)==1)
                {
                    tileI.GetComponentInChildren<SpriteRenderer>().color = new Color(1f,1f,1f,0.5f);
                }
                tileI.transform.localScale = new Vector3(Grid.step, Grid.step, 1f);
                //tileI.GetComponentInChildren<Tile>().designer = this;
            }
        }
        //PlacePlayer();
    }

    public void ChooseBlock(int blockIndex)
    {
        if(blockIndex==-1)
        {
            Destroy(objectToPlace);
            chosenBlock = null;
            chosenBlockSprite.sprite = null;
        }
        else
        {            
            if(blocks[blockIndex]==chosenBlock)
            {
                return;
            }
            if (chosenBlock != null)
            {
                Destroy(objectToPlace);
            }
            chosenBlock = blocks[blockIndex];
            chosenBlockSprite.sprite = blocks[blockIndex].GetComponentInChildren<SpriteRenderer>().sprite;
            objectToPlace = Instantiate(chosenBlock);
            objectToPlace.layer = 2;
        }
        
    }

    private void PlaceBlock()
    {
        //GameObject block = Instantiate(chosenBlock, SelectedTile.transform.position, Quaternion.identity);
        Element[] elements = block.GetComponentsInChildren<Element>();
        List<Vector3> positions = new List<Vector3>();
        foreach (var item in elements)
        {
            positions.Add(item.gameObject.transform.position);
        }
        if (IsEmpty(positions.ToArray()))
        {
            FillCells(positions.ToArray());
            DesignerItem item = block.AddComponent<DesignerItem>();
            item.background = background;
        }
        else
        {
            Destroy(block);
        }
    }

    public void PlacePlayer()
    {
        Vector3 playerPlace = new Vector3(Grid.origin.x + Grid.step * (Grid.width / 2), Grid.origin.y + Grid.step*(Grid.height/2), 0f);
        Instantiate(playerPrefab,playerPlace,Quaternion.identity);
    }

    private void Update()
    {
        if(chosenBlock!=null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            objectToPlace.transform.position = mousePos;
            if (Input.GetMouseButtonDown(1))
            {
                ChooseBlock(-1);
            }
            if(Input.GetMouseButtonDown(0))
            {
                if(SelectedTile!=null)
                {
                    PlaceBlock();
                }
            }
        }
        else
        {
            //if (Input.GetMouseButtonDown(1))
            //{
               
            //}
            //if (Input.GetMouseButtonDown(0))
            //{
            //    SelectItem();
            //}
        }
    }

    private bool IsEmpty(Vector3[] elementsPositions)
    {
        foreach (var item in elementsPositions)
        {
            try
            {
                if (!Grid.WorldPosToCell(item).IsEmpty)
                {
                    return false;
                }
            }
            catch(System.IndexOutOfRangeException)
            {
                return false;
            }
        }        
        return true;
    }

    private void FillCells(Vector3[] elementsPositions)
    {
        foreach (var item in elementsPositions)
        {
            Grid.WorldPosToCell(item).IsEmpty = false;
        }
    }

    //private void SelectItem()
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    //    if (hit.collider != null)
    //    {
    //        Debug.Log("hit something");
    //        DesignerItem designerItem = hit.collider.gameObject.GetComponent<DesignerItem>();
    //        if (designerItem != null)
    //        {
    //            designerItem.Selected = true;
    //        }
    //    }
    //}
    */
}
