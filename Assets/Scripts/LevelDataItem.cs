﻿[System.Serializable]
public class LevelDataItem
{
    public string prefabName;
    public float xPos;
    public float yPos;
    public string type;

    public LevelDataItem(string prefabName, float xPos, float yPos, string type)     //item properties(now only coordinates and name)
    {
        this.prefabName = prefabName;
        this.xPos = xPos;
        this.yPos = yPos;
        this.type = type;
    }
}
