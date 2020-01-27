using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : Booster
{
    public override string BoosterName => "TimeStop";
    public override int MaxInInventory => 0;

    public float timePauseDuration = 3f;

    public override void Activate(Cell cell)
    {
        CameraS.Instance.TimeStopBooster(timePauseDuration);
        AnimationFX.Instance.PlayTimeStopAnimation(timePauseDuration);
    }
    
}
