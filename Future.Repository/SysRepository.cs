using Dapper;
using Future.Model.DTO.Sys;
using Future.Model.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Future.Repository
{
    public class SysRepository : BaseRepository
    {
        private readonly string SELECT_FUNCTION = "SELECT Id,ParentId,Text,Url,IconCls,EnumFuncType,Remark,CreateTime,ModifyTime,CreateUserId,ModifyUserId FROM dbo.sys_Function";

        private readonly string SELECT_STAFF = "SELECT StaffId, StaffName, Gender, Role, Mobile, Email, PassWord, CreateTime, ModifyTime FROM dbo.sys_Staff ";

        private readonly string SELECT_IMGGALLERY = "SELECT ImgId,ImgName,ShortUrl,Remark ,UseCount, CreateUserId,ModifyUserId,CreateTime,ModifyTime FROM dbo.gallery_ImgGallery ";

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


        public StaffEntity StaffByUId(long userId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = string.Format("{0} where StaffId={1}", SELECT_STAFF, userId);
                return Db.QueryFirstOrDefault<StaffEntity>(sql);
            }
        }

        public StaffEntity StaffByMobile(string mobile)
        {
            using (var Db = GetDbConnection())
            {
                string sql = string.Format("{0} where Mobile='{1}'", SELECT_STAFF, mobile);
                return Db.QueryFirstOrDefault<StaffEntity>(sql);
            }
        }

        public List<StaffEntity> StaffList(int pageIndex, int pageSize, string staffName, string mobile)
        {
            using (var Db = GetDbConnection())
            {
                var sql = new StringBuilder(SELECT_STAFF);
                sql.Append(" where 1=1 ");

                if (!string.IsNullOrWhiteSpace(staffName))
                {
                    sql.AppendFormat("and StaffName like '%{0}%' ", staffName.Trim());
                }

                if (!string.IsNullOrWhiteSpace(mobile))
                {
                    sql.AppendFormat("and Mobile like '%{0}%' ", mobile.Trim());
                }

                sql.AppendFormat(" order by CreateTime desc OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (pageIndex - 1) * pageSize, pageSize);
                return Db.Query<StaffEntity>(sql.ToString()).AsList();
            }
        }
        public ImgGalleryEntity ImgGallery(long imageId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} where ImgId={1}", SELECT_IMGGALLERY, imageId);

                return Db.QueryFirstOrDefault<ImgGalleryEntity>(sql);
            }
        }

        public Tuple<List<ImgGalleryEntity>, int> ImgGalleryList(int pageIndex, int pageSize, string imgName, long creater, DateTime? startDateTime, DateTime? endCreateTime)
        {
            using (var Db = GetDbConnection())
            {
                var sql = new StringBuilder(SELECT_IMGGALLERY);
                sql.Append(" where 1=1 ");

                if (!string.IsNullOrWhiteSpace(imgName))
                {
                    sql.AppendFormat("and ImgName like '%{0}%' ", imgName.Trim());
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

                int count = Db.Query<ImgGalleryEntity>(sql.ToString()).AsList().Count;

                sql.AppendFormat(" order by CreateTime desc OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (pageIndex - 1) * pageSize, pageSize);

                var list = Db.Query<ImgGalleryEntity>(sql.ToString()).AsList();

                return new Tuple<List<ImgGalleryEntity>, int>(list, count);
            }
        }
        
        public List<ImgGalleryEntity> ImgGalleryList()
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} order by UseCount ", SELECT_IMGGALLERY);

                return Db.Query<ImgGalleryEntity>(sql).AsList();
            }
        }
        /// <summary>
        /// 通过FuncId获取Function列表
        /// </summary>
        public FunctionEntity GetFunctionByFuncId(int id)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} where Id={1}", SELECT_FUNCTION, id);
                return Db.QueryFirstOrDefault<FunctionEntity>(sql);
            }
        }
        
        public List<FunctionDTO> GetFunctionDTOByFuncType(int type)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} where EnumFuncType={1}", SELECT_FUNCTION, type);
                return Db.Query<FunctionDTO>(sql).AsList();
            }
        }

        public bool AddFunction(FunctionEntity req)
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
                                           ,@ModifyUserId)";
                return Db.Execute(sql, req)>0;
            }
        }

        public bool IndertImageGallery(ImgGalleryEntity req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.gallery_ImgGallery
                                   (ImgName
                                   ,ShortUrl
                                   ,Remark
                                   ,CreateUserId
                                   ,ModifyUserId
                                   ,CreateTime
                                   ,ModifyTime)
                             VALUES
                                   (@ImgName
                                   ,@ShortUrl
                                   ,@Remark
                                   ,@CreateUserId
                                   ,@ModifyUserId
                                   ,@CreateTime
                                   ,@ModifyTime)";
                return Db.Execute(sql, req) > 0;
            }
        }

        public bool IndertStaff(StaffEntity request)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.sys_Staff
                                  (StaffName
                                  ,Gender
                                  ,Role
                                  ,Mobile
                                  ,Email
                                  ,PassWord
                                  ,CreateTime
                                  ,ModifyTime)
                            VALUES
                                  (@StaffName
                                  ,@Gender
                                  ,@Role
                                  ,@Mobile
                                  ,@Email
                                  ,@PassWord
                                  ,@CreateTime
                                  ,@ModifyTime)";
                return Db.Execute(sql, request) > 0;
            }
        }

        public bool UpdateImageGallery(ImgGalleryEntity req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.gallery_ImgGallery
                               SET ImgName = @ImgName
                                  , Remark = @Remark
                                  , ModifyUserId = @ModifyUserId
                                  , ModifyTime = @ModifyTime
                             WHERE ImgId = @ImgId";
                return Db.Execute(sql, req) > 0;
            }
        }

        public bool UpdateShortUrl(ImgGalleryEntity req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.gallery_ImgGallery
                               SET ShortUrl = @ShortUrl
                                  , ModifyUserId = @ModifyUserId
                                  , ModifyTime = @ModifyTime
                             WHERE ImgId = @ImgId";
                return Db.Execute(sql, req) > 0;
            }
        }

        public bool UpdateImgUseCount(long imgId)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.gallery_ImgGallery
                               SET UseCount =UseCount+1
                                  ,ModifyTime = @UpdateTime
                               WHERE ImgId=@ImgId";
                return Db.Execute(sql, new { UpdateTime = DateTime.Now, ImgId = imgId }) > 0;
            }
        }

        public bool UpdateFunc(FunctionEntity func)
        {
            using (var Db = GetDbConnection())
            {
                string sql = @"UPDATE dbo.sys_Function
                               SET Text =@Text
                                  ,Url= @Url
                                  ,IconCls = @IconCls
                                  ,Remark = @Remark
                                  ,ModifyTime = @ModifyTime
                                  ,ModifyUserId = @ModifyUserId
                             WHERE Id=@Id";
                return Db.Execute(sql, func) > 0;
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
        
        public int StaffListCount()
        {
            using (var Db = GetDbConnection())
            {
                var sql = "select count(1) from dbo.sys_Staff";

                return Db.QueryFirstOrDefault<int>(sql);
            }
        }
        
        public bool UpdateStaff(StaffEntity request)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.sys_Staff
                               SET StaffName = @StaffName
                                  , Gender = @Gender
                                  , Role = @Role
                                  , Mobile = @Mobile
                                  , Email = @Email
                                  , ModifyTime = @ModifyTime
                             WHERE StaffId = @StaffId";
                return Db.Execute(sql, request) > 0;
            }
        }

        public bool DeleteStaff(long staffId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("DELETE FROM dbo.sys_Staff WHERE StaffId={0}", staffId);

                return Db.Execute(sql) > 0;
            }
        }

        public bool DeleteImage(long imgId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("DELETE FROM dbo.gallery_ImgGallery WHERE ImgId={0}", imgId);

                return Db.Execute(sql) > 0;
            }
        }
    }
}
