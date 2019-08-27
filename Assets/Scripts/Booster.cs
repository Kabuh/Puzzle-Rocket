public abstract class Booster : IBooster
{
    protected Block playerBlock;

    public Booster(Block playerBlock)
    {
        this.playerBlock = playerBlock;
    }

    public abstract void Activate(Cell cell);
}
