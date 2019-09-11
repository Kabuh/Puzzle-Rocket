public interface ISpawnable
{
    void SelfDestroy();
    void DestroyIfLower();
    bool ShouldBeDestroyed();
    void ChangeCellsLevel();
}
