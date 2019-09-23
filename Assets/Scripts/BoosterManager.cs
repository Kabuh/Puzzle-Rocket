using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public static BoosterManager Instance { get; private set; }

    public Dictionary<BoosterType, Booster> boosters = new Dictionary<BoosterType, Booster>();

    public Camera cam;

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
        boosters.Add(BoosterType.Bomb, new Bomb());
        boosters.Add(BoosterType.BigBomb, new BigBomb());
        boosters.Add(BoosterType.Teleport, new Teleport(cam));
    }

    void Activate() {
    }
}
