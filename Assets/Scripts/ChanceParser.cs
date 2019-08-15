using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChanceParser : MonoBehaviour
{
    public List<Text> testerUIprompts = new List<Text>();
    public List<Text> testerUIcheckSums = new List<Text>();
    public Text ConsoleUI;
    public Text testField;

    LevelManager levelManagerInstance;
    void Start()
    {
        levelManagerInstance = GetComponent<LevelManager>();
    }

    public static string ArrayToString(int[] arr)
    {
        List<object> lst = new List<object>();
        object[] obj = new object[arr.Length];
        System.Array.Copy(arr, obj, arr.Length);

        lst.AddRange(obj);
        return string.Join(",", lst.ToArray());
    }

    int[] Parser(Text prompt)
    {
        string textLine = prompt.text;
        string holderString = "";
        int currentIndex = 0;
        List<int> chanceNumbers = new List<int>();
        for (int i = 0; i < textLine.Length; i++)
        {
            string k = textLine[i].ToString();
            if (k == " ")
            {
                currentIndex = 0;

                chanceNumbers.Add(int.Parse(holderString));
                holderString = "";
            }
            else
            {
                holderString = holderString.Insert(currentIndex, k);
                currentIndex++;
            }
        }

        return chanceNumbers.ToArray();
    }

    void CheckSumCheck(Text prompt, Text CheckSum)
    {
        CheckSum.text = "CheckSum:" + Sum(Parser(prompt));
    }

    int Sum(int[] array)
    {
        int total = 0;
        foreach (int num in array)
        {
            total += num;
        }
        return total;
    }

    public void MakeNewChances()
    {
        NullCheckerEqual(Parser(testerUIprompts[0]), ref levelManagerInstance.sizeChancesIftwo);
        NullCheckerEqual(Parser(testerUIprompts[1]), ref levelManagerInstance.sizeChancesIfOne);
        NullCheckerEqual(Parser(testerUIprompts[2]), ref levelManagerInstance.sizeChancesIfNone);
        NullCheckerEqual(Parser(testerUIprompts[3]), ref levelManagerInstance.typeChances);

        testField.text = "New chances are: \n " + ArrayToString(levelManagerInstance.typeChances) + "| \n" + ArrayToString(levelManagerInstance.sizeChancesIftwo) + "| /n" + ArrayToString(levelManagerInstance.sizeChancesIfOne) + "| /n" + ArrayToString(levelManagerInstance.sizeChancesIfNone);
    }

    void NullCheckerEqual(int[] newData, ref int[] dataSlot)
    {
        if (newData != null)
        { 
            if (newData.Length == dataSlot.Length)
            {
                
                System.Array.Copy(newData, dataSlot, dataSlot.Length);
                ConsoleUI.text += "\n chances created succesfuly ";
            }
            else
            {
                ConsoleUI.text += "\n check array length";
            }

        }
    }

    private void Update()
    {
        for (int i = 0; i < testerUIprompts.Count; i++)
        {
            if (testerUIprompts[i].text.Length > 0)
            {
                CheckSumCheck(testerUIprompts[i], testerUIcheckSums[i]);
            }
        }

    }
}
