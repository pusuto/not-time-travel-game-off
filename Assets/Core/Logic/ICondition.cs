namespace NotTimeTravel.Core.Logic
{
    public interface ICondition
    {
        bool IsActive();
        bool IsCleared();
        string GetMessage();
    }
}