using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveNode : ScriptableObject
{
    public static bool FirstRunCompleted = false;

    public static string PlayerBoosterChoise;
    public static string PlayerBlockChoise;

    public static List<string> BlockType = new List<string>{};
    public static List<string> BoosterType = new List<string>{};

    public static void PopulateLists() {
        if (!FirstRunCompleted) {

            BlockType = HubManagerScript.Instance.BlockType;
            BoosterType = HubManagerScript.Instance.BoosterType;
            FirstRunCompleted = true;
        }
        else
        {
            HubManagerScript.Instance.BlockType = BlockType;
            HubManagerScript.Instance.BoosterType = BoosterType;
        }
    }

    public static void GameOver() {
        FirstRunCompleted = false;
    }

}
