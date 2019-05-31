using Dapper;
using Future.Model.Entity.Today;
using System;
using System.Collections.Generic;
using System.Text;

namespace Future.Repository
{
    public class TodayRepository : BaseRepository
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.TodayHappy;
        }

        private readonly string SELECT_IMGGALLERY = "SELECT ImgId,ImgName,ShortUrl,ImgSource,Author,Remark ,CreateUserId,ModifyUserId,CreateTime,ModifyTime FROM dbo.gallery_ImgGallery ";

        private readonly string SELECT_TEXTGALLERY = "SELECT TextId ,TextSource ,TextContent,Author ,Remark,CreateUserId ,ModifyUserId,CreateTime,ModifyTime FROM dbo.gallery_TextGallery ";

        private readonly string SELECT_HOMEINFO = "SELECT HomeInfoId,DisplayStartDateTime,DisplayEndDateTime,Remark ,CreateUserId,ModifyUserId,CreateTime,ModifyTime FROM dbo.home_HomeInfo ";

        private readonly string SELECT_HOMETEXT = "SELECT HomeTextId,HomeInfoId,TextId,SortNum ,CreateUserId,CreateTime FROM dbo.home_HomeText ";

        private readonly string SELECT_HOMEIMG = "SELECT HomeImgId,HomeInfoId,ImgId,SortNum,CreateUserId ,CreateTime FROM dbo.home_HomeImg ";
        
        public TextGalleryEntity TextGallery(long textId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} where TextId={1}", SELECT_TEXTGALLERY, textId);

                return Db.QueryFirstOrDefault<TextGalleryEntity>(sql);
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

        public HomeTextEntity HomeText(long homeTextId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} where HomeTextId={1}", SELECT_HOMETEXT, homeTextId);

                return Db.QueryFirstOrDefault<HomeTextEntity>(sql);
            }
        }

        public HomeImgEntity HomeImg(long homeImgId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} where HomeImgId={1}", SELECT_HOMEIMG, homeImgId);

                return Db.QueryFirstOrDefault<HomeImgEntity>(sql);
            }
        }

        public List<TextGalleryEntity> TextGalleryList(int pageIndex, int pageSize, string textContent, string textSource, long creater, DateTime? startDateTime, DateTime? endCreateTime)
        {
            using (var Db = GetDbConnection())
            {
                var sql = new StringBuilder(SELECT_TEXTGALLERY);
                sql.Append(" where 1=1 ");

                if (!string.IsNullOrWhiteSpace(textContent))
                {
                    sql.AppendFormat("and TextContent like '%{0}%' ", textContent.Trim());
                }

                if (!string.IsNullOrWhiteSpace(textSource))
                {
                    sql.AppendFormat("and TextSource like '%{0}%' ", textSource.Trim());
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

                sql.AppendFormat(" order by CreateTime desc OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (pageIndex - 1) * pageSize, pageSize);
                return Db.Query<TextGalleryEntity>(sql.ToString()).AsList();
            }
        }

        public List<ImgGalleryEntity> ImgGalleryList(int pageIndex, int pageSize, string imgName, string imgSource, long creater, DateTime? startDateTime, DateTime? endCreateTime)
        {
            using (var Db = GetDbConnection())
            {
                var sql = new StringBuilder(SELECT_IMGGALLERY);
                sql.Append(" where 1=1 ");

                if (!string.IsNullOrWhiteSpace(imgName))
                {
                    sql.AppendFormat("and ImgName like '%{0}%' ", imgName.Trim());
                }

                if (!string.IsNullOrWhiteSpace(imgSource))
                {
                    sql.AppendFormat("and ImgSource like '%{0}%' ", imgSource.Trim());
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

                sql.AppendFormat(" order by CreateTime desc OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (pageIndex - 1) * pageSize, pageSize);

                return Db.Query<ImgGalleryEntity>(sql.ToString()).AsList();
            }
        }
        
        public List<HomeInfoEntity> HomeInfoList(int pageIndex, int pageSize,  DateTime? startDateTime, DateTime? endCreateTime)
        {
            using (var Db = GetDbConnection())
            {
                var sql = new StringBuilder(SELECT_HOMEINFO);
                sql.Append(" where 1=1 ");
                
                if (!startDateTime.Equals(new DateTime()))
                {
                    sql.AppendFormat("and DisplayStartDateTime>'{0}' ", startDateTime.Value.ToString());
                }

                if (!endCreateTime.Equals(new DateTime()))
                {
                    sql.AppendFormat("and DisplayEndDateTime<'{0}' ", endCreateTime.Value.ToString());
                }

                sql.AppendFormat(" order by CreateTime desc OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (pageIndex - 1) * pageSize, pageSize);
                return Db.Query<HomeInfoEntity>(sql.ToString()).AsList();
            }
        }

        public List<HomeInfoEntity> HomeInfoListByDisplayTime(DateTime dateTime)
        {
            using (var Db = GetDbConnection())
            {
                var sql = SELECT_HOMEINFO + @" Where DisplayStartDateTime<=@DisplayDateTime and @DisplayDateTime<=DisplayEndDateTime";
                return Db.Query<HomeInfoEntity>(sql,new { DisplayDateTime= dateTime }).AsList();
            }
        }

        public List<HomeTextEntity> HomeTextList(long homeInfoId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} Where HomeInfoId={1}", SELECT_HOMETEXT, homeInfoId);

                return Db.Query<HomeTextEntity>(sql).AsList();
            }
        }

        public List<HomeImgEntity> HomeImgList(long homeInfoId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("{0} Where HomeInfoId={1}", SELECT_HOMEIMG, homeInfoId);

                return Db.Query<HomeImgEntity>(sql).AsList();
            }
        }

        public HomeInfoEntity HomeInfo(DateTime dateTime)
        {
            using (var Db = GetDbConnection())
            {
                var sql = $@"{SELECT_HOMEINFO} Where DisplayStartDateTime<@CurrentTime and @CurrentTime<=DisplayEndDateTime";

                return Db.QueryFirstOrDefault<HomeInfoEntity>(sql, new { CurrentTime= dateTime });
            }
        }

        public int ImgGalleryListCount()
        {
            using (var Db = GetDbConnection())
            {
                var sql = "select count(1) from dbo.gallery_ImgGallery";

                return Db.QueryFirstOrDefault<int>(sql);
            }
        }

        public int HomeInfoCount()
        {
            using (var Db = GetDbConnection())
            {
                var sql = "select count(1) from dbo.home_HomeInfo";

                return Db.QueryFirstOrDefault<int>(sql);
            }
        }

        public int HomeTextCount()
        {
            using (var Db = GetDbConnection())
            {
                var sql = "select count(1) from dbo.home_HomeText";

                return Db.QueryFirstOrDefault<int>(sql);
            }
        }

        public int TextGalleryListCount()
        {
            using (var Db = GetDbConnection())
            {
                var sql = "select count(1) from dbo.gallery_TextGallery";

                return Db.QueryFirstOrDefault<int>(sql);
            }
        }
        
        public bool IndertTextGallery(TextGalleryEntity req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.gallery_TextGallery
                                  (TextSource
                                  ,TextContent
                                  ,Author
                                  ,Remark
                                  ,CreateUserId
                                  ,ModifyUserId
                                  ,CreateTime
                                  ,ModifyTime)
                            VALUES
                                  (@TextSource
                                  ,@TextContent
                                  ,@Author
                                  ,@Remark
                                  ,@CreateUserId
                                  ,@ModifyUserId
                                  ,@CreateTime
                                  ,@ModifyTime)";
                return Db.Execute(sql, req) >0;
            }
        }
        
        public bool IndertImageGallery(ImgGalleryEntity req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.gallery_ImgGallery
                                   (ImgName
                                   ,ShortUrl
                                   ,ImgSource
                                   ,Author
                                   ,Remark
                                   ,CreateUserId
                                   ,ModifyUserId
                                   ,CreateTime
                                   ,ModifyTime)
                             VALUES
                                   (@ImgName
                                   ,@ShortUrl
                                   ,@ImgSource
                                   ,@Author
                                   ,@Remark
                                   ,@CreateUserId
                                   ,@ModifyUserId
                                   ,@CreateTime
                                   ,@ModifyTime)";
                return Db.Execute(sql, req) > 0;
            }
        }

        public bool IndertHomeInfo(HomeInfoEntity req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.home_HomeInfo
                                  (DisplayStartDateTime
                                  ,DisplayEndDateTime
                                  ,Remark
                                  ,CreateUserId
                                  ,ModifyUserId
                                  ,CreateTime
                                  ,ModifyTime)
                            VALUES
                                  (@DisplayStartDateTime
                                  ,@DisplayEndDateTime
                                  ,@Remark
                                  ,@CreateUserId
                                  ,@ModifyUserId
                                  ,@CreateTime
                                  ,@ModifyTime)";
                return Db.Execute(sql, req) > 0;
            }
        }

        public bool IndertHomeText(HomeTextEntity req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.home_HomeText
                                  (HomeInfoId
                                  ,TextId
                                  ,SortNum
                                  ,CreateUserId
                                  ,CreateTime)
                            VALUES
                                  (@HomeInfoId
                                  ,@TextId
                                  ,@SortNum
                                  ,@CreateUserId
                                  ,@CreateTime)";
                return Db.Execute(sql, req) > 0;
            }
        }

        public bool IndertHomeImg(HomeImgEntity req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.home_HomeImg
                                  (HomeInfoId
                                  ,ImgId
                                  ,SortNum
                                  ,CreateUserId
                                  ,CreateTime)
                            VALUES
                                  (@HomeInfoId
                                  ,@ImgId
                                  ,@SortNum
                                  ,@CreateUserId
                                  ,@CreateTime)";
                return Db.Execute(sql, req) > 0;
            }
        }

        public bool UpdateTextGallery(TextGalleryEntity req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.gallery_TextGallery
                               SET TextSource = @TextSource
                                  , TextContent = @TextContent
                                  , Author = @Author
                                  , Remark = @Remark
                                  , ModifyUserId = @ModifyUserId
                                  , ModifyTime = @ModifyTime
                             WHERE TextId = @TextId";
                return Db.Execute(sql, req) > 0;
            }
        }

        public bool UpdateImageGallery(ImgGalleryEntity req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.gallery_ImgGallery
                               SET ImgName = @ImgName
                                  , ImgSource = @ImgSource
                                  , Author = @Author
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
        
        public bool UpdateHomeInfo(HomeInfoEntity req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.home_HomeInfo
                               SET DisplayStartDateTime = @DisplayStartDateTime
                                  ,DisplayEndDateTime = @DisplayEndDateTime
                                  ,Remark = @Remark
                                  ,ModifyUserId= @ModifyUserId
                                  ,ModifyTime = @ModifyTime
                             WHERE HomeInfoId=@HomeInfoId";
                return Db.Execute(sql, req) > 0;
            }
        }

        public bool UpdateHomeTextSortNum(HomeTextEntity req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.home_HomeText
                               SET SortNum = @SortNum
                             WHERE HomeTextId=@HomeTextId";
                return Db.Execute(sql, req) > 0;
            }
        }

        public bool UpdateHomeImgSortNum(HomeImgEntity req)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.home_HomeImg
                               SET SortNum = @SortNum
                             WHERE HomeImgId=@HomeImgId";
                return Db.Execute(sql, req) > 0;
            }
        }

        public bool DeleteText(long textId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("DELETE FROM dbo.gallery_TextGallery WHERE TextId={0}", textId);

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

        public bool DeleteHomeInfo(long homeInfoId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("DELETE FROM dbo.home_HomeInfo WHERE HomeInfoId={0}", homeInfoId);

                return Db.Execute(sql) > 0;
            }
        }

        public bool DeleteHomeTextByHomeInfoId(long homeInfoId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("DELETE FROM dbo.home_HomeText WHERE HomeInfoId={0}", homeInfoId);

                return Db.Execute(sql) > 0;
            }
        }

        public bool DeleteHomeTextByHomeTextId(long homeTextId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("DELETE FROM dbo.home_HomeText WHERE HomeTextId={0}", homeTextId);

                return Db.Execute(sql) > 0;
            }
        }

        public bool DeleteHomeImgByHomeInfoId(long homeInfoId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("DELETE FROM dbo.home_HomeImg WHERE HomeInfoId={0}", homeInfoId);

                return Db.Execute(sql) > 0;
            }
        }

        public bool DeleteHomeImgByHomeImgId(long homeImgId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = string.Format("DELETE FROM dbo.home_HomeImg WHERE HomeImgId={0}", homeImgId);

                return Db.Execute(sql) > 0;
            }
        }
    }
}
