using Dapper;
using Future.Model.Entity.Today;
using System.Collections.Generic;

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
        
        public List<ImgGalleryEntity> ImgGalleryList(int pageIndex,int pageSize)
        {
            using (var Db = GetDbConnection())
            {
                var sql = $@"{SELECT_IMGGALLERY} order by CreateTime desc OFFSET @OFFSETCount ROWS FETCH NEXT @TakeCount ROWS ONLY";

                return Db.Query<ImgGalleryEntity>(sql,new { OFFSETCount= (pageIndex - 1)* pageSize, TakeCount=pageSize }).AsList();
            }
        }

        public List<TextGalleryEntity> TextGalleryList(int pageIndex, int pageSize)
        {
            using (var Db = GetDbConnection())
            {
                var sql = $@"{SELECT_TEXTGALLERY} order by CreateTime desc OFFSET @OFFSETCount ROWS FETCH NEXT @TakeCount ROWS ONLY";

                return Db.Query<TextGalleryEntity>(sql, new { OFFSETCount = (pageIndex - 1) * pageSize, TakeCount = pageSize }).AsList();
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

        public int TextGalleryListCount()
        {
            using (var Db = GetDbConnection())
            {
                var sql = "select count(1) from dbo.gallery_TextGallery";

                return Db.QueryFirstOrDefault<int>(sql);
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
    }
}
