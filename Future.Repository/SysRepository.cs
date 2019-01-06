using Dapper;
using Future.Model.DTO.Sys;
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
        /// 获取Function列表
        /// </summary>
        public List<FunctionDTO> GetFunctions()
        {
            using (var Db = GetDbConnection())
            {
                return Db.Query<FunctionDTO>(SELECT_FUNCTION).AsList();
            }
        }

        /// <summary>
        /// 通过ParentId获取Function列表
        /// </summary>
        public List<FunctionDTO> GetFunctionsByParentId(int id)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} where ParentId={1}", SELECT_FUNCTION, id);
                return Db.Query<FunctionDTO>(sql).AsList();
            }
        }

        /// <summary>
        /// 通过FuncId获取Function列表
        /// </summary>
        public Function GetFunctionByFuncId(int id)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} where FuncId={1}", SELECT_FUNCTION, id);
                return Db.QueryFirstOrDefault<Function>(sql);
            }
        }

        /// <summary>
        /// 通过FuncType获取Function列表
        /// </summary>
        public List<FunctionDTO> GetFunctionDTOByFuncType(int type)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} where EnumFuncType={1}", SELECT_FUNCTION, type);
                return Db.Query<FunctionDTO>(sql).AsList();
            }
        }

        public bool AddFunction(Function req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.sys_Function
                                           (ParentId
                                           ,Text
                                           ,Url
                                           ,IconCls
                                           ,EnumFuncType
                                           ,Remark
                                           ,OrderIndex
                                           ,CreateTime
                                           ,ModifyTime
                                           ,CreateUserId
                                           ,ModifyUserId)
                                     VALUES
                                           (@ParentId
                                           ,@Text
                                           ,@Url
                                           ,@IconCls
                                           ,@EnumFuncType
                                           ,@Remark
                                           ,@OrderIndex
                                           ,@CreateTime
                                           ,@ModifyTime
                                           ,@CreateUserId
                                           ,@ModifyUserId";
                return Db.Execute(sql, req)>0;
            }
        }
    }
}
