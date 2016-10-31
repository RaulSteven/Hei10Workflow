namespace Hei10.Core.Infrastructure
{
    public interface IStartupTask
    {
        void Execute();

        int Order { get; } 
    }
}