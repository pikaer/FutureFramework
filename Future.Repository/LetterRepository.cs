using Dapper;
using Future.Model.DTO.Letter;
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

        private readonly string SELECT_LetterUserEntity = "SELECT UId,OpenId,UserType,Gender,NickName,BirthDate,Province,City,Area,Country,Mobile,WeChatNo,HeadPhotoPath,Signature,SchoolName,SchoolType,LiveState,EntranceDate,CreateTime,UpdateTime FROM dbo.letter_LetterUser ";

        private readonly string SELECT_MomentEntity = "SELECT MomentId,UId,TextContent,ImgContent,IsDelete,IsReport,ReplyCount,CreateTime,UpdateTime FROM dbo.letter_Moment ";

        private readonly string SELECT_PickUpEntity = "SELECT PickUpId,MomentId,MomentUId,PickUpUId,IsUserDelete,IsPartnerDelete,CreateTime,UpdateTime FROM dbo.letter_PickUp ";

        private readonly string SELECT_CollectEntity = "SELECT CollectId,UId,MomentId,PickUpId,FromPage,CreateTime,UpdateTime FROM dbo.letter_Collect ";

        protected override DbEnum GetDbEnum()
        {
            return DbEnum.LetterService;
        }

        public LetterUserEntity LetterUser(long uId, string openId = "")
        {
            var sql = string.Empty;
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

        public List<PickUpEntity> PickUpListByPageIndex(long uId, int pageIndex, int pageSize)
        {
            var sql = @"SELECT pick.PickUpId,pick.MomentId,pick.MomentUId,pick.PickUpUId,pick.CreateTime,pick.UpdateTime 
                         FROM dbo.letter_PickUp pick 
                         Left Join letter_Discuss discuss on pick.PickUpId= discuss.PickUpId
                         Where PickUpUId=@UId and discuss.PickUpId is Null and IsPartnerDelete=0
                         Order by CreateTime desc 
                         OFFSET @OFFSETCount ROWS 
                         FETCH NEXT @FETCHCount ROWS ONLY";
            using (var Db = GetDbConnection())
            {
                return Db.Query<PickUpEntity>(sql, new { UId = uId, OFFSETCount = (pageIndex - 1) * pageSize, FETCHCount = pageSize }).AsList();
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
                                           dis1Temp.DiscussContent,
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
        
        public List<DiscussEntity>DiscussList(Guid pickUpId)
        {
            var sql = string.Format("{0} Where PickUpId='{1}' Order by CreateTime desc", SELECT_DiscussEntity, pickUpId);
            using (var Db = GetDbConnection())
            {
                return Db.Query<DiscussEntity>(sql).AsList();
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

        public MomentEntity GetMoment(long uId,int pickUpCount, GenderEnum gender)
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
                          Inner join dbo.letter_LetterUser us On us.UId=moment.UId
                          Where moment.UId!=@UId  
                            and moment.CreateTime<@CreateTime 
                            and pick.MomentId is Null 
                            and moment.ReplyCount<=@PickUpCount 
                            and moment.IsReport=0 
                            and moment.IsDelete=0
                            and us.Gender!=@Gender
                          Order by moment.CreateTime desc";
                return Db.QueryFirstOrDefault<MomentEntity>(sql, new { UId = uId, PickUpCount = pickUpCount, Gender = gender , CreateTime =DateTime.Now});
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

        public bool UpdatePickDelete(Guid pickUpId,int isUserDelete, int isPartnerDelete)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_PickUp
                               SET IsUserDelete =@IsUserDelete,
                                   IsPartnerDelete=@IsPartnerDelete,
                                   UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                return Db.Execute(sql, new {
                                              UpdateTime = DateTime.Now,
                                              PickUpId = pickUpId,
                                              IsUserDelete= isUserDelete,
                                              IsPartnerDelete= isPartnerDelete
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
        /// <summary>
        /// 删除所有我主动捡起的瓶子
        /// </summary>
        public bool DeleteAllPickBottle(Guid pickUpId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.letter_PickUp
                               SET IsPartnerDelete =1
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
                                  ,UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, PickUpId = pickUpId }) > 0;
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
                                  ,PickUpUId                              
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@PickUpId
                                  ,@MomentId
                                  ,@MomentUId
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
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@OpenId
                                  ,@Gender
                                  ,@UserType
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
                                  ,@CreateTime
                                  ,@UpdateTime)";
                return Db.Execute(sql, userEntity) > 0;
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
        public bool DeleteDiscuss(Guid pickUpId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.letter_Discuss Where PickUpId=@PickUpId";
                return Db.Execute(sql, new { PickUpId= pickUpId }) > 0;
            }
        }

        public bool DeleteSimulateMoment(Guid momentId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.letter_Moment Where MomentId=@MomentId";
                return Db.Execute(sql, new { MomentId = momentId }) > 0;
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
