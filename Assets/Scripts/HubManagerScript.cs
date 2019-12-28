using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HubManagerScript : MonoBehaviour
{
    public static HubManagerScript Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI BoosterOne=null;
    [SerializeField] private TextMeshProUGUI BlockOne=null;
    [SerializeField] private TextMeshProUGUI BoosterTwo=null;
    [SerializeField] private TextMeshProUGUI BlockTwo=null;

    int FirstRandomNumberForBlock;
    int FirstRandomNumberForBooster;
    int SecondRandomNumberForBlock;
    int SecondRandomNumberForBooster;

    public List<string> BlockType = new List<string>
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
        Debug.Log(SaveNode.FirstRunCompleted);
        SaveNode.PopulateLists();

        FirstRandomNumberForBlock = Mathf.FloorToInt(Random.Range(0, BlockType.Count));
        BlockOne.text = BlockType[FirstRandomNumberForBlock];

        FirstRandomNumberForBooster = Mathf.FloorToInt(Random.Range(0, BoosterType.Count));
        BoosterOne.text = BoosterType[FirstRandomNumberForBooster];

        SecondRandomNumberForBlock = Mathf.FloorToInt(Random.Range(0, BlockType.Count));
        BlockTwo.text = BlockType[SecondRandomNumberForBlock];

        SecondRandomNumberForBooster = Mathf.FloorToInt(Random.Range(0, BoosterType.Count));
        BoosterTwo.text = BoosterType[SecondRandomNumberForBooster];
    }



    public void Cast(Cell cell)
    {
        if (cell.XPos == 0)
        {
            SaveNode.PlayerBlockChoise = BlockOne.text;
            SaveNode.PlayerBoosterChoise = BoosterOne.text;
            SaveNode.BlockType.Remove(SaveNode.BlockType[FirstRandomNumberForBlock]);
            SaveNode.BoosterType.Remove(SaveNode.BlockType[FirstRandomNumberForBooster]);
            SceneManager.LoadScene(2);

        }
        else if (cell.XPos == 4)
        {
            SaveNode.PlayerBlockChoise = BlockTwo.text;
            SaveNode.PlayerBoosterChoise = BoosterTwo.text;
            SaveNode.BlockType.Remove(SaveNode.BlockType[SecondRandomNumberForBlock]);
            SaveNode.BoosterType.Remove(SaveNode.BlockType[SecondRandomNumberForBooster]);
            SceneManager.LoadScene(2);
        }
        else { Debug.Log("choise selection bug"); }
    }
}
