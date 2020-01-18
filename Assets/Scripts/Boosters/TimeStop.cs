using UnityEngine;

[CreateAssetMenu(fileName = "Time Stop", menuName = "Boosters/Time Stop")]
public class TimeStop : BoosterType
{
    public float timePauseDuration = 3f;

    public override void Activate(Cell cell)
    {
        CameraS.Instance.TimeStopBooster(timePauseDuration);
        AnimationFX.Instance.PlayTimeStopAnimation(timePauseDuration);
    }
    
}
