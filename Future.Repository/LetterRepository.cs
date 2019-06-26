using Dapper;
using Future.Model.DTO.Letter;
using Future.Model.Entity.Letter;
using System;
using System.Collections.Generic;

namespace Future.Repository
{
    public class LetterRepository : BaseRepository
    {
        private readonly string SELECT_DiscussEntity = "SELECT DiscussId,PickUpId,UId,DiscussContent,HasRead,CreateTime,UpdateTime FROM dbo.letter_Discuss ";

        private readonly string SELECT_LetterUserEntity = "SELECT UId,OpenId,Gender,NickName,BirthDate,Province,City,Country,Mobile,WeChatNo,HeadPhotoPath,CreateTime,UpdateTime FROM dbo.letter_LetterUser ";

        private readonly string SELECT_MomentEntity = "SELECT MomentId,UId,TextContent,ImgContent,IsDelete,IsReport,ReplyCount,CreateTime,UpdateTime FROM dbo.letter_Moment ";

        private readonly string SELECT_PickUpEntity = "SELECT PickUpId,MomentId,MomentUId,PickUpUId,IsUserDelete,IsPartnerDelete,CreateTime,UpdateTime FROM dbo.letter_PickUp ";


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
                return Db.Query<PickUpEntity>(sql,new { UId = uId , OFFSETCount = (pageIndex - 1) * pageSize , FETCHCount = pageSize }).AsList();
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
            var sql = string.Format("{0} Where PickUpId='{1}' Order by CreateTime", SELECT_DiscussEntity, pickUpId);
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

        public MomentEntity GetMoment(long uId,int pickUpCount)
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
                          Where moment.UId!=@UId  
                            and pick.MomentId is Null 
                            and moment.ReplyCount<=@PickUpCount 
                            and moment.IsReport=0 
                            and moment.IsDelete=0
                          Order by moment.CreateTime desc";
                return Db.QueryFirstOrDefault<MomentEntity>(sql, new { UId = uId, PickUpCount = pickUpCount });
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

        public bool InsertLetterUser(LetterUserEntity userEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.letter_LetterUser
                                  (OpenId
                                  ,Gender
                                  ,NickName
                                  ,BirthDate
                                  ,Province
                                  ,City
                                  ,Country
                                  ,Mobile
                                  ,WeChatNo
                                  ,HeadPhotoPath
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@OpenId
                                  ,@Gender
                                  ,@NickName
                                  ,@BirthDate
                                  ,@Province
                                  ,@City
                                  ,@Country
                                  ,@Mobile
                                  ,@WeChatNo
                                  ,@HeadPhotoPath
                                  ,@CreateTime
                                  ,@UpdateTime)";
                return Db.Execute(sql, userEntity) > 0;
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

        public bool DeleteDiscuss(Guid pickUpId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.letter_Discuss Where PickUpId=@PickUpId";
                return Db.Execute(sql, new { PickUpId= pickUpId }) > 0;
            }
        }

        public bool DeletePickUp(Guid pickUpId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.letter_PickUp Where PickUpId=@PickUpId";
                return Db.Execute(sql, new { PickUpId = pickUpId }) > 0;
            }
        }
    }
}
