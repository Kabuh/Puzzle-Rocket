using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public static BoosterManager Instance { get; private set; }

    public Dictionary<BoosterType, IBooster> boosters = new Dictionary<BoosterType, IBooster>();

    private Block playerBlock;

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
        playerBlock = Game.Instance.Player.GetComponent<Block>();
        boosters.Add(BoosterType.Bomb, new Bomb(playerBlock));
    }
}
