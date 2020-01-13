using UnityEngine;

public static class Database
{
    private static DatabaseAsset databaseAsset;

    // lazy initialization
    public static DatabaseAsset Asset
    {
        get
        {
            if(databaseAsset==null)
            {
                databaseAsset = Resources.Load<DatabaseAsset>("Database");
            }            
            return databaseAsset;
        }
        set
        {
            databaseAsset = value;
        }
    }
}
