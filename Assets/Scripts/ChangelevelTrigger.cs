using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangelevelTrigger : Booster
{
    public override string BoosterName => "ChangeLevel";
    public override int MaxInInventory => 0;

    public override void Activate(Cell cell)
    {
        SceneManager.LoadScene(2);
    }
}
