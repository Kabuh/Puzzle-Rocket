using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [SerializeField] private int slotsCount = 4;

    [SerializeField] private int[] slotCosts = null;

    [SerializeField] private Text[] slotTexts = null;

    private Slot[] slots;

    private Slot nextSlotToUnlock;

    private int coinsCount;

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
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new Slot();
            slots[i].index = i;
            slots[i].slotText = slotTexts[i];
            slots[i].coinsToUnlock = slotCosts[i];
            nextSlotToUnlock = slots[1];
            slots[i].UpdateSlotText();
        }

        // FIRST SLOT IS UNLOCKED FROM THE START
        slots[0].isUnlocked = true;        
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

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnSlotTouch(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AddCoins(1);
        }
    }

    // make available only if player is standing
    private void OnSlotTouch(int slotIndex)
    {   
        if(slots[slotIndex].isUnlocked)
        {
            if(slots[slotIndex].boosterType!=null)
            {
                UseBooster(slotIndex);

                //slots[slotIndex].boosterType.Activate(playerBlock.elements[0].myCell);
            }
        }
        else
        {
            UnlockSlot(slots[slotIndex]);
        }
    }

    public void UnlockSlot(Slot slot)
    {
        //if (coinsCount >= slotCosts[slotIndex])
        //{
        //    slots[slotIndex].isUnlocked = true;
        //    coinsCount -= slotCosts[slotIndex];
        //    // visual changes
        //    slots[slotIndex].UpdateSlotText();
        //}

        slot.isUnlocked = true;        
        // visual changes
        slot.UpdateSlotText();
    }

    public void AddCoins(int amount)
    {
        //coinsCount += amount;
        nextSlotToUnlock.coinsToUnlock -= amount;
        nextSlotToUnlock.UpdateSlotText();
        if(nextSlotToUnlock.coinsToUnlock<=0)
        {
            UnlockSlot(nextSlotToUnlock);
            if (nextSlotToUnlock.index < slots.Length - 1)
            {
                nextSlotToUnlock = slots[nextSlotToUnlock.index + 1];
            }
        }
    }

    public void TryAddBooster(Booster booster, Cell boosterCell)
    {
        Slot emptySlot = null;

        /*
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].isUnlocked)
            {
                if (slots[i].boosterType == booster)
                {
                    if(slots[i].boostersCount==slots[i].maxBoosters)
                    {
                        booster.Activate(boosterCell);
                    }
                    else
                    {
                        AddToSlot(slots[i]);
                    }
                    return;
                }

                if (emptySlot==null && slots[i].boosterType==null)
                {
                    emptySlot = slots[i];
                }                
            }
        }
        */

        if (emptySlot != null)
        {
            FillSlot(emptySlot, booster);
        }
        else
        {
            booster.Activate(boosterCell);
        }
    }

    public void UseBooster(int slotIndex)
    {
        Slot slot = slots[slotIndex];

        if (slot.isUnlocked)
        {
            if (slot.boosterType != null)
            {
                slot.boosterType.Activate(Game.Instance.Player.playerBlock.elements[0].myCell);
                slot.boostersCount--;
                slot.UpdateSlotText();
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
        slot.maxBoosters = booster.MaxInInventory;

        slot.UpdateSlotText();
    }

    private void AddToSlot(Slot slot)
    {        
        slot.boostersCount++;
        // + visual changes or fire event listened by ui
        slot.UpdateSlotText();        
    }

    private void ClearSlot(Slot slot)
    {
        slot.boosterType = null;
        // + visual changes or fire event listened by ui
        slot.UpdateSlotText();
    }
}
