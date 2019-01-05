using Dapper;
using Future.Model.Entity.Sys;
using Future.Utility;
using System.Collections.Generic;

namespace Future.Repository
{
    public class SysRepository : BaseRepository
    {
        public static SysRepository Instance = SingletonProvider<SysRepository>.Instance;

        private readonly string SELECT_FUNCTION = "SELECT FuncId,ParentId,Text,Url,IconCls,EnumFuncType,Remark,CreateTime,ModifyTime,CreateUserId,ModifyUserId FROM dbo.sys_Function";

        protected override DbEnum GetDbEnum()
        {
            return DbEnum.FutureFramework;
        }

        /// <summary>
        /// 通过FuncId获取Function列表
        /// </summary>
        public List<Function> GetFunctionsById(int id)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} where FuncId={1}", SELECT_FUNCTION, id);
                return Db.Query<Function>(sql).AsList();
            }
        }

        /// <summary>
        /// 通过ParentId获取Function列表
        /// </summary>
        public List<Function> GetFunctionsByParentId(int id)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} where ParentId={1}", SELECT_FUNCTION, id);
                return Db.Query<Function>(sql).AsList();
            }
        }

        /// <summary>
        /// 通过FuncType获取Function列表
        /// </summary>
        public List<Function> GetFunctionsByFuncType(int type)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} where EnumFuncType={1}", SELECT_FUNCTION, type);
                return Db.Query<Function>(sql).AsList();
            }
        }
    }
}
