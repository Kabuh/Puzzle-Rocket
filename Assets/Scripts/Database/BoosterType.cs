using UnityEngine;

public abstract class BoosterType : ScriptableObject
{
    public string boosterName;
    public float spawnChance;
    public GameObject boosterObject;
    public int maxInInventory;  

    public abstract void Activate(Cell cell);
}
