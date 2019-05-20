namespace Future.Repository
{
    public class LogRepository : BaseRepository
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.LogService;
        }

        private readonly string SELECT_Log = "SELECT LogId, LogLevel, TransactionID, UId, Platform, LogTitle, LogContent, ServiceName, CreateTime FROM dbo.sys_Log ";

        private readonly string SELECT_ServiceLog = "SELECT ServiceLogId,ServiceName,Module,Method,Request,Response,UId,Code,Msg,Platform,TransactionId,CreateTime FROM dbo.sys_ServiceLog ";


    }
}
