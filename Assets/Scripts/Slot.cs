using UnityEngine.UI;

[System.Serializable]
public class Slot
{
    public bool isUnlocked;
    public IBooster boosterType;
    public int boostersCount;
    public int maxBoosters;
    public Text slotText; // TEMPORARILY BEFORE ACTUAL UI PREFAB

    public void UpdateSlotText()
    {
        if (boosterType != null)
        {
            slotText.text = boosterType.BoosterName + " (" + boostersCount + ")";
        }
        else
        {
            slotText.text = "_";
        }
    }
}
