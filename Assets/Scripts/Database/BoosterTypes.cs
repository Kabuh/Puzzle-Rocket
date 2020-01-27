using UnityEngine;

[System.Serializable]
public class BoosterTypes
{
    public string name;
    public float SpawnChance;
    public GameObject blockObject;
    public int maxInInventory;

    public BoosterTypes(string nm, float spwnChns, GameObject bstrObj, int max)
    {
        name = nm;
        SpawnChance = spwnChns;
        blockObject = bstrObj;
        maxInInventory = max;
    }
}
