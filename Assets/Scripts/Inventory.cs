﻿using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [SerializeField] private int slotsCount = 3;

    [SerializeField] private int[] slotCosts;

    [SerializeField] private Text[] slotTexts;

    private Slot[] slots;

    private int coinsCount;

    private Block playerBlock;

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
        }
    }

    private void Start()
    {
        playerBlock = Game.Instance.player.GetComponent<Block>();
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
            UnlockSlot(slotIndex);
        }
    }

    public void UnlockSlot(int slotIndex)
    {
        if (coinsCount >= slotCosts[slotIndex])
        {
            slots[slotIndex].isUnlocked = true;
            coinsCount -= slotCosts[slotIndex];
            // visual changes
            slotTexts[slotIndex].text = "_";
        }
    }

    public void AddCoins(int amount)
    {
        coinsCount += amount;
    }

    public void TryAddBooster(IBooster booster, Cell boosterCell)
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
                slot.boosterType.Activate(playerBlock.elements[0].myCell);
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
