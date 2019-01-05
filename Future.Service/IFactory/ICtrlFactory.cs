using Future.Service.Interface;

namespace Future.Service
{
    public interface ICtrlFactory : IFactory
    {
        IInsert Insert { get; }

        IDelete Delete { get; }

        IUpdate Update { get; }

        IQuery Query { get; }
    }
}
