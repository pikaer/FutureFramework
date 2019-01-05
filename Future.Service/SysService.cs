using Future.Model.Entity.Sys;
using Future.Model.Enum.Sys;
using Future.Repository;
using Future.Utility;
using System.Collections.Generic;

namespace Future.Service
{
    public class SysService
    {
        public static SysService Instance = SingletonProvider<SysService>.Instance;

        private readonly SysRepository sysDal=SysRepository.Instance;

        public List<Function> GetFunctionsByPara(int id,bool isKeyId=true)
        {
            if (isKeyId)
            {
                return sysDal.GetFunctionsById(id);
            }
            else
            {
                return sysDal.GetFunctionsByParentId(id);
            }
        }

        public List<Function> GetModules(EnumFuncType type)
        {
            return sysDal.GetFunctionsByFuncType((int)type);
        }
    }
}
