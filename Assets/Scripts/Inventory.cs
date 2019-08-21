using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Inventory Instance { get; private set; }

    [SerializeField] private int slotsCount = 3;

    [SerializeField] private Text[] slotTexts;

    private IBooster[] boosterSlots;
    private bool[] openSlots;
    private int emptySlotsCount;

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

        boosterSlots = new IBooster[slotsCount];
        openSlots = new bool[slotsCount];
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

    private void OnSlotTouch(int slot)
    {
        if (!openSlots[slot])
        {
            UnlockSlot(slot);
        }
        else
        {
            if (boosterSlots[slot] != null)
            {
                boosterSlots[slot].Activate();
            }
        }
    }

    public void UnlockSlot(int slotIndex)
    {
        openSlots[slotIndex] = true;
        emptySlotsCount++;
        // visual changes
        slotTexts[slotIndex].text = "_";
    }

    public void TryAddBooster(IBooster booster)
    {
        if(emptySlotsCount>0)
        {
            AddBooster(booster);
        }
        else
        {
            booster.Activate();
        }
    }

    public void UseBooster(int slot)
    {
        if (openSlots[slot])
        {
            if (boosterSlots != null)
            {
                boosterSlots[slot].Activate();
                ClearSlot(slot);
            }
        }
    }

    private void AddBooster(IBooster booster)
    {
        for (int i = 0; i < openSlots.Length; i++)
        {
            if(openSlots[i])
            {
                if(boosterSlots[i]==null)
                {
                    FillSlot(i, booster);
                }
            }
        }
    }

    private void FillSlot(int slot, IBooster booster)
    {
        boosterSlots[slot] = booster;
        // + visual changes or fire event listened by ui
    }

    private void ClearSlot(int slot)
    {
        boosterSlots[slot] = null;
        // + visual changes or fire event listened by ui
    }
}
