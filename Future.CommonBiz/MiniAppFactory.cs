using Future.CommonBiz.Impls;
using Future.Model.Enum.Letter;
using Infrastructure;

namespace Future.CommonBiz
{
    public class MiniAppFactory
    {
        public static IMiniAppBiz Factory(PlatformEnum platform)
        {
            switch (platform)
            {
                case PlatformEnum.QQ_MiniApp:
                    return SingletonProvider<QQBiz>.Instance;
                case PlatformEnum.WX_MiniApp:
                default:
                    return SingletonProvider<WeChatBiz>.Instance;
            }
        }
    }
}
