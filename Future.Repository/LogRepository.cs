using Dapper;
using Future.Model.Entity.Sys;
using System;
using System.Collections.Generic;

namespace Future.Repository
{
    public class LogRepository : BaseRepository
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.LogService;
        }

        private readonly string SELECT_LOG = "SELECT LogId,LogLevel,TransactionID,UId,Platform,LogTitle,LogContent,ServiceName,CreateTime FROM dbo.sys_Log ";

        private readonly string SELECT_LOGTAG = "SELECT TagId,LogId,LogKey,LogValue,CreateTime FROM dbo.sys_LogTag ";

        private readonly string SELECT_ServiceLog = "SELECT ServiceLogId,ServiceName,Module,Method,Request,Response,UId,Code,Msg,Platform,TransactionId,CreateTime FROM dbo.sys_ServiceLog ";

        public List<LogEntity> GetLogList(int pageIndex, int pageSize)
        {
            using (var Db = GetDbConnection())
            {
                var sql = $@"{SELECT_LOG} order by CreateTime desc OFFSET @OFFSETCount ROWS FETCH NEXT @TakeCount ROWS ONLY";

                return Db.Query<LogEntity>(sql, new { OFFSETCount = (pageIndex - 1) * pageSize, TakeCount = pageSize }).AsList();
            }
        }

        public List<LogTag> LogTagList(Guid tagId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @SELECT_LOGTAG + " Where TagId=@TagId";
                return Db.Query<LogTag>(sql, new { TagId = tagId }).AsList();
            }
        }

        public int LogListCount()
        {
            using (var Db = GetDbConnection())
            {
                var sql = "select count(1) from dbo.sys_Log";

                return Db.QueryFirstOrDefault<int>(sql);
            }
        }
    }
}
