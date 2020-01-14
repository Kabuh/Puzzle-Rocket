using UnityEngine;

public static class SpawnerTools
{
    public static int ComplexRando(float[] valueLib, int cut)
    {
        float x = Random.Range(0, ArraySum(valueLib, cut));
        float currentSum = 0f;
        for (int i = 0; i < valueLib.Length; i++)
        {
            currentSum += valueLib[i];
            if (x < currentSum)
            {
                return ++i;
            }
        }
        Debug.LogError("Chance float array has false data");
        return 0;
    }

    public static float ArraySum(float[] array, int cut)
    {
        float Sum = 0f;
        for (int i = 0; i < array.Length - cut; i++)
        {
            Sum += array[i];
        }
        return Sum;
    }

    public static bool BinaryRandom(float chance)
    {
        float x = Random.Range(0, 100);
        if (chance > 100)
        {
            chance = 100;
        }

        if (x <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static float GetRatio(float x, float y)
    {
        return (x / (x + y));
    }

    public static float GetGroupRatio(float x, float y, float k, float l)
    {
        float sumOne = x + y;
        float sumTwo = k + l;
        return GetRatio(sumOne, sumTwo);
    }
}
