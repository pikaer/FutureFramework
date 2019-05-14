using Future.Model.DTO.Today;
using Future.Model.Utils;
using Future.Repository;
using Infrastructure;
using System.Linq;

namespace Future.Service
{
    public class TodayService
    {
        private readonly TodayRepository todayDal = SingletonProvider<TodayRepository>.Instance;
        private readonly SysRepository sysDal = SingletonProvider<SysRepository>.Instance;
        public PageResult<TextGalleryDTO> GetTextList(int pageIndex, int pageSize)
        {
            var rtn = new PageResult<TextGalleryDTO>();
            var entityList = todayDal.TextGalleryList(pageIndex, pageSize);
            if (entityList.NotEmpty())
            {
                var txtList = entityList.Select(a => new TextGalleryDTO()
                {
                    TextId=a.TextId,
                    TextContent = a.TextContent,
                    TextSource = a.TextSource,
                    Author = a.Author,
                    Remark = a.Remark,
                    CreateUser = GetStaffName(a.CreateUserId),
                    ModifyUser = GetStaffName(a.ModifyUserId),
                    CreateTimeDesc = a.CreateTime.ToString(),
                    ModifyTimeDesc = a.ModifyTime.ToString(),
                }).ToList();
                rtn.rows = txtList;
                rtn.total = todayDal.TextGalleryListCount();
            }
            return rtn;
        }

        public string GetStaffName(long userId)
        {
            if (userId <= 0)
            {
                return string.Empty;
            }
            var entity = sysDal.Staff(userId);
            if (entity == null)
            {
                return string.Empty;
            }
            return entity.StaffName;
        }
    }
}
