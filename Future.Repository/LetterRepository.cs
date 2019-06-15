using Dapper;
using Future.Model.Entity.Letter;
using System;
using System.Collections.Generic;

namespace Future.Repository
{
    public class LetterRepository : BaseRepository
    {
        private readonly string SELECT_DiscussEntity = "SELECT DiscussId,PickUpId,UId,DiscussContent,CreateTime,UpdateTime FROM dbo.letter_Discuss ";

        private readonly string SELECT_LetterUserEntity = "SELECT UId,OpenId,Gender,NickName,BirthDate,Province,City,Country,Mobile,WeChatNo,HeadPhotoPath,CreateTime,UpdateTime FROM dbo.letter_LetterUser ";

        private readonly string SELECT_MomentEntity = "SELECT MomentId,UId,TextContent,ImgContent,IsDelete,IsReport,ReplyCount,CreateTime,UpdateTime FROM dbo.letter_Moment ";

        private readonly string SELECT_PickUpEntity = "SELECT PickUpId,MomentId,MomentUId,PickUpUId,CreateTime,UpdateTime FROM dbo.letter_PickUp ";


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

        public List<PickUpEntity> PickUpList(long uId,int pageIndex)
        {
            var sql = string.Format("{0} Where PickUpUId={1}", SELECT_PickUpEntity, uId);
            using (var Db = GetDbConnection())
            {
                return Db.Query<PickUpEntity>(sql).AsList();
            }
        }

        public List<DiscussEntity>DiscussList(Guid pickUpId)
        {
            var sql = string.Format("{0} Where PickUpId='{1}'", SELECT_DiscussEntity, pickUpId);
            using (var Db = GetDbConnection())
            {
                return Db.Query<DiscussEntity>(sql).AsList();
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
    }
}
