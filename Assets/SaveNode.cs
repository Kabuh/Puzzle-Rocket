using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveNode
{
    public static bool FirstRunCompleted = false;

    public static string PlayerBoosterChoise;
    public static string PlayerBlockChoise;

    public static List<string> SavedBlockType = new List<string>{};
    public static List<string> SavedBoosterType = new List<string>{};

    public static void PopulateLists() {
        if (!FirstRunCompleted) {

            SavedBlockType = HubManagerScript.Instance.BlockType;
            SavedBoosterType = HubManagerScript.Instance.BoosterType;
            FirstRunCompleted = true;
        }
        else
        {
            HubManagerScript.Instance.BlockType = SavedBlockType;
            HubManagerScript.Instance.BoosterType = SavedBoosterType;
        }
    }

    public static void GameOver() {
        FirstRunCompleted = false;
    }
}
