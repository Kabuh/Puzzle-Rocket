using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HubManagerScript : MonoBehaviour
{
    public static HubManagerScript Instance { get; private set; }

    [SerializeField] private string[] texts = new string[4];

    [SerializeField] private TextMeshProUGUI boosterOne;
    [SerializeField] private TextMeshProUGUI boosterTwo;

    int FirstRandomNumberForBlock;
    int FirstRandomNumberForBooster;
    int SecondRandomNumberForBlock;
    int SecondRandomNumberForBooster;

    public ChoiseBlockClass ChoiseBlockLeft = null;
    public ChoiseBlockClass ChoiseBlockRight = null;

    public List<string> BlockType;

    public List<string> BoosterType = new List<string>
        {
           {"shot_booster"},
           {"laser_V_booster"},
           {"laser_H_booster"},
           {"bomb_booster"},
           {"bigbomb_booster"},
           {"teleport_booster"},
           {"time_stop_booster"},
           {"PH_Slot" },
           {"PH_Slot2" },
           {"PH_Slot3" },
           {"PH_Slot4" },
           {"PH_Shrink" },
           {"PH_Move-enabler" },
           {"PH_Slow" },
           {"PH_Breaker" },
           {"PH_anti-immovables" }
        };

    public void HubSetup(ChoiseBlockClass choiseBlock) {
        if (ChoiseBlockLeft == null) {
            ChoiseBlockLeft = choiseBlock;
        }
        else if (ChoiseBlockRight == null) {
            ChoiseBlockRight = choiseBlock;
            RandomizerInit();
        }
        
    }

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

        SaveNode.PopulateLists();
        BlockType = new List<string>
        {
            { "Single" },
            { "Di Ver" },
            { "Tri Ver" },
            { "Di Hor" },
            { "Cube" },
            { "Tri Hor" },

            { "SingleV" },
            { "CubeV" },
            { "ImmovableV" },
            { "IM_CubeV" },

            { "Immovable" },
            { "IM_Di Ver" },
            { "IM_Tri Ver" },
            { "IM_Di Hor" },
            { "IM_Cube" },
            { "IM_Tri Hor" }
        };
    }

    private void RandomizerInit() {
        FirstRandomNumberForBlock = Mathf.FloorToInt(Random.Range(0, BlockType.Count));
        texts[0] = BlockType[FirstRandomNumberForBlock];

        FirstRandomNumberForBooster = Mathf.FloorToInt(Random.Range(0, BoosterType.Count));
        texts[1] = BoosterType[FirstRandomNumberForBooster];

        SecondRandomNumberForBlock = Mathf.FloorToInt(Random.Range(0, BlockType.Count));
        texts[2] = BlockType[SecondRandomNumberForBlock];

        SecondRandomNumberForBooster = Mathf.FloorToInt(Random.Range(0, BoosterType.Count));
        texts[3] = BoosterType[SecondRandomNumberForBooster];

        for (int i = 0; i < texts.Length; i++) {
            if (i < 2)
            {
                Instantiator(ref ChoiseBlockLeft, texts[i], i);
            }
            else {
                Instantiator(ref ChoiseBlockRight, texts[i], i);
            }
            
        }
        
    }

    private void Instantiator(ref ChoiseBlockClass item, string itemName, int isBlock) {
        if (isBlock % 2 == 0)
        {
            Block FakeBlock = Instantiate(
                Game.Instance.prefabs[itemName],
                new Vector3(-2, 6.5f, 0f),
                Quaternion.identity
                ).GetComponent<Block>();
            item.SetBlock(FakeBlock);
            FakeBlock.SelfDestroy();
        }
        else {
            if (itemName.Contains("PH_"))
            {
                if (isBlock == 1)
                {
                    boosterOne.text = itemName;
                }
                if (isBlock == 3)
                {
                    boosterTwo.text = itemName;
                }
            }
            else {
                BoosterObject FakeBooster = Instantiate(
                Game.Instance.prefabs[itemName],
                new Vector3(2, 8.5f, 0f),
                Quaternion.identity
                ).GetComponent<BoosterObject>();
                item.SetBooster(FakeBooster);
                FakeBooster.SelfDestroy();
            }
            
        }
        
    }





    public void Cast(Cell cell)
    {
        if (cell.XPos == 0)
        {
            SaveNode.PlayerBlockChoise = texts[0];
            SaveNode.PlayerBoosterChoise = texts[1];
            SaveNode.SavedBlockType.Remove(SaveNode.SavedBlockType[FirstRandomNumberForBlock]);
            SaveNode.SavedBlockType.Remove(SaveNode.SavedBlockType[FirstRandomNumberForBooster]);
            SceneManager.LoadScene(2);

        }
        else if (cell.XPos == 4)
        {
            SaveNode.PlayerBlockChoise = texts[2];
            SaveNode.PlayerBoosterChoise = texts[3];
            SaveNode.SavedBlockType.Remove(SaveNode.SavedBlockType[SecondRandomNumberForBlock]);
            SaveNode.SavedBlockType.Remove(SaveNode.SavedBlockType[SecondRandomNumberForBooster]);
            SceneManager.LoadScene(2);
        }
        else { Debug.Log("choise selection bug"); }
    }
}
