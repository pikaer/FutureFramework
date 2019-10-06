using Future.Model.DTO.Sys;
using Future.Model.Entity.Sys;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
using System.Collections.Generic;

namespace Future.Service.Interface
{
    public interface ISysBiz
    {
        List<FunctionDTO> GetFunctions();

        ResponseContext<FunctionEntity> GetFunctionByFuncId(int id);

        List<FunctionDTO> GetFunctionsByParentId(int id);

        List<FunctionDTO> GetModules(EnumFuncType type);

        List<FunctionDTO> GetMenus();

        ResponseContext<bool> AddEqFunc(FunctionEntity req);

        ResponseContext<bool> UpdateFunc(FunctionEntity req);

        ResponseContext<bool> AddSubFunc(FunctionEntity req);

        ResponseContext<bool> DeleteFuncs(int id);

        PageResult<LogDTO> GetLogList(int pageIndex, int pageSize);

        PageResult<StaffDTO> GetStaffList(int pageIndex, int pageSize, string staffName, string mobile);

        StaffEntity StaffByMobile(string mobile);

        ResponseContext<bool> AddOrUpdateStaff(StaffEntity request);

        ResponseContext<bool> DeleteStaff(long staffId);
    }
}
