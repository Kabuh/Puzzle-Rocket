using UnityEngine;

public static class DatabaseProvider
{
    private static Database databaseAsset;

    // lazy initialization
    public static Database Database
    {
        get
        {
            if(databaseAsset==null)
            {
                databaseAsset = Resources.Load<Database>("Database");
            }            
            return databaseAsset;
        }
        set
        {
            databaseAsset = value;
        }
    }
}
