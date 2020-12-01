namespace NotTimeTravel.Core.Logic
{
    public interface IGameAction
    {
        bool IsActive();
        void Invoke();
    }
}