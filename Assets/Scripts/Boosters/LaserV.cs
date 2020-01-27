using UnityEngine;

[CreateAssetMenu(fileName = "Laser Vertical", menuName = "Boosters/Lazer Vertical")]
public class LaserV : LaserH
{
    public override void Activate(Cell cell)
    {
        playerBlock = Game.Instance.Player.playerBlock;
        GetBlocks(cell, Axis.Vertical);
        BlocksToDestroy.Remove(playerBlock);
        Cut(cell, Axis.Vertical);
    }
}
