﻿using Dapper;
using Future.Model.DTO.Letter;
using Future.Model.Entity.Bingo;
using Future.Model.Entity.Hubs;
using Future.Model.Enum.Bingo;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Future.Repository
{
    public class BingoRepository : BaseRepository
    {
        private readonly string SELECT_DiscussEntity = "SELECT DiscussId,PickUpId,UId,DiscussContent,HasRead,CreateTime,UpdateTime FROM dbo.bingo_Discuss ";

        private readonly string SELECT_LetterUserEntity = "SELECT UId,OpenId,Platform,UserType,Gender,NickName,BirthDate,Province,City,Area,Country,Mobile,WeChatNo,HeadPhotoPath,Signature,SchoolName,SchoolType,LiveState,EntranceDate,IsDelete,IsRegister,LastLoginTime,CreateTime,UpdateTime FROM dbo.bingo_UserInfo ";

        private readonly string SELECT_MomentEntity = "SELECT MomentId,UId,TextContent,ImgContent,IsDelete,IsReport,ReplyCount,IsHide,HidingNickName,SourceFlag,PlayTypeTag,StateType,SubscribeMessageOpen,CreateTime,UpdateTime FROM dbo.bingo_Moment ";

        private readonly string SELECT_PickUpEntity = "SELECT PickUpId,MomentId,MomentUId,PickUpUId,IsPickUpDelete,IsUserDelete,FromPage,IsHide,HidingNickName,IsPartnerDelete,UserLastDeleteTime,PartnerLastDeleteTime,CreateTime,UpdateTime FROM dbo.bingo_PickUp ";

        private readonly string SELECT_CollectEntity = "SELECT CollectId,UId,MomentId,PickUpId,FromPage,CreateTime,UpdateTime FROM dbo.bingo_Collect ";

        private readonly string SELECT_CoinEntity = "SELECT CoinId,UId,TotalCoin,CreateTime,UpdateTime FROM dbo.bingo_Coin ";

        private readonly string SELECT_UserTagEntity = "SELECT TagId,UId,TagType,Tag,CreateTime,UpdateTime FROM dbo.bingo_UserTag ";

        private readonly string SELECT_CoinDetailEntity = "SELECT CoinDetailId,UId,CoinId,ChangeValue,CoinChangeType,Remark,OperateUser,CreateTime,UpdateTime FROM dbo.bingo_CoinDetail ";

        private readonly string SELECT_OnLineUserEntity = "SELECT OnLineId,UId,ConnectionId,IsOnLine,Latitude,Longitude,LastOnLineTime,LastScanMomentTime,CreateTime,UpdateTime FROM dbo.hub_OnLineUserHub ";

        private readonly string SELECT_ChatListHubEntity = "SELECT ChatListHubId,UId,ConnectionId,IsOnLine,LastOnLineTime,CreateTime,UpdateTime FROM dbo.hub_ChatListHub ";

        private readonly string SELECT_OnChatHubEntity = "SELECT OnChatHubId,UId,PartnerUId,ConnectionId,IsOnLine,LastOnLineTime,CreateTime,UpdateTime FROM dbo.hub_OnChatHub ";

        private readonly string SELECT_OnlineNotifyEntity = "SELECT OnlineNotifyId,UId,PartnerUId,CreateTime,UpdateTime FROM dbo.hub_OnlineNotify ";

        private readonly string SELECT_AttentionEntity = "SELECT AttentionId,UId,PartnerUId,AttentionMomentId,CreateTime,UpdateTime FROM dbo.bingo_Attention ";

        protected override DbEnum GetDbEnum() => DbEnum.LetterService;

        public UserInfoEntity LetterUser(long uId, string openId = "")
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
                return Db.QueryFirstOrDefault<UserInfoEntity>(sql);
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

        public List<PickUpDTO> PickUpListByPageIndex(long uId, int pageIndex, int pageSize, bool playMoment=false)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format(@"SELECT pick.PickUpId,
                                                 pick.MomentId,
                                                 useinfo.UId,
                                                 useinfo.Gender,
                                                 useinfo.BirthDate,
                                                 useinfo.NickName,
                                                 useinfo.HeadPhotoPath,
                                                 moment.UId as 'MomentUId',
                                                 moment.TextContent,
                                                 moment.ImgContent,
                                                 moment.IsHide,
                                                 moment.HidingNickName,
                                                 moment.PlayTypeTag,
                                                 moment.StateType,
                                                 moment.CreateTime
                                          FROM dbo.bingo_PickUp pick 
                                          Inner Join bingo_Moment moment on moment.MomentId= pick.MomentId
                                          Inner Join bingo_UserInfo useinfo on useinfo.UId=pick.MomentUId
                                          Where pick.PickUpUId={0} and pick.IsPickUpDelete=0 and pick.FromPage=0 ", uId);
                if (playMoment)
                {
                    sql += " and SourceFlag=1 ";
                }
                else
                {
                    sql += " and SourceFlag!=1 ";
                }

                sql += string.Format(" Order by pick.CreateTime desc  OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", (pageIndex - 1) * pageSize, pageSize);

                return Db.Query<PickUpDTO>(sql).AsList();
            }
        }

        public List<PickUpDTO> PlayTogetherListByPageIndex(PlayTogetherListRequest request, UserInfoEntity currentUser, int pageSize)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"SELECT useinfo.UId,
                                  useinfo.Gender,
                                  useinfo.BirthDate,
                                  useinfo.NickName,
                                  useinfo.HeadPhotoPath,
                                  useinfo.Area,
                                  useinfo.Province,
                                  useinfo.City,
                                  useinfo.Country,
                                  useinfo.Signature,
                                  useinfo.SchoolName,
                                  useinfo.LiveState,
                                  moment.UId as 'MomentUId',
                                  moment.TextContent,
                                  moment.ImgContent,
                                  moment.IsHide,
                                  moment.MomentId,
                                  moment.HidingNickName,
                                  moment.PlayTypeTag,
                                  moment.StateType,
                                  moment.CreateTime
                           FROM  bingo_Moment moment 
                           Inner Join bingo_UserInfo useinfo on useinfo.UId=moment.UId
                           Where moment.IsDelete=0 and moment.IsReport=0 and moment.SourceFlag=1 ";
                sql += string.Format(" and moment.StateType={0} ", (int)request.StateType);
                if (request.LiveState != LiveStateEnum.Default)
                {
                    sql += string.Format(" and useinfo.LiveState={0} ", (int)request.LiveState);
                }
                if (request.Gender != GenderEnum.Default&&request.Gender != GenderEnum.All)
                {
                    sql += string.Format(" and useinfo.Gender={0} ", (int)request.Gender);
                }
                if (request.LocationType == LocationTypeEnum.RecieveProvince)
                {
                    sql += string.Format(" and useinfo.Province !='' and  useinfo.Province is not null and useinfo.Country={0} and  useinfo.Province={1}", currentUser.Country, currentUser.Province);
                }
                if (request.LocationType == LocationTypeEnum.OnlyCommonCity)
                {
                    sql += string.Format(" and useinfo.Province !='' and  useinfo.Province is not null and useinfo.Country={0} and  useinfo.Province={1} and  useinfo.City={2}", currentUser.Country, currentUser.Province, currentUser.City);
                }
                if (request.AgeRangeList.NotEmpty()&&!request.AgeRangeList.Exists(a=>a.Equals(AgeRangeEnum.Default)))
                {
                    
                    foreach(AgeRangeEnum ageItem in request.AgeRangeList)
                    {
                        switch (ageItem)
                        {
                            case AgeRangeEnum.Before80:
                                sql += "and useinfo.BirthDate<'1980-01-01' ";
                                break;
                            case AgeRangeEnum.After80:
                                sql += "and useinfo.BirthDate>'1980-01-01' and useinfo.BirthDate<'1985-01-01' ";
                                break;
                            case AgeRangeEnum.After85:
                                sql += "and useinfo.BirthDate>'1985-01-01' and useinfo.BirthDate<'1990-01-01' ";
                                break;
                            case AgeRangeEnum.After90:
                                sql += "and useinfo.BirthDate>'1990-01-01' and useinfo.BirthDate<'1995-01-01' ";
                                break;
                            case AgeRangeEnum.After95:
                                sql += "and useinfo.BirthDate>'1995-01-01' and useinfo.BirthDate<'2000-01-01' ";
                                break;
                            case AgeRangeEnum.After00:
                                sql += "and useinfo.BirthDate>'2000-01-01' and useinfo.BirthDate<'2005-01-01' ";
                                break;
                            case AgeRangeEnum.After05:
                                sql += "and useinfo.BirthDate>'2005-01-01' ";
                                break;
                        }
                    }
                }
                sql += string.Format(" Order by moment.CreateTime desc  OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", (request.PageIndex - 1) * pageSize, pageSize);

                return Db.Query<PickUpDTO>(sql).AsList();
            }
        }


        public List<PickUpDTO> AttentionListByPageIndex(long uId, int pageIndex, int pageSize)
        {
            var sql = @"SELECT * FROM (
                                      SELECT moment.MomentId,
                                             useinfo.UId,
                                             useinfo.Gender,
                                             useinfo.BirthDate,
                                             useinfo.NickName,
                                             useinfo.HeadPhotoPath,
                                             moment.TextContent,
                                             moment.ImgContent,
                                             moment.CreateTime
                                      FROM dbo.bingo_Attention attention 
                                      Inner Join bingo_Moment moment on moment.UId= attention.PartnerUId
                                      Inner Join bingo_UserInfo useinfo on useinfo.UId=attention.PartnerUId
                                      Where attention.UId=@UId and moment.IsDelete=0 and moment.IsHide=0 and moment.SourceFlag=0 and moment.CreateTime>=attention.CreateTime
                                      
                                      Union
                                      
                                      SELECT moment.MomentId,
                                             useinfo.UId,
                                             useinfo.Gender,
                                             useinfo.BirthDate,
                                             useinfo.NickName,
                                             useinfo.HeadPhotoPath,
                                             moment.TextContent,
                                             moment.ImgContent,
                                             moment.CreateTime
                                      FROM dbo.bingo_Attention attention 
                                      Inner Join bingo_Moment moment on moment.MomentId= attention.AttentionMomentId
                                      Inner Join bingo_UserInfo useinfo on useinfo.UId=attention.PartnerUId
                                      Where attention.UId=@UId and moment.IsDelete=0 and moment.IsHide=0 and moment.SourceFlag=0 ) temp
                        Order by temp.CreateTime desc 
                        OFFSET @OFFSETCount ROWS
                        FETCH NEXT @FETCHCount ROWS ONLY ";

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
                            FROM dbo.bingo_PickUp pick  ";

                switch (pickType)
                {
                    case PickUpTypeEnum.IsDelete:
                        sql0 += "Where PickUpUId=@UId and  IsPartnerDelete=1 ";
                        break;
                    case PickUpTypeEnum.NoDelete:
                        sql0 += "Where PickUpUId=@UId and  IsPartnerDelete=0 ";
                        break;
                    case PickUpTypeEnum.HasDiscuss:
                        sql0 += @"inner Join bingo_Discuss discuss on pick.PickUpId= discuss.PickUpId
                                  Where pick.PickUpUId = @UId ";
                        break;
                    case PickUpTypeEnum.NoDiscuss:
                        sql0 += @"Left Join bingo_Discuss discuss on pick.PickUpId= discuss.PickUpId
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
                          FROM dbo.bingo_PickUp pick
                          Inner join bingo_Discuss dis on pick.PickUpId=dis.PickUpId
                          Where PickUpUId=@UId";
            using (var Db = GetDbConnection())
            {
                return Db.Query<PickUpEntity>(sql,new { UId = uId }).AsList();
            }
        }

        public Tuple<List<UserInfoEntity>,int> GetSimulateUserList(int pageIndex, int pageSize, long uId, string nickName, GenderEnum gender, long creater, DateTime? startDateTime, DateTime? endCreateTime)
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

                int count = Db.Query<UserInfoEntity>(sql.ToString()).AsList().Count;

                sql.AppendFormat(" order by CreateTime desc OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (pageIndex - 1) * pageSize, pageSize);

                var list= Db.Query<UserInfoEntity>(sql.ToString()).AsList();

                return new Tuple<List<UserInfoEntity>, int>(list, count);
            }
        }

        public Tuple<List<UserInfoEntity>, int> GetRealUserList(int pageIndex, int pageSize, long uId, string nickName, string openId, GenderEnum gender, DateTime? startDateTime, DateTime? endCreateTime)
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

                int count = Db.Query<UserInfoEntity>(sql.ToString()).AsList().Count;

                sql.AppendFormat(" order by CreateTime desc OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (pageIndex - 1) * pageSize, pageSize);

                var list = Db.Query<UserInfoEntity>(sql.ToString()).AsList();

                return new Tuple<List<UserInfoEntity>, int>(list, count);
            }
        }

        public Tuple<List<MomentEntity>, int> GetMomentList(int pageIndex, int pageSize, long uId, MomentStateEnum state, DateTime? startDateTime, DateTime? endCreateTime)
        {
            using (var Db = GetDbConnection())
            {
                var sql = new StringBuilder("SELECT DISTINCT moment.MomentId,moment.UId,moment.TextContent,moment.ImgContent,moment.IsDelete,moment.IsReport,moment.ReplyCount,moment.CreateTime,moment.UpdateTime FROM dbo.bingo_Moment  moment");
               
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
                        sql.Append(" Inner Join dbo.bingo_PickUp pick on pick.MomentId=moment.MomentId" +
                                   " Inner Join dbo.bingo_Discuss disc on pick.PickUpId=disc.PickUpId where 1=1");
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
                var sql = new StringBuilder("SELECT pick.PickUpId,pick.MomentId,pick.MomentUId,pick.PickUpUId,pick.IsUserDelete,pick.IsPartnerDelete,pick.CreateTime,pick.UpdateTime FROM dbo.bingo_PickUp  pick ");

                switch (state)
                {
                    case MomentPickUpEnum.NoDiscuss:
                        sql.AppendFormat("Left Join dbo.bingo_Discuss disc on pick.PickUpId=disc.PickUpId where disc.PickUpId is null and MomentId={0} ", momentId.ToString());
                        break;
                    case MomentPickUpEnum.HasDiscuss:
                        sql.AppendFormat("Inner Join dbo.bingo_Discuss disc on pick.PickUpId=disc.PickUpId and MomentId={0} ", momentId.ToString());
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
                          FROM dbo.bingo_PickUp pick
                          Inner join bingo_Discuss dis on pick.PickUpId=dis.PickUpId
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
				            From  (    
                                   --我捡起了别人发布的动态，对应互动内容
                                   SELECT pick.PickUpId,
                                         pick.MomentId,
                                         moment.IsHide,
                                         moment.HidingNickName,
				          				 dis1Temp.CreateTime,
                                         dis1Temp.DiscussContent as 'TextContent',
                                         online.Latitude,
                                         online.Longitude,
                                         online.IsOnLine,
                                         online.LastOnLineTime,
				          			     us.UId,
				          				 us.NickName,
                                         us.Gender,
				          			     us.HeadPhotoPath
                                    FROM dbo.bingo_PickUp pick
                                    Inner join (Select row_number() over(partition by dis1.PickUpId order by dis1.CreateTime desc) as rownum,
				          		                     dis1.PickUpId,
				          							 dis1.CreateTime,
				          		                     dis1.DiscussContent
				          		              From bingo_Discuss dis1) dis1Temp
				          				on dis1Temp.PickUpId=pick.PickUpId and dis1Temp.rownum=1
				          		  Inner join bingo_UserInfo us on us.UId=pick.MomentUId
                                  Inner join bingo_Moment moment on moment.MomentId=pick.MomentId
                                  Inner join dbo.hub_OnLineUserHub online on online.UId=pick.MomentUId
                                  Where pick.PickUpUId=@UId and pick.IsPartnerDelete=0
                          
				          		  Union
                          
                                  --我自己发布的动态被被别人评论，对应互动内容
				          		  SELECT pick.PickUpId,
                                         pick.MomentId,
                                         pick.IsHide,
                                         pick.HidingNickName,
				          				 dis2Temp.CreateTime,
                                         dis2Temp.DiscussContent,
                                         online.Latitude,
                                         online.Longitude,
                                         online.IsOnLine,
                                         online.LastOnLineTime,
				          				 us.UId,
				          			     us.NickName,
                                         us.Gender,
				          			     us.HeadPhotoPath
                                    FROM dbo.bingo_PickUp pick
                                    Inner join (Select row_number() over(partition by dis2.PickUpId order by dis2.CreateTime desc) as rownum,
				          		                     dis2.PickUpId,
				          							 dis2.CreateTime,
				          		                     dis2.DiscussContent
				          		              From bingo_Discuss dis2) dis2Temp
				          				on dis2Temp.PickUpId=pick.PickUpId and dis2Temp.rownum=1
				          		  Inner join bingo_UserInfo us on us.UId=pick.PickUpUId
                                  Inner join bingo_Moment moment on moment.MomentId=pick.MomentId
                                  Inner join dbo.hub_OnLineUserHub online on online.UId=pick.PickUpUId
                                  Where pick.MomentUId=@UId and pick.IsUserDelete=0 and pick.MomentUId!=pick.PickUpUId
                                 )temp
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

        public List<MomentEntity> GetMomentByPageIndex(long uId, int pageIndex, int pageSize,bool isFilterHide)
        {
            string sql;
            if (isFilterHide)
            {
                sql = SELECT_MomentEntity + @" Where UId=@UId and IsDelete=0 and IsHide=0
                                               Order by CreateTime desc 
                                               OFFSET @OFFSETCount ROWS 
                                               FETCH NEXT @FETCHCount ROWS ONLY";
            }
            else
            {
                sql = SELECT_MomentEntity + @" Where UId=@UId and IsDelete=0
                                               Order by CreateTime desc 
                                               OFFSET @OFFSETCount ROWS 
                                               FETCH NEXT @FETCHCount ROWS ONLY";
            }
            using (var Db = GetDbConnection())
            {
                return Db.Query<MomentEntity>(sql, new 
                { 
                    UId = uId, 
                    OFFSETCount = (pageIndex - 1) * pageSize, 
                    FETCHCount = pageSize
                }).AsList();
            }
        }

        public List<MomentEntity> GetRecentImgMomentList(long uId)
        {
            var sql = SELECT_MomentEntity + @" Where UId=@UId and IsDelete=0 and ImgContent!='' and ImgContent is not null and IsHide=0
                                               Order by CreateTime desc 
                                               OFFSET 0 ROWS 
                                               FETCH NEXT 3 ROWS ONLY";
            using (var Db = GetDbConnection())
            {
                return Db.Query<MomentEntity>(sql, new { UId = uId}).AsList();
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

        public List<CollectDTO> CollectListByUId(long uId, int pageIndex, int pageSize)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"SELECT collect.CollectId
                              ,moment.UId
                              ,moment.MomentId
                              ,moment.IsHide
                              ,moment.HidingNickName
                              ,moment.TextContent
                        	  ,moment.ImgContent
                              ,collect.PickUpId
                              ,collect.CreateTime
                              ,us.NickName
                              ,us.HeadPhotoPath
                              ,us.Gender
                              ,online.Latitude
                              ,online.Longitude
                              ,online.IsOnLine
                              ,online.LastOnLineTime
                        FROM dbo.bingo_Collect collect
                        Inner Join dbo.bingo_Moment moment On collect.MomentId=moment.MomentId
                        Inner Join dbo.bingo_UserInfo us On us.UId=moment.UId
                        Inner join dbo.hub_OnLineUserHub online on online.UId=moment.UId
                        Where collect.UId=@UId
                        Order by collect.CreateTime desc 
                        OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";
                return Db.Query<CollectDTO>(sql, new { UId = uId, Skip = (pageIndex - 1) * pageSize, Take = pageSize }).AsList();
            }
        }

        public List<MomentEntity> GetMomentList(long uId,int pickUpCount, GenderEnum gender,MomentTypeEnum momentType, MomentSourceEnum sourceEnum)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"SELECT top(10)
                              moment.MomentId
                              ,moment.UId
                              ,moment.TextContent
                              ,moment.ImgContent
                              ,moment.IsDelete
                              ,moment.IsReport
                              ,moment.ReplyCount
                              ,moment.CreateTime
                              ,moment.IsHide
                              ,moment.HidingNickName
                              ,moment.UpdateTime
                          FROM dbo.bingo_Moment moment
                          Left join dbo.bingo_PickUp pick
                          ON moment.MomentId=pick.MomentId and pick.PickUpUId=@UId
                          Left join dbo.bingo_Attention attention
                          ON moment.UId=attention.PartnerUId and attention.UId=@UId
                          Inner join dbo.bingo_UserInfo us On us.UId=moment.UId
                          Where moment.UId!=@UId  
                            and moment.CreateTime<@CreateTime 
                            and pick.MomentId is Null 
                            and attention.AttentionId is Null 
                            and moment.ReplyCount<=@PickUpCount 
                            and moment.IsReport=0 
                            and moment.IsDelete=0
                            and moment.SourceFlag=@SourceFlag
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
                return Db.Query<MomentEntity>(sql,new 
                { 
                    UId = uId, 
                    PickUpCount = pickUpCount, 
                    Gender = gender , 
                    CreateTime =DateTime.Now,
                    SourceFlag= sourceEnum
                }).AsList();
            }
        }

        public List<MomentEntity> GetMomentList(long uId, int pickUpCount, GenderEnum gender,int minAge,int maxAge)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"SELECT top (9)  moment.MomentId
                              ,moment.UId
                              ,moment.TextContent
                              ,moment.ImgContent
                              ,moment.IsDelete
                              ,moment.IsReport
                              ,moment.ReplyCount
                              ,moment.CreateTime
                              ,moment.IsHide
                              ,moment.HidingNickName
                              ,moment.UpdateTime
                          FROM dbo.bingo_Moment moment
                          Left join dbo.bingo_PickUp pick
                          ON moment.MomentId=pick.MomentId and pick.PickUpUId=@UId
                          Left join dbo.bingo_Attention attention
                          ON moment.UId=attention.PartnerUId and attention.UId=@UId
                          Inner join dbo.bingo_UserInfo us On us.UId=moment.UId
                          Where moment.UId!=@UId  
                            and moment.CreateTime<@CreateTime 
                            and pick.MomentId is Null 
                            and attention.AttentionId is Null 
                            and moment.ReplyCount<=@PickUpCount 
                            and moment.IsReport=0 
                            and moment.IsDelete=0
                            and (moment.ImgContent is Null or moment.ImgContent='' )";

                switch (gender)
                {
                    case GenderEnum.Man:
                        sql += " and us.Gender=1 ";
                        break;
                    case GenderEnum.Woman:
                        sql += " and us.Gender=2 ";
                        break;
                    case GenderEnum.All:
                        break;
                }
                if (minAge > 0&& minAge<maxAge)
                {
                    string minAgeStr = DateTime.Now.AddYears(-minAge).ToString("yyyy-MM-dd");
                    sql +=string.Format(" and us.BirthDate<'{0}' ", minAgeStr);
                }
                if (maxAge > 0&&maxAge<100&&maxAge>minAge)
                {
                    string maxAgeStr = DateTime.Now.AddYears(-maxAge).ToString("yyyy-MM-dd");
                    sql += string.Format(" and us.BirthDate>'{0}' ", maxAgeStr);
                }

                sql += " Order by moment.CreateTime desc ,moment.ReplyCount ";

                return Db.Query<MomentEntity>(sql, new
                {
                    UId = uId,
                    PickUpCount = pickUpCount,
                    Gender = gender,
                    CreateTime = DateTime.Now
                }).AsList();
            }
        }

        public List<MomentEntity> GetPlayMoments(long uId, int pickUpCount, GenderEnum gender, StateTypeEnum stateType)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"SELECT top (20) 
                               moment.MomentId
                              ,moment.UId
                              ,moment.TextContent
                              ,moment.ImgContent
                              ,moment.IsDelete
                              ,moment.IsReport
                              ,moment.ReplyCount
                              ,moment.CreateTime
                              ,moment.IsHide
                              ,moment.HidingNickName
                              ,moment.UpdateTime
                          FROM dbo.bingo_Moment moment
                          Left join dbo.bingo_PickUp pick
                          ON moment.MomentId=pick.MomentId and pick.PickUpUId=@UId
                          Inner join dbo.bingo_UserInfo us On us.UId=moment.UId
                          Where moment.UId!=@UId  
                            and moment.CreateTime<@CreateTime 
                            and pick.MomentId is Null 
                            and moment.ReplyCount<=@PickUpCount 
                            and moment.IsReport=0 
                            and moment.IsDelete=0
                            and moment.SourceFlag=1
                            and moment.StateType=@StateType
                            and us.Gender!=@Gender
                         Order by moment.CreateTime desc ,moment.ReplyCount ";
                return Db.Query<MomentEntity>(sql, new
                {
                    UId = uId,
                    PickUpCount = pickUpCount,
                    Gender = gender,
                    CreateTime = DateTime.Now,
                    StateType = stateType
                }).AsList();
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

        public List<UserTagEntity> GetUserTagListByUId(long uId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = SELECT_UserTagEntity + @" Where UId=@UId ";
                return Db.Query<UserTagEntity>(sql, new { UId = uId }).AsList();
            }
        }

        public int UnReadCount(Guid pickUpId, long uId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"SELECT Count(0) FROM dbo.bingo_Discuss  Where PickUpId=@PickUpId and UId!=@UId and HasRead=0";
                return Db.QueryFirstOrDefault<int>(sql, new { PickUpId = pickUpId, UId = uId });
            }
        }

        public int UnReadTotalCount(long uId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"Select Count(0) 
                               From dbo.bingo_PickUp pic
                               Inner Join dbo.bingo_Discuss dis
                               On pic.PickUpId=dis.PickUpId 
                               Where ((pic.MomentUId=@UId and pic.IsUserDelete=0) or (pic.PickUpUId=@UId and pic.IsPartnerDelete=0 ))and dis.UId!=@UId and HasRead=0";
                return Db.QueryFirstOrDefault<int>(sql, new {UId = uId });
            }
        }

        public int UnReadAttentionMomentCount(long uId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"Select Count(0) 
                               From dbo.bingo_Moment moment
                               Inner Join dbo.bingo_Attention attention 
                               On moment.UId=attention.PartnerUId 
                               Inner Join dbo.hub_OnLineUserHub userInfo
                               On userInfo.UId=attention.UId 
                               Where (userInfo.LastScanMomentTime is null or  userInfo.LastScanMomentTime<moment.CreateTime ) and userInfo.UId=@UId and moment.IsHide=0 and moment.SourceFlag=0 ";
                return Db.QueryFirstOrDefault<int>(sql, new { UId = uId });
            }
        }

        public bool UpdatePickCount(Guid momentId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.bingo_Moment
                               SET ReplyCount =ReplyCount+1
                                  ,UpdateTime = @UpdateTime
                               WHERE MomentId=@MomentId";
                return Db.Execute(sql, new { UpdateTime =DateTime.Now, MomentId = momentId }) > 0;
            }
        }

        public bool UpdateLastScanMomentTime(long uId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.hub_OnLineUserHub
                               SET LastScanMomentTime =@UpdateTime
                                  ,UpdateTime = @UpdateTime
                               WHERE UId=@UId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, UId = uId }) > 0;
            }
        }

        public bool UpdateUserTotalCoin(long coinId,long uId,int changeValue)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.bingo_Coin
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
                string sql = @"UPDATE dbo.bingo_Discuss
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
                string sql = @"UPDATE dbo.bingo_PickUp
                               SET IsPickUpDelete =1,
                                   UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                return Db.Execute(sql, new {UpdateTime = DateTime.Now, PickUpId = pickUpId}) > 0;
            }
        }

        public bool UpdateAllPickDeleteByUId(long uid)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.bingo_PickUp
                               SET IsPickUpDelete =1,
                                   UpdateTime = @UpdateTime
                               Where PickUpUId=@UId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, UId = uid }) > 0;
            }
        }

        public bool UpdatePickDeleteTime(Guid pickUpId,DateTime updateTime, bool isUserDelete)
        {
            using (var Db = GetDbConnection())
            {
                string sql;
                if (isUserDelete)
                {
                    sql = @"UPDATE dbo.bingo_PickUp
                               SET IsUserDelete =1,
                                   UserLastDeleteTime = @UpdateTime,
                                   UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                }
                else
                {
                    sql = @"UPDATE dbo.bingo_PickUp
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
                string sql = @"UPDATE dbo.bingo_Moment
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
                string sql = @"UPDATE dbo.bingo_PickUp
                               SET IsUserDelete =0
                                  ,UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, PickUpId = pickUpId }) > 0;
            }
        }

        public bool UpdateHiding(Guid pickUpId,bool isHide,string hidingNickName)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.bingo_PickUp
                               SET IsHide =@IsHide
                                  ,HidingNickName =@HidingNickName
                                  ,UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, PickUpId = pickUpId, IsHide=isHide, HidingNickName = hidingNickName }) > 0;
            }
        }

        public bool UpdatePickUpPartnerDelete(Guid pickUpId)
        {
            using (var Db = GetDbConnection())
            {
                //同步创建时间为当前
                string sql = @"UPDATE dbo.bingo_PickUp
                               SET IsPickUpDelete =0,
                                   IsPartnerDelete =0,
                                   CreateTime = @UpdateTime,
                                   UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, PickUpId = pickUpId }) > 0;
            }
        }

        public bool UpdateMomentDelete(Guid momentId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.bingo_Moment
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
                string sql = @"UPDATE dbo.bingo_Moment
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
                string sql = @"UPDATE dbo.bingo_UserInfo
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
                string sql = @"UPDATE dbo.bingo_Moment
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
                string sql = @"UPDATE dbo.bingo_UserInfo
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
                string sql = @"UPDATE dbo.bingo_Discuss
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
                string sql = @"UPDATE dbo.bingo_Discuss
                               SET HasRead =1
                                  ,UpdateTime = @UpdateTime
                               WHERE PickUpId=@PickUpId and UId!=@UId and CreateTime>@ReadTime";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, PickUpId = pickUpId, UId = uId, ReadTime= readTime }) > 0;
            }
        }

        public bool UpdateLetterUser(UserInfoEntity userEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.bingo_UserInfo
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

        public bool UpdateUserBasicInfo(UserInfoEntity userEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.bingo_UserInfo
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

        public bool UpdateLastLoginTime(UserInfoEntity userEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.bingo_UserInfo
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
                var sql = @"UPDATE dbo.bingo_Moment
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
                var sql = @"UPDATE dbo.bingo_Collect
                            SET UpdateTime= @UpdateTime
                          WHERE CollectId=@CollectId";
                return Db.Execute(sql, entity) > 0;
            }
        }
        
        public bool InsertMoment(MomentEntity momentEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.bingo_Moment
                                  (MomentId
                                  ,UId
                                  ,TextContent
                                  ,ImgContent
                                  ,IsDelete
                                  ,IsReport
                                  ,ReplyCount  
                                  ,IsHide
                                  ,SourceFlag
                                  ,PlayTypeTag
                                  ,StateType
                                  ,HidingNickName  
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
                                  ,@IsHide 
                                  ,@SourceFlag 
                                  ,@PlayTypeTag
                                  ,@StateType
                                  ,@HidingNickName
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
                var sql = @"INSERT INTO dbo.bingo_PickUp
                                  (PickUpId
                                  ,MomentId
                                  ,MomentUId
                                  ,FromPage      
                                  ,PickUpUId  
                                  ,IsPickUpDelete  
                                  ,IsPartnerDelete  
                                  ,PartnerLastDeleteTime  
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@PickUpId
                                  ,@MomentId
                                  ,@MomentUId
                                  ,@FromPage
                                  ,@PickUpUId
                                  ,@IsPickUpDelete
                                  ,@IsPartnerDelete
                                  ,@PartnerLastDeleteTime
                                  ,@CreateTime
                                  ,@UpdateTime)";
                return Db.Execute(sql, pickUpEntity) > 0;
            }
        }

        public bool InsertUserTag(UserTagEntity tagEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.bingo_UserTag
                                  (TagId
                                  ,UId
                                  ,TagType
                                  ,Tag      
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@TagId
                                  ,@UId
                                  ,@TagType
                                  ,@Tag
                                  ,@CreateTime
                                  ,@UpdateTime)";
                return Db.Execute(sql, tagEntity) > 0;
            }
        }

        public bool InsertDiscuss(DiscussEntity discussEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.bingo_Discuss
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

        public bool InsertLetterUser(UserInfoEntity userEntity)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.bingo_UserInfo
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
                var sql = @"INSERT INTO dbo.bingo_Coin
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
                var sql = @"INSERT INTO dbo.bingo_Attention
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
                var sql = @"INSERT INTO dbo.bingo_CoinDetail
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
                var sql = @"INSERT INTO dbo.bingo_Collect
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
                string sql = @"UPDATE dbo.bingo_PickUp
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
                string sql = @"UPDATE dbo.bingo_PickUp
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
                var sql = @"Delete dbo.bingo_Discuss Where PickUpId=@PickUpId";
                return Db.Execute(sql, new { PickUpId= pickUpId }) > 0;
            }
        }

        public bool DeleteMoment(Guid momentId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.bingo_Moment Where MomentId=@MomentId";
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
                var sql = @"Delete dbo.bingo_Collect Where CollectId=@CollectId";
                return Db.Execute(sql, new { CollectId = collectId }) > 0;
            }
        }

        public bool DeleteAttention(long uId,long partnerUId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.bingo_Attention Where UId=@UId and PartnerUId=@PartnerUId";
                return Db.Execute(sql, new { UId = uId, PartnerUId= partnerUId}) > 0;
            }
        }

        public bool DeleteAllCollect(long uId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.bingo_Collect Where UId=@UId";
                return Db.Execute(sql, new { UId = uId }) > 0;
            }
        }

        public bool DeleteUserTag(Guid tagId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Delete dbo.bingo_UserTag Where TagId=@TagId";
                return Db.Execute(sql, new { TagId = tagId }) > 0;
            }
        }
    }
}
