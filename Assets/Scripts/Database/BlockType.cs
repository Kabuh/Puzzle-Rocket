using UnityEngine;

[System.Serializable]
public class BlockType
{
    public string name;
    public int height;
    public int width;
    public float SpawnChance;
    public bool isVertical;
    public GameObject blockObject;

    public BlockType(string n, int h, int w, float sc, bool isV, GameObject bO)
    {
        name = n;
        height = h;
        width = w;
        SpawnChance = sc;
        isVertical = isV;
        blockObject = bO;
    }
}
