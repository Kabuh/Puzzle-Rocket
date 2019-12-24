public interface IBooster
{
    string BoosterName { get; }
    int MaxInInventory { get; }
    void Activate(Cell cell);
}
