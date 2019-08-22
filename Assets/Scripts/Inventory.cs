using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Inventory Instance { get; private set; }

    [SerializeField] private int slotsCount = 3;

    [SerializeField] private Text[] slotTexts;

    private Slot[] slots;

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

        slots = new Slot[slotsCount];        
    }


    // TEMP!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnSlotTouch(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnSlotTouch(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnSlotTouch(2);
        }
    }

    private void OnSlotTouch(int slotIndex)
    {   
        if(slots[slotIndex].isUnlocked)
        {
            if(slots[slotIndex].boosterType!=null)
            {
                slots[slotIndex].boosterType.Activate();
            }
        }
    }

    public void UnlockSlot(int slotIndex)
    {
        slots[slotIndex].isUnlocked = true;
        // visual changes
        slotTexts[slotIndex].text = "_";
    }

    public void TryAddBooster(IBooster booster)
    {
        Slot emptySlot = null;

        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].isUnlocked)
            {
                if (slots[i].boosterType == booster)
                {
                    AddToSlot(slots[i]);
                    return;
                }

                if (emptySlot==null && slots[i].boosterType==null)
                {
                    emptySlot = slots[i];
                }                
            }
        }

        if (emptySlot != null)
        {
            FillSlot(emptySlot, booster);
        }
        else
        {
            booster.Activate();
        }
    }

    public void UseBooster(int slotIndex)
    {
        Slot slot = slots[slotIndex];

        if (slot.isUnlocked)
        {
            if (slot.boosterType != null)
            {
                slot.boosterType.Activate();
                slot.boostersCount--;
                if (slot.boostersCount == 0)
                {
                    ClearSlot(slot);
                }
            }
        }
    }    

    private void FillSlot(Slot slot, IBooster booster)
    {
        slot.boosterType = booster;
        slot.boostersCount++;
    }

    private void AddToSlot(Slot slot)
    {
        slot.boostersCount++;        
        // + visual changes or fire event listened by ui
    }

    private void ClearSlot(Slot slot)
    {
        slot.boosterType = null;
        // + visual changes or fire event listened by ui
    }
}
