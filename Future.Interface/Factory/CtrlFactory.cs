using Future.Utility;

namespace Future.Interface
{
    public class CtrlFactory
    {
        private static TFactory Instance<TFactory>(string ctrlName) where TFactory : class, IFactory
        {

            IFactory factory = null;

            switch (ctrlName.ToLower())
            {
                case "list":
                    factory = SingletonProvider<ListFactory>.Instance;
                    break;
            }

            return factory as TFactory;
        }

        public static ICtrlFactory Instance(string ctrlName)
        {
            var instance = Instance<ICtrlFactory>(ctrlName);
            return instance;
        }
    }
}
