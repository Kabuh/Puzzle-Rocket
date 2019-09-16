using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public static BoosterManager Instance { get; private set; }

    public Dictionary<BoosterType, Booster> boosters = new Dictionary<BoosterType, Booster>();

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
    }

    private void Start()
    {
        boosters.Add(BoosterType.Bomb, new Bomb(playerBlock));
        boosters.Add(BoosterType.BigBomb, new BigBomb(playerBlock));
    }

    void Activate() {
    }
}
