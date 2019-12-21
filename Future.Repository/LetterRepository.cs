using Dapper;
using Future.Model.DTO.Letter;
using Future.Model.Entity.Hubs;
using Future.Model.Entity.Letter;
using Future.Model.Enum.Letter;
using Future.Model.Enum.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Future.Repository
{
    public class LetterRepository : BaseRepository
    {
        private readonly string SELECT_DiscussEntity = "SELECT DiscussId,PickUpId,UId,DiscussContent,HasRead,CreateTime,UpdateTime FROM dbo.letter_Discuss ";

        private readonly string SELECT_LetterUserEntity = "SELECT UId,OpenId,Platform,UserType,Gender,NickName,BirthDate,Province,City,Area,Country,Mobile,WeChatNo,HeadPhotoPath,Signature,SchoolName,SchoolType,LiveState,EntranceDate,IsDelete,IsRegister,LastLoginTime,CreateTime,UpdateTime FROM dbo.letter_LetterUser ";

        private readonly string SELECT_MomentEntity = "SELECT MomentId,UId,TextContent,ImgContent,IsDelete,IsReport,ReplyCount,SubscribeMessageOpen,CreateTime,UpdateTime FROM dbo.letter_Moment ";

        private readonly string SELECT_PickUpEntity = "SELECT PickUpId,MomentId,MomentUId,PickUpUId,IsPickUpDelete,IsUserDelete,FromPage,IsPartnerDelete,UserLastDeleteTime,PartnerLastDeleteTime,CreateTime,UpdateTime FROM dbo.letter_PickUp ";

        private readonly string SELECT_CollectEntity = "SELECT CollectId,UId,MomentId,PickUpId,FromPage,CreateTime,UpdateTime FROM dbo.letter_Collect ";

        private readonly string SELECT_CoinEntity = "SELECT CoinId,UId,TotalCoin,CreateTime,UpdateTime FROM dbo.letter_Coin ";

        private readonly string SELECT_CoinDetailEntity = "SELECT CoinDetailId,UId,CoinId,ChangeValue,CoinChangeType,Remark,OperateUser,CreateTime,UpdateTime FROM dbo.letter_CoinDetail ";

        private readonly string SELECT_OnLineUserEntity = "SELECT OnLineId,UId,ConnectionId,IsOnLine,Latitude,Longitude,LastOnLineTime,CreateTime,UpdateTime FROM dbo.hub_OnLineUserHub ";

        private readonly string SELECT_ChatListHubEntity = "SELECT ChatListHubId,UId,ConnectionId,IsOnLine,LastOnLineTime,CreateTime,UpdateTime FROM dbo.hub_ChatListHub ";

        private readonly string SELECT_OnChatHubEntity = "SELECT OnChatHubId,UId,PartnerUId,ConnectionId,IsOnLine,LastOnLineTime,CreateTime,UpdateTime FROM dbo.hub_OnChatHub ";

        private readonly string SELECT_OnlineNotifyEntity = "SELECT OnlineNotifyId,UId,PartnerUId,CreateTime,UpdateTime FROM dbo.hub_OnlineNotify ";

        private readonly string SELECT_AttentionEntity = "SELECT AttentionId,UId,PartnerUId,AttentionMomentId,CreateTime,UpdateTime FROM dbo.letter_Attention ";

        protected override DbEnum GetDbEnum() => DbEnum.LetterService;

        public LetterUserEntity LetterUser(long uId, string openId = "")
        {
            string sql;
            if (!string.IsNullOrWhiteSpace(openId))
            {
                sql = string.Format("{0} Where OpenId='{1}'", SELECT_LetterUserEntity, openId);
            }
            else
            {
                sql = string.Format("{0} Where UId={1}", SELECT_LetterUserEntity, uId);
            }
            using (var Db = GetDbConnection())
            {
                return Db.QueryFirstOrDefault<LetterUserEntity>(sql);
            }
        }

        public OnLineUserHubEntity GetOnLineUser(long uId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} Where UId={1}", SELECT_OnLineUserEntity, uId);
                return Db.QueryFirstOrDefault<OnLineUserHubEntity>(sql);
            }
        }

        public ChatListHubEntity ChatListHub(long uId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} Where UId={1}", SELECT_ChatListHubEntity, uId);
                return Db.QueryFirstOrDefault<ChatListHubEntity>(sql);
            }
        }

        public OnChatHubEntity OnChatHub(long uId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} Where UId={1}", SELECT_OnChatHubEntity, uId);
                return Db.QueryFirstOrDefault<OnChatHubEntity>(sql);
            }
        }

        public OnlineNotifyEntity OnlineNotify(long uId,long partnerUId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} Where UId={1} and PartnerUId={2} ", SELECT_OnlineNotifyEntity, uId, partnerUId);
                return Db.QueryFirstOrDefault<OnlineNotifyEntity>(sql);
            }
        }

        public AttentionEntity Attention(long uId, long partnerUId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} Where UId={1} and PartnerUId={2} ", SELECT_AttentionEntity, uId, partnerUId);
                return Db.QueryFirstOrDefault<AttentionEntity>(sql);
            }
        }

        public List<PickUpDTO> PickUpListByPageIndex(long uId, int pageIndex, int pageSize)
        {
            var sql = @"SELECT pick.PickUpId,
                               pick.MomentId,
                               useinfo.UId,
                               useinfo.Gender,
                               useinfo.BirthDate,
                               useinfo.NickName,
                               useinfo.HeadPhotoPath,
                               moment.TextContent,
                               moment.ImgContent,
                               moment.CreateTime
                        FROM dbo.letter_PickUp pick 
                        Inner Join letter_Moment moment on moment.MomentId= pick.MomentId
                        Inner Join letter_LetterUser useinfo on useinfo.UId=pick.MomentUId
                        Where pick.PickUpUId=@UId and pick.IsPickUpDelete=0 and pick.FromPage=0 
                        Order by pick.CreateTime desc 
                        OFFSET @OFFSETCount ROWS
                        FETCH NEXT @FETCHCount ROWS ONLY";

            using (var Db = GetDbConnection())
            {
                return Db.Query<PickUpDTO>(sql, new { UId = uId, OFFSETCount = (pageIndex - 1) * pageSize, FETCHCount = pageSize }).AsList();
            }
        }

        public List<PickUpDTO> AttentionListByPageIndex(long uId, int pageIndex, int pageSize)
        {
            var sql = @"SELECT moment.MomentId,
                               useinfo.UId,
                               useinfo.Gender,
                               useinfo.BirthDate,
                               useinfo.NickName,
                               useinfo.HeadPhotoPath,
                               moment.TextContent,
                               moment.ImgContent,
                               moment.CreateTime
                        FROM dbo.letter_Attention attention 
                        Inner Join letter_Moment moment on moment.UId= attention.PartnerUId
                        Inner Join letter_LetterUser useinfo on useinfo.UId=attention.PartnerUId
                        Where attention.UId=@UId and moment.IsDelete=0 and moment.CreateTime>=attention.CreateTime
                        Order by moment.CreateTime desc 
                        OFFSET @OFFSETCount ROWS
                        FETCH NEXT @FETCHCount ROWS ONLY";

            using (var Db = GetDbConnection())
            {
                return Db.Query<PickUpDTO>(sql, new { UId = uId, OFFSETCount = (pageIndex - 1) * pageSize, FETCHCount = pageSize }).AsList();
            }
        }

        public Tuple<List<PickUpEntity>, int> PickUpListByParam(long uId, int pageIndex, int pageSize, PickUpTypeEnum pickType)
        {
            using (var Db = GetDbConnection())
            {
                var sql0 = @"SELECT pick.PickUpId,pick.MomentId,pick.MomentUId,pick.PickUpUId,pick.CreateTime,pick.UpdateTime 
                            FROM dbo.letter_PickUp pick  ";

                switch (pickType)
                {
                    case PickUpTypeEnum.IsDelete:
                        sql0 += "Where PickUpUId=@UId and  IsPartnerDelete=1 ";
                        break;
                    case PickUpTypeEnum.NoDelete:
                        sql0 += "Where PickUpUId=@UId and  IsPartnerDelete=0 ";
                        break;
                    case PickUpTypeEnum.HasDiscuss:
                        sql0 += @"inner Join letter_Discuss discuss on pick.PickUpId= discuss.PickUpId
                                  Where pick.PickUpUId = @UId ";
                        break;
                    case PickUpTypeEnum.NoDiscuss:
                        sql0 += @"Left Join letter_Discuss discuss on pick.PickUpId= discuss.PickUpId
                                  Where pick.PickUpUId = @UId and discuss.PickUpId is Null ";
                        break;
                    default:
                        sql0 += "Where PickUpUId=@UId ";
                        break;
                }

                var sql1 = sql0+ @"Order by pick.CreateTime desc  OFFSET @OFFSETCount ROWS  FETCH NEXT @FETCHCount ROWS ONLY";

                int count = Db.Query<PickUpEntity>(sql0, new { UId = uId }).AsList().Count;
                var list = Db.Query<PickUpEntity>(sql1, new { UId = uId, OFFSETCount = (pageIndex - 1) * pageSize, FETCHCount = pageSize }).AsList();
                return new Tuple<List<PickUpEntity>, int>(list, count);
            }
        }

        public List<PickUpEntity> PickUpListByPickUpUIdWithoutReply(long uId)
        {
            var sql = @"SELECT pick.PickUpId
                              ,pick.MomentId
                              ,pick.MomentUId
                              ,pick.PickUpUId
                              ,pick.IsUserDelete
                              ,pick.IsPartnerDelete
                              ,pick.CreateTime
                              ,pick.UpdateTime
                          FROM dbo.letter_PickUp pick
                          Left join letter_Discuss dis on pick.PickUpId=dis.PickUpId
                          Where PickUpUId=@UId and dis.PickUpId is Null";
            using (var Db = GetDbConnection())
            {
                return Db.Query<PickUpEntity>(sql, new { UId = uId }).AsList();
            }
        }

        public List<CollectDTO> CollectListByUId(long uId, int pageIndex, int pageSize)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"SELECT collect.CollectId
                              ,moment.UId
                              ,moment.MomentId
                              ,moment.TextContent
                        	  ,moment.ImgContent
                              ,collect.PickUpId
                              ,collect.CreateTime
                        FROM dbo.letter_Collect collect
                        Inner Join dbo.letter_Moment moment 
                        On collect.MomentId=moment.MomentId
                        Where collect.UId=@UId
                        Order by collect.CreateTime desc OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";
                return Db.Query<CollectDTO>(sql, new { UId = uId, Skip = (pageIndex - 1) * pageSize, Take = pageSize }).AsList();
            }
        }

        public List<PickUpEntity> PickUpListByPickUpUId(long uId)
        {
            var sql = @"SELECT pick.PickUpId
                              ,pick.MomentId
                              ,pick.MomentUId
                              ,pick.PickUpUId
                              ,pick.IsUserDelete
                              ,pick.IsPartnerDelete
                              ,pick.CreateTime
                              ,pick.UpdateTime
                          FROM dbo.letter_PickUp pick
                          Inner join letter_Discuss dis on pick.PickUpId=dis.PickUpId
                          Where PickUpUId=@UId";
            using (var Db = GetDbConnection())
            {
                return Db.Query<PickUpEntity>(sql,new { UId = uId }).AsList();
            }
        }

        public Tuple<List<LetterUserEntity>,int> GetSimulateUserList(int pageIndex, int pageSize, long uId, string nickName, GenderEnum gender, long creater, DateTime? startDateTime, DateTime? endCreateTime)
        {
            using (var Db = GetDbConnection())
            {
                var sql = new StringBuilder(SELECT_LetterUserEntity);
                sql.Append(" where UserType=2 and IsDelete=0 ");

                if (uId>0)
                {
                    sql.AppendFormat("and UId={0} ", uId);
                }

                if (!string.IsNullOrWhiteSpace(nickName))
                {
                    sql.AppendFormat("and NickName like '%{0}%' ", nickName.Trim());
                }

                if (gender != GenderEnum.All)
                {
                    sql.AppendFormat("and Gender={0} ", (int)gender);
                }

                if (creater > 0)
                {
                    sql.AppendFormat("and CreateUserId={0} ", creater);
                }

                if (!startDateTime.Equals(new DateTime()))
                {
                    sql.AppendFormat("and CreateTime>'{0}' ", startDateTime.Value.ToString());
                }

                if (!endCreateTime.Equals(new DateTime()))
                {
                    sql.AppendFormat("and CreateTime<'{0}' ", endCreateTime.Value.ToString());
                }

                int count = Db.Query<LetterUserEntity>(sql.ToString()).AsList().Count;

                sql.AppendFormat(" order by CreateTime desc OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (pageIndex - 1) * pageSize, pageSize);

                var list= Db.Query<LetterUserEntity>(sql.ToString()).AsList();

                return new Tuple<List<LetterUserEntity>, int>(list, count);
            }
        }

        public Tuple<List<LetterUserEntity>, int> GetRealUserList(int pageIndex, int pageSize, long uId, string nickName, string openId, GenderEnum gender, DateTime? startDateTime, DateTime? endCreateTime)
        {
            using (var Db = GetDbConnection())
            {
                var sql = new StringBuilder(SELECT_LetterUserEntity);
                sql.Append(" where UserType=0 and IsDelete=0 ");

                if (uId > 0)
                {
                    sql.AppendFormat("and UId={0} ", uId);
                }

                if (!string.IsNullOrWhiteSpace(nickName))
                {
                    sql.AppendFormat("and NickName like '%{0}%' ", nickName.Trim());
                }

                if (gender != GenderEnum.All)
                {
                    sql.AppendFormat("and Gender={0} ", (int)gender);
                }

                if (!string.IsNullOrWhiteSpace(openId))
                {
                    sql.AppendFormat("and OpenId like '%{0}%' ", openId);
                }

                if (!startDateTime.Equals(new DateTime()))
                {
                    sql.AppendFormat("and CreateTime>'{0}' ", startDateTime.Value.ToString());
                }

                if (!endCreateTime.Equals(new DateTime()))
                {
                    sql.AppendFormat("and CreateTime<'{0}' ", endCreateTime.Value.ToString());
                }

                int count = Db.Query<LetterUserEntity>(sql.ToString()).AsList().Count;

                sql.AppendFormat(" order by CreateTime desc OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (pageIndex - 1) * pageSize, pageSize);

                var list = Db.Query<LetterUserEntity>(sql.ToString()).AsList();

                return new Tuple<List<LetterUserEntity>, int>(list, count);
            }
        }

        public Tuple<List<MomentEntity>, int> GetMomentList(int pageIndex, int pageSize, long uId, MomentStateEnum state, DateTime? startDateTime, DateTime? endCreateTime)
        {
            using (var Db = GetDbConnection())
            {
                var sql = new StringBuilder("SELECT DISTINCT moment.MomentId,moment.UId,moment.TextContent,moment.ImgContent,moment.IsDelete,moment.IsReport,moment.ReplyCount,moment.CreateTime,moment.UpdateTime FROM dbo.letter_Moment  moment");
               
                switch (state)
                {
                    case MomentStateEnum.CanUse:
                        sql.AppendFormat("where moment.CreateTime<'{0}' ", DateTime.Now.ToString());
                        break;
                    case MomentStateEnum.CanNotUse:
                        sql.AppendFormat("where moment.CreateTime>'{0}' ", DateTime.Now.ToString());
                        break;
                    case MomentStateEnum.CanNotPickUp:
                        sql.Append("where moment.ReplyCount=0 ");
                        break;
                    case MomentStateEnum.HasDiscuss:
                        sql.Append(" Inner Join dbo.letter_PickUp pick on pick.MomentId=moment.MomentId" +
                                   " Inner Join dbo.letter_Discuss disc on pick.PickUpId=disc.PickUpId where 1=1");
                        break;
                    default:
                        sql.Append(" where 1=1 ");
                        break;

                }

                if (uId > 0)
                {
                    sql.AppendFormat("and moment.UId={0} ", uId);
                }
                if (!startDateTime.Equals(new DateTime()))
                {
                    sql.AppendFormat("and  moment.CreateTime>'{0}' ", startDateTime.Value.ToString());
                }
                if (!endCreateTime.Equals(new DateTime()))
                {
                    sql.AppendFormat("and  moment.CreateTime<'{0}' ", endCreateTime.Value.ToString());
                }

                sql.Append("Group by moment.MomentId,moment.UId,moment.TextContent,moment.ImgContent,moment.IsDelete,moment.IsReport,moment.ReplyCount,moment.CreateTime,moment.UpdateTime ");

                int count = Db.Query<MomentEntity>(sql.ToString()).AsList().Count;

                sql.AppendFormat(" order by moment.CreateTime desc OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (pageIndex - 1) * pageSize, pageSize);

                var list = Db.Query<MomentEntity>(sql.ToString()).AsList();

                return new Tuple<List<MomentEntity>, int>(list, count);
            }
        }

        public Tuple<List<PickUpEntity>, int> GetPickUpList(int pageIndex, int pageSize, Guid momentId, long uId, MomentPickUpEnum state)
        {
            using (var Db = GetDbConnection())
            {
                var sql = new StringBuilder("SELECT pick.PickUpId,pick.MomentId,pick.MomentUId,pick.PickUpUId,pick.IsUserDelete,pick.IsPartnerDelete,pick.CreateTime,pick.UpdateTime FROM dbo.letter_PickUp  pick ");

                switch (state)
                {
                    case MomentPickUpEnum.NoDiscuss:
                        sql.AppendFormat("Left Join dbo.letter_Discuss disc on pick.PickUpId=disc.PickUpId where disc.PickUpId is null and MomentId={0} ", momentId.ToString());
                        break;
                    case MomentPickUpEnum.HasDiscuss:
                        sql.AppendFormat("Inner Join dbo.letter_Discuss disc on pick.PickUpId=disc.PickUpId and MomentId={0} ", momentId.ToString());
                        break;
                    case MomentPickUpEnum.IsDelete:
                        sql.AppendFormat("where pick.IsPartnerDelete=1 and MomentId='{0}' ", momentId.ToString());
                        break;
                    case MomentPickUpEnum.NoDelete:
                        sql.AppendFormat("where pick.IsPartnerDelete=0 and MomentId='{0}' ", momentId.ToString());
                        break;
                    default:
                        sql.AppendFormat(" where MomentId='{0}' ", momentId.ToString());
                        break;
                }

                if (uId > 0)
                {
                    sql.AppendFormat("and pick.PickUpUId={0} ", uId);
                }
                int count = Db.Query<PickUpEntity>(sql.ToString()).AsList().Count;

                sql.AppendFormat(" order by pick.CreateTime desc OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (pageIndex - 1) * pageSize, pageSize);

                var list = Db.Query<PickUpEntity>(sql.ToString()).AsList();

                return new Tuple<List<PickUpEntity>, int>(list, count);
            }
        }

        public List<PickUpEntity> PickUpListByMomentUId(long uId)
        {
            var sql = @"SELECT pick.PickUpId
                              ,pick.MomentId
                              ,pick.MomentUId
                              ,pick.PickUpUId
                              ,pick.IsUserDelete
                              ,pick.IsPartnerDelete
                              ,pick.CreateTime
                              ,pick.UpdateTime
                          FROM dbo.letter_PickUp pick
                          Inner join letter_Discuss dis on pick.PickUpId=dis.PickUpId
                          Where MomentUId=@UId";
            using (var Db = GetDbConnection())
            {
                return Db.Query<PickUpEntity>(sql, new { UId = uId }).AsList();
            }
        }

        public List<PickUpDTO> PickUpDTOs(long uId, int pageIndex, int pageSize)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"SELECT *
				            From    (SELECT pick.PickUpId,
				          				 dis1Temp.CreateTime,
                                         dis1Temp.DiscussContent as 'TextContent',
				          			     us.UId,
				          				 us.NickName,
				          			     us.HeadPhotoPath
                                    FROM dbo.letter_PickUp pick
                                    Inner join (Select row_number() over(partition by dis1.PickUpId order by dis1.CreateTime desc) as rownum,
				          		                     dis1.PickUpId,
				          							 dis1.CreateTime,
				          		                     dis1.DiscussContent
				          		              From letter_Discuss dis1) dis1Temp
				          				on dis1Temp.PickUpId=pick.PickUpId and dis1Temp.rownum=1
				          		  Inner join letter_LetterUser us on us.UId=pick.MomentUId
                                    Where pick.PickUpUId=@UId and pick.IsPartnerDelete=0
                          
				          		  Union
                          
				          		  SELECT pick.PickUpId,
				          				 dis2Temp.CreateTime,
                                           dis2Temp.DiscussContent,
				          				 us.UId,
				          			     us.NickName,
				          			     us.HeadPhotoPath
                                    FROM dbo.letter_PickUp pick
                                    Inner join (Select row_number() over(partition by dis2.PickUpId order by dis2.CreateTime desc) as rownum,
				          		                     dis2.PickUpId,
				          							 dis2.CreateTime,
				          		                     dis2.DiscussContent
				          		              From letter_Discuss dis2) dis2Temp
				          				on dis2Temp.PickUpId=pick.PickUpId and dis2Temp.rownum=1
				          		  Inner join letter_LetterUser us on us.UId=pick.PickUpUId
                                    Where pick.MomentUId=@UId and pick.IsUserDelete=0) temp
				           Order by temp.CreateTime desc 
                           OFFSET @OFFSETCount ROWS 
                           FETCH NEXT @FETCHCount ROWS ONLY";
                return Db.Query<PickUpDTO>(sql, new { UId = uId, OFFSETCount = (pageIndex - 1) * pageSize, FETCHCount = pageSize }).AsList();
            }
        }

        public List<DiscussEntity> DiscussList(Guid pickUpId, DateTime? deleteTime = null)
        {
            string sql;
            if(deleteTime!=null&& deleteTime.HasValue)
            {
                sql = SELECT_DiscussEntity + @" Where PickUpId=@PickUpId and CreateTime>@DeleteTime Order by CreateTime desc";
            }
            else
            {
                sql = SELECT_DiscussEntity + @" Where PickUpId=@PickUpId  Order by CreateTime desc";
            }
            using (var Db = GetDbConnection())
            {
                return Db.Query<DiscussEntity>(sql,new { PickUpId= pickUpId , DeleteTime = deleteTime}).AsList();
            }
        }

        public PickUpEntity PickUp(Guid pickUpId)
        {
            var sql = string.Format("{0} Where PickUpId='{1}' ", SELECT_PickUpEntity, pickUpId.ToString());
            using (var Db = GetDbConnection())
            {
                return Db.QueryFirstOrDefault<PickUpEntity>(sql);
            }
        }

        public PickUpEntity PickUpByMomentId(Guid momentId,long uId)
        {
            var sql = string.Format("{0} Where MomentId='{1}' and PickUpUId={2}", SELECT_PickUpEntity, momentId.ToString(), uId);
            using (var Db = GetDbConnection())
            {
                return Db.QueryFirstOrDefault<PickUpEntity>(sql);
            }
        }

        public MomentEntity GetMoment(Guid momentId)
        {
            var sql = SELECT_MomentEntity + @" Where MomentId=@MomentId";
            using (var Db = GetDbConnection())
            {
                return Db.QueryFirstOrDefault<MomentEntity>(sql,new { MomentId= momentId });
            }
        }

        public CollectEntity GetCollect(Guid momentId,long uId)
        {
            var sql = SELECT_CollectEntity + @" Where MomentId=@MomentId and UId=@UId ";
            using (var Db = GetDbConnection())
            {
                return Db.QueryFirstOrDefault<CollectEntity>(sql, new { MomentId = momentId, UId= uId });
            }
        }

        public List<MomentEntity> GetMomentByPageIndex(long uId, int pageIndex, int pageSize)
        {
            var sql = SELECT_MomentEntity + @" Where UId=@UId and IsDelete=0
                                               Order by CreateTime desc 
                                               OFFSET @OFFSETCount ROWS 
                                               FETCH NEXT @FETCHCount ROWS ONLY";
            using (var Db = GetDbConnection())
            {
                return Db.Query<MomentEntity>(sql, new { UId = uId, OFFSETCount = (pageIndex - 1) * pageSize, FETCHCount = pageSize }).AsList();
            }
        }

        public List<CoinDetailEntity> GetCoinDetailListByUId(long uId, int pageIndex, int pageSize)
        {
            var sql = SELECT_CoinDetailEntity + @" Where UId=@UId 
                                               Order by CreateTime desc 
                                               OFFSET @OFFSETCount ROWS 
                                               FETCH NEXT @FETCHCount ROWS ONLY";
            using (var Db = GetDbConnection())
            {
                return Db.Query<CoinDetailEntity>(sql, new { UId = uId, OFFSETCount = (pageIndex - 1) * pageSize, FETCHCount = pageSize }).AsList();
            }
        }
        
        public MomentEntity GetMoment(long uId,int pickUpCount, GenderEnum gender,MomentTypeEnum momentType)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"SELECT moment.MomentId
                              ,moment.UId
                              ,moment.TextContent
                              ,moment.ImgContent
                              ,moment.IsDelete
                              ,moment.IsReport
                              ,moment.ReplyCount
                              ,moment.CreateTime
                              ,moment.UpdateTime
                          FROM dbo.letter_Moment moment
                          Left join dbo.letter_PickUp pick
                          ON moment.MomentId=pick.MomentId and pick.PickUpUId=@UId
                          Left join dbo.letter_Attention attention
                          ON moment.UId=attention.PartnerUId and attention.UId=@UId
                          Inner join dbo.letter_LetterUser us On us.UId=moment.UId
                          Where moment.UId!=@UId  
                            and moment.CreateTime<@CreateTime 
                            and pick.MomentId is Null 
                            and attention.AttentionId is Null 
                            and moment.ReplyCount<=@PickUpCount 
                            and moment.IsReport=0 
                            and moment.IsDelete=0
                            and us.Gender!=@Gender";
                if (momentType == MomentTypeEnum.TextMoment)
                {
                    sql += " and (moment.ImgContent is Null or moment.ImgContent='' )";
                }

                if(momentType == MomentTypeEnum.ImgMoment)
                {
                    sql += " and moment.ImgContent is not null and moment.ImgContent!='' ";
                }

                sql += " Order by moment.CreateTime desc ,moment.ReplyCount ";
                return Db.QueryFirstOrDefault<MomentEntity>(sql, new { UId = uId, PickUpCount = pickUpCount, Gender = gender , CreateTime =DateTime.Now});
            }
        }

        public CoinEntity GetCoinByUId(long uId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = SELECT_CoinEntity + @" Where UId=@UId ";
                return Db.QueryFirstOrDefault<CoinEntity>(sql, new { UId = uId });
            }
        }

        public int UnReadCount(Guid pickUpId, long uId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"SELECT Count(0) FROM dbo.letter_Discuss  Where PickUpId=@PickUpId and UId!=@UId and HasRead=0";
                return Db.QueryFirstOrDefault<int>(sql, new { PickUpId = pickUpId, UId = uId });
            }
        }

        public int UnReadTotalCount(long uId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"Select Count(0) 
                               From dbo.letter_PickUp pic
                               Inner Join dbo.letter_Discuss dis
                               On pic.PickUpId=dis.PickUpId 
                               Where ((pic.MomentUId=@UId and pic.IsUserDelete=0) or (pic.PickUpUId=@UId and pic.IsPartnerDelete=0 ))and dis.UId!=@UId and HasRead=0";
                return Db.QueryFirstOrDefault<int>(sql, new {UId = uId });
            }
        }

        public bool UpdatePickCount(Guid momentId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_Moment
                               SET ReplyCount =ReplyCount+1
                                  ,UpdateTime = @UpdateTime
                               WHERE MomentId=@MomentId";
                return Db.Execute(sql, new { UpdateTime =DateTime.Now, MomentId = momentId }) > 0;
            }
        }

        public bool UpdateUserTotalCoin(long coinId,long uId,int changeValue)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_Coin
                               SET TotalCoin =TotalCoin + @ChangeValue
                                  ,UpdateTime = @UpdateTime
                               WHERE CoinId=@CoinId and UId=@UId ";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, CoinId = coinId,UId= uId, ChangeValue = changeValue }) > 0;
            }
        }

        public bool UpdateDiscussHasRead(Guid pickUpId,long uId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_Discuss
                               SET HasRead =1
                                  ,UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId and UId=@UId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, PickUpId = pickUpId , UId = uId }) > 0;
            }
        }

        public bool UpdatePickDelete(Guid pickUpId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_PickUp
                               SET IsPickUpDelete =1,
                                   UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                return Db.Execute(sql, new {UpdateTime = DateTime.Now, PickUpId = pickUpId}) > 0;
            }
        }

        public bool UpdatePickDeleteTime(Guid pickUpId,DateTime updateTime, bool isUserDelete)
        {
            using (var Db = GetDbConnection())
            {
                string sql;
                if (isUserDelete)
                {
                    sql = @"UPDATE dbo.letter_PickUp
                               SET IsUserDelete =1,
                                   UserLastDeleteTime = @UpdateTime,
                                   UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                }
                else
                {
                    sql = @"UPDATE dbo.letter_PickUp
                               SET IsPartnerDelete=1,
                                   PartnerLastDeleteTime = @UpdateTime,
                                   UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                }
                return Db.Execute(sql, new
                {
                    UpdateTime = updateTime,
                    PickUpId = pickUpId
                }) > 0;
            }
        }

        public bool UpdatePickUpReport(Guid momentId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_Moment
                               SET IsReport =1
                                  ,UpdateTime = @UpdateTime
                               WHERE MomentId=@MomentId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, MomentId = momentId }) > 0;
            }
        }

        public bool UpdatePickUpUserDelete(Guid pickUpId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_PickUp
                               SET IsUserDelete =0
                                  ,UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, PickUpId = pickUpId }) > 0;
            }
        }

        public bool UpdatePickUpPartnerDelete(Guid pickUpId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_PickUp
                               SET IsPartnerDelete =0
                                  ,UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, PickUpId = pickUpId }) > 0;
            }
        }

        public bool UpdateMomentDelete(Guid momentId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_Moment
                               SET IsDelete =1
                                  ,UpdateTime = @UpdateTime
                               WHERE MomentId=@MomentId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, MomentId = momentId }) > 0;
            }
        }

        public bool UpdateOnLineUser(OnLineUserHubEntity onLineUser)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.hub_OnLineUserHub
                               SET ConnectionId = @ConnectionId
                                  ,IsOnLine = @IsOnLine
                                  ,LastOnLineTime = @LastOnLineTime
                                  ,UpdateTime = @UpdateTime
                               WHERE OnLineId=@OnLineId";
                return Db.Execute(sql, onLineUser) > 0;
            }
        }

        public bool UpdateUserLocation(OnLineUserHubEntity onLineUser)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.hub_OnLineUserHub
                               SET Latitude = @Latitude
                                  ,Longitude = @Longitude
                                  ,UpdateTime = @UpdateTime
                               WHERE OnLineId=@OnLineId";
                return Db.Execute(sql, onLineUser) > 0;
            }
        }

        public bool UpdateChatListHub(ChatListHubEntity chatListHub)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.hub_ChatListHub
                               SET ConnectionId = @ConnectionId
                                  ,IsOnLine = @IsOnLine
                                  ,LastOnLineTime = @LastOnLineTime
                                  ,UpdateTime = @UpdateTime
                               WHERE ChatListHubId=@ChatListHubId";
                return Db.Execute(sql, chatListHub) > 0;
            }
        }

        public bool UpdateOnChatHub(OnChatHubEntity userHub)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.hub_OnChatHub
                               SET ConnectionId = @ConnectionId
                                  ,IsOnLine = @IsOnLine
                                  ,PartnerUId = @PartnerUId
                                  ,LastOnLineTime = @LastOnLineTime
                                  ,UpdateTime = @UpdateTime
                               WHERE OnChatHubId=@OnChatHubId";
                return Db.Execute(sql, userHub) > 0;
            }
        }

        public bool UpdateMomentDelete(long uId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_Moment
                               SET IsDelete =1
                                  ,UpdateTime = @UpdateTime
                               WHERE UId=@UId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, UId = uId }) > 0;
            }
        }

        public bool UpdateLetterUserDelete(long uId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_LetterUser
                               SET IsDelete =1
                                  ,UpdateTime = @UpdateTime
                               WHERE UId=@UId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, UId = uId }) > 0;
            }
        }

        public bool UpdateMomentImgContent(string shortUrl, Guid momentId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_Moment
                               SET ImgContent =@ImgContent
                                  ,UpdateTime = @UpdateTime
                               WHERE MomentId=@MomentId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, MomentId = momentId, ImgContent= shortUrl }) > 0;
            }
        }
        
        public bool UpdateAvatarUrl(string avatarUrl,long uId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_LetterUser
                               SET HeadPhotoPath =@HeadPhotoPath
                                  ,UpdateTime = @UpdateTime
                               WHERE UId=@UId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, HeadPhotoPath = avatarUrl, UId= uId }) > 0;
            }
        }

        public bool UpdateHasRead(Guid pickUpId, long uId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_Discuss
                               SET HasRead =1
                                  ,UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId and UId!=@UId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, PickUpId = pickUpId, UId = uId }) > 0;
            }
        }

        public bool UpdateHasRead(Guid pickUpId, long uId,DateTime readTime)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_Discuss
                               SET HasRead =1
                                  ,UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId and UId!=@UId and CreateTime>@ReadTime";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, PickUpId = pickUpId, UId = uId, ReadTime= readTime }) > 0;
            }
        }

        public bool UpdateLetterUser(LetterUserEntity userEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.letter_LetterUser
                            SET Gender = @Gender
                               ,SchoolType= @SchoolType
                               ,SchoolName = @SchoolName
                               ,LiveState= @LiveState
                               ,NickName= @NickName
                               ,BirthDate = @BirthDate
                               ,EntranceDate= @EntranceDate
                               ,Province = @Province
                               ,City= @City
                               ,Area= @Area
                               ,Signature= @Signature
                               ,UpdateTime= @UpdateTime
                          WHERE UId=@UId";
                return Db.Execute(sql, userEntity) > 0;
            }
        }

        public bool UpdateUserBasicInfo(LetterUserEntity userEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.letter_LetterUser
                            SET Gender = @Gender
                               ,NickName= @NickName
                               ,HeadPhotoPath = @HeadPhotoPath
                               ,UpdateTime= @UpdateTime
                               ,Province = @Province
                               ,City= @City
                               ,Country= @Country
                               ,IsRegister=1
                          WHERE UId=@UId";
                return Db.Execute(sql, userEntity) > 0;
            }
        }

        public bool UpdateLastLoginTime(LetterUserEntity userEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.letter_LetterUser
                            SET LastLoginTime= @LastLoginTime
                               ,UpdateTime= @UpdateTime
                          WHERE UId=@UId";
                return Db.Execute(sql, userEntity) > 0;
            }
        }

        public bool UpdateMoment(MomentEntity momentEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.letter_Moment
                            SET TextContent = @TextContent
                               ,CreateTime= @CreateTime
                               ,UpdateTime= @UpdateTime
                          WHERE MomentId=@MomentId";
                return Db.Execute(sql, momentEntity) > 0;
            }
        }

        public bool UpdateCollectUpdateTime(CollectEntity entity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.letter_Collect
                            SET UpdateTime= @UpdateTime
                          WHERE CollectId=@CollectId";
                return Db.Execute(sql, entity) > 0;
            }
        }
        
        public bool InsertMoment(MomentEntity momentEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.letter_Moment
                                  (MomentId
                                  ,UId
                                  ,TextContent
                                  ,ImgContent
                                  ,IsDelete
                                  ,IsReport
                                  ,ReplyCount       
                                  ,SubscribeMessageOpen       
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@MomentId
                                  ,@UId
                                  ,@TextContent
                                  ,@ImgContent
                                  ,@IsDelete
                                  ,@IsReport
                                  ,@ReplyCount
                                  ,@SubscribeMessageOpen
                                  ,@CreateTime
                                  ,@UpdateTime)";
                return Db.Execute(sql, momentEntity) > 0;
            }
        }

        public bool InsertPickUp(PickUpEntity pickUpEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.letter_PickUp
                                  (PickUpId
                                  ,MomentId
                                  ,MomentUId
                                  ,FromPage      
                                  ,PickUpUId      
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@PickUpId
                                  ,@MomentId
                                  ,@MomentUId
                                  ,@FromPage
                                  ,@PickUpUId
                                  ,@CreateTime
                                  ,@UpdateTime)";
                return Db.Execute(sql, pickUpEntity) > 0;
            }
        }

        public bool InsertDiscuss(DiscussEntity discussEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.letter_Discuss
                                  (DiscussId
                                  ,PickUpId
                                  ,UId
                                  ,DiscussContent                              
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@DiscussId
                                  ,@PickUpId
                                  ,@UId
                                  ,@DiscussContent
                                  ,@CreateTime
                                  ,@UpdateTime)";
                return Db.Execute(sql, discussEntity) > 0;
            }
        }

        public bool InsertLetterUser(LetterUserEntity userEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.letter_LetterUser
                                  (OpenId
                                  ,Gender
                                  ,UserType
                                  ,Platform
                                  ,SchoolType
                                  ,LiveState
                                  ,EntranceDate
                                  ,SchoolName
                                  ,NickName
                                  ,BirthDate
                                  ,Province
                                  ,City
                                  ,Area
                                  ,Country
                                  ,Mobile
                                  ,WeChatNo
                                  ,HeadPhotoPath
                                  ,Signature
                                  ,LastLoginTime
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@OpenId
                                  ,@Gender
                                  ,@UserType
                                  ,@Platform
                                  ,@SchoolType
                                  ,@LiveState
                                  ,@EntranceDate
                                  ,@SchoolName
                                  ,@NickName
                                  ,@BirthDate
                                  ,@Province
                                  ,@City
                                  ,@Area
                                  ,@Country
                                  ,@Mobile
                                  ,@WeChatNo
                                  ,@HeadPhotoPath
                                  ,@Signature
                                  ,@LastLoginTime
                                  ,@CreateTime
                                  ,@UpdateTime)";
                return Db.Execute(sql, userEntity) > 0;
            }
        }

        public bool InsertCoin(CoinEntity entity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.letter_Coin
                                   (UId
                                   ,TotalCoin
                                   ,CreateTime
                                   ,UpdateTime)
                             VALUES
                                   (@UId
                                   ,@TotalCoin
                                   ,@CreateTime
                                   ,@UpdateTime)";
                return Db.Execute(sql, entity) > 0;
            }
        }

        public bool InsertOnlineNotify(OnlineNotifyEntity onlineNotify)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.hub_OnlineNotify
                                   (OnlineNotifyId
                                   ,UId
                                   ,PartnerUId
                                   ,CreateTime
                                   ,UpdateTime)
                             VALUES
                                   (@OnlineNotifyId
                                   ,@UId
                                   ,@PartnerUId
                                   ,@CreateTime
                                   ,@UpdateTime)";
                return Db.Execute(sql, onlineNotify) > 0;
            }
        }

        public bool InsertAttention(AttentionEntity entity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.letter_Attention
                                   (AttentionId
                                   ,UId
                                   ,PartnerUId
                                   ,AttentionMomentId
                                   ,CreateTime
                                   ,UpdateTime)
                             VALUES
                                   (@AttentionId
                                   ,@UId
                                   ,@PartnerUId
                                   ,@AttentionMomentId
                                   ,@CreateTime
                                   ,@UpdateTime)";
                return Db.Execute(sql, entity) > 0;
            }
        }


        public bool InsertChatListHub(ChatListHubEntity chatListHub)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.hub_ChatListHub
                                   (ChatListHubId
                                   ,UId
                                   ,ConnectionId
                                   ,IsOnLine
                                   ,LastOnLineTime
                                   ,CreateTime
                                   ,UpdateTime)
                             VALUES
                                   (@ChatListHubId
                                   ,@UId
                                   ,@ConnectionId
                                   ,@IsOnLine
                                   ,@LastOnLineTime
                                   ,@CreateTime
                                   ,@UpdateTime)";
                return Db.Execute(sql, chatListHub) > 0;
            }
        }
        public bool InsertOnChatHub(OnChatHubEntity userHub)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.hub_OnChatHub
                                   (OnChatHubId
                                   ,UId
                                   ,PartnerUId
                                   ,ConnectionId
                                   ,IsOnLine
                                   ,LastOnLineTime
                                   ,CreateTime
                                   ,UpdateTime)
                             VALUES
                                   (@OnChatHubId
                                   ,@UId
                                   ,@PartnerUId
                                   ,@ConnectionId
                                   ,@IsOnLine
                                   ,@LastOnLineTime
                                   ,@CreateTime
                                   ,@UpdateTime)";
                return Db.Execute(sql, userHub) > 0;
            }
        }

        public bool InsertOnLineUser(OnLineUserHubEntity onLineUser)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.hub_OnLineUserHub
                                   (OnLineId
                                   ,UId
                                   ,ConnectionId
                                   ,IsOnLine
                                   ,LastOnLineTime
                                   ,CreateTime
                                   ,Latitude
                                   ,Longitude
                                   ,UpdateTime)
                             VALUES
                                   (@OnLineId
                                   ,@UId
                                   ,@ConnectionId
                                   ,@IsOnLine
                                   ,@LastOnLineTime
                                   ,@CreateTime
                                   ,@Latitude
                                   ,@Longitude
                                   ,@UpdateTime)";
                return Db.Execute(sql, onLineUser) > 0;
            }
        }

        public bool InsertCoinDetail(CoinDetailEntity entity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.letter_CoinDetail
                                  (CoinDetailId
                                  ,UId
                                  ,CoinId
                                  ,ChangeValue
                                  ,CoinChangeType
                                  ,Remark
                                  ,OperateUser
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@CoinDetailId
                                  ,@UId
                                  ,@CoinId
                                  ,@ChangeValue
                                  ,@CoinChangeType
                                  ,@Remark
                                  ,@OperateUser
                                  ,@CreateTime
                                  ,@UpdateTime)";
                return Db.Execute(sql, entity) > 0;
            }
        }

        public bool InsertCollect(CollectEntity entity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.letter_Collect
                                  (CollectId
                                  ,UId
                                  ,MomentId
                                  ,PickUpId
                                  ,FromPage
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@CollectId
                                  ,@UId
                                  ,@MomentId
                                  ,@PickUpId
                                  ,@FromPage
                                  ,@CreateTime
                                  ,@UpdateTime)";
                return Db.Execute(sql, entity) > 0;
            }
        }

        /// <summary>
        /// 删除所有我主动捡起的瓶子
        /// </summary>
        public bool DeleteAllPickBottle(Guid pickUpId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_PickUp
                               SET IsPartnerDelete =1
                                  ,PartnerLastDeleteTime = @UpdateTime
                                  ,UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, PickUpId = pickUpId }) > 0;
            }
        }

        /// <summary>
        /// 删除所有我扔出去的被别人捡起的瓶子
        /// </summary>
        public bool DeleteAllPublishBottle(Guid pickUpId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_PickUp
                               SET IsUserDelete =1
                                  ,UserLastDeleteTime = @UpdateTime
                                  ,UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, PickUpId = pickUpId }) > 0;
            }
        }

        public bool DeleteDiscuss(Guid pickUpId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.letter_Discuss Where PickUpId=@PickUpId";
                return Db.Execute(sql, new { PickUpId= pickUpId }) > 0;
            }
        }

        public bool DeleteMoment(Guid momentId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.letter_Moment Where MomentId=@MomentId";
                return Db.Execute(sql, new { MomentId = momentId }) > 0;
            }
        }

        public bool DeletePushToken(Guid pushTokenId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.common_PushToken Where PushTokenId=@PushTokenId";
                return Db.Execute(sql, new { PushTokenId = pushTokenId }) > 0;
            }
        }

        public bool DeleteCollect(Guid collectId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.letter_Collect Where CollectId=@CollectId";
                return Db.Execute(sql, new { CollectId = collectId }) > 0;
            }
        }

        public bool DeleteAttention(long uId,long partnerUId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.letter_Attention Where UId=@UId and PartnerUId=@PartnerUId";
                return Db.Execute(sql, new { UId = uId, PartnerUId= partnerUId}) > 0;
            }
        }

        public bool DeleteAllCollect(long uId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.letter_Collect Where UId=@UId";
                return Db.Execute(sql, new { UId = uId }) > 0;
            }
        }
    }
}
