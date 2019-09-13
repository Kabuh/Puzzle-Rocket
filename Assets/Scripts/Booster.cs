public abstract class Booster : IBooster
{    
    public abstract string BoosterName { get; }

    public abstract int MaxInInventory { get; }

    public abstract void Activate(Cell cell);
}
