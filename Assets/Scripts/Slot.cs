using UnityEngine.UI;

[System.Serializable]
public class Slot
{
    public int index;
    public bool isUnlocked;
    public BoosterType boosterType;
    public int boostersCount;
    public int maxBoosters;
    public Text slotText; // TEMPORARILY BEFORE ACTUAL UI PREFAB

    public void UpdateSlotText()
    {
        if (boosterType != null)
        {
            //slotText.text = boosterType.BoosterName + " (" + boostersCount + ")";
            slotText.text = "(" + boostersCount + ")";
        }        
        else
        {
            slotText.text = "_";
        }
    }
}
