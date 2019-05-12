﻿using Dapper;
using Future.Model.DTO.Sys;
using Future.Model.Entity.Sys;
using Infrastructure;
using System.Collections.Generic;

namespace Future.Repository
{
    public class SysRepository : BaseRepository
    {
        public static SysRepository Instance = SingletonProvider<SysRepository>.Instance;

        private readonly string SELECT_FUNCTION = "SELECT Id,ParentId,Text,Url,IconCls,EnumFuncType,Remark,CreateTime,ModifyTime,CreateUserId,ModifyUserId FROM dbo.sys_Function";

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
                var sql = string.Format("{0} where Id={1}", SELECT_FUNCTION, id);
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

        public bool DeleteFuncByParentId(int parentId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = string.Format("Delete sys_Function where ParentId={0}", parentId);
                return Db.Execute(sql) > 0;
            }
        }

        public bool DeleteFuncByFuncId(int funcId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = string.Format("Delete sys_Function where Id={0}", funcId);
                return Db.Execute(sql)>0;
            }
        }
    }
}
