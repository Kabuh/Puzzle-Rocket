using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [SerializeField] private int slotsCount = 4;

    [SerializeField] private int[] slotCosts = null;

    [SerializeField] private Text[] slotTexts = null;

    [SerializeField] private Button[] buttons = null;

    [SerializeField] private Image[] artNotch = new Image[4];

    private Slot[] slots;

    private Slot nextSlotToUnlock;
    private Button nextButtonToUnlock;

    private int coinsCount;
    private int buttonCounter;

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
            slots[i].UpdateSlotText();
        }
        buttonCounter = 1;
        nextSlotToUnlock = slots[buttonCounter];
        nextButtonToUnlock = buttons[buttonCounter];
        // FIRST SLOT IS UNLOCKED FROM THE START
        slots[0].isUnlocked = true;        
    }

    //// TEMP!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
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

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    TryAddBooster(BoosterManager.Instance.boosters[BoosterType.Teleport], Game.Instance.CombinedGrid.cells[0, 0], );
        //}
    }

    public void ButtonOneActivate() {
        OnSlotTouch(0);
    }
    public void ButtonTwoActivate()
    {
        OnSlotTouch(1);
    }
    public void ButtonThreeActivate()
    {
        OnSlotTouch(2);
    }
    public void ButtonFourActivate()
    {
        OnSlotTouch(3);
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
            nextButtonToUnlock.interactable = true;
            if (nextSlotToUnlock.index < slots.Length - 1)
            {
                buttonCounter++;
                nextSlotToUnlock = slots[buttonCounter];
                nextButtonToUnlock = buttons[buttonCounter];
            }
        }
    }

    //after collide booster handler
    public void TryAddBooster(Booster booster, Cell boosterCell, Sprite sprite)
    {
        Slot emptySlot = null;


        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].isUnlocked)
            {
                if (slots[i].boosterType == booster)
                {
                    if (slots[i].boostersCount == slots[i].maxBoosters)
                    {
                        booster.Activate(boosterCell);
                    }
                    else
                    {
                        AddToSlot(slots[i]);
                    }
                    return;
                }

                if (emptySlot == null && slots[i].boosterType == null)
                {
                    emptySlot = slots[i];
                }
            }
        }


        if (emptySlot != null && booster.MaxInInventory > 0)
        {
            FillSlot(emptySlot, booster);
            artNotch[emptySlot.index].sprite = sprite;
            Color fillerColor = artNotch[emptySlot.index].color;
            fillerColor.a = 1;
            artNotch[emptySlot.index].color = fillerColor;
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
                    artNotch[slot.index].sprite = null;
                    Debug.Log("slot to add opaque - " + slot.index);
                    Color fillerColor = artNotch[slot.index].color;
                    fillerColor.a = 0.2f;
                    artNotch[slot.index].color = fillerColor;
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
