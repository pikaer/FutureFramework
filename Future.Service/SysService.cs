using Future.Model.DTO.Sys;
using Future.Model.Entity.Sys;
using Future.Model.Enum.Sys;
using Future.Repository;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Future.Service
{
    public class SysService
    {
        private readonly SysRepository sysDal = SingletonProvider<SysRepository>.Instance;

        public List<FunctionDTO> GetFunctions()
        {
            return sysDal.GetFunctions();
        }

        public Function GetFunctionByFuncId(int id)
        {
            return sysDal.GetFunctionByFuncId(id);
        }

        public List<FunctionDTO> GetFunctionsByParentId(int id)
        {
            return sysDal.GetFunctionsByParentId(id);
        }

        public List<FunctionDTO> GetModules(EnumFuncType type)
        {
            return sysDal.GetFunctionDTOByFuncType((int)type);
        }

        public List<FunctionDTO> GetMenus()
        {
            var rtn = sysDal.GetFunctionDTOByFuncType((int)EnumFuncType.Module); ;
            if(rtn==null || !rtn.Any())
            {
                return rtn;
            }

            foreach(var module in rtn)
            {
                var moduleList = GetFunctionsByParentId(module.FuncId);
                if(moduleList != null&& moduleList.Any())
                {
                    foreach(var menu in moduleList)
                    {
                        var menuList = GetFunctionsByParentId(menu.FuncId);
                        if(menuList!=null&& menuList.Any())
                        {
                            foreach(var page in menuList)
                            {
                                var pageList = GetFunctionsByParentId(page.FuncId);
                                if(pageList!=null&& pageList.Any())
                                {
                                    foreach(var button in pageList)
                                    {
                                        var buttonList = GetFunctionsByParentId(button.FuncId);
                                        button.Children = buttonList;
                                    }
                                }
                                page.Children = pageList;
                            }
                        }
                        menu.Children = menuList;
                    }
                }
                module.Children = moduleList;
            }
            return rtn;
        }

        public bool AddEqFunc(Function req)
        {
            req.Text = "新增项";
            req.CreateTime = DateTime.Now;
            return sysDal.AddFunction(req);
        }

        public bool UpdateFunc(Function req)
        {
            return true;
        }

        public bool AddSubFunc(Function req)
        {
            var dto = new Function()
            {
                ParentId= req.FuncId,
                Text = "新增项",
                CreateTime = DateTime.Now
            };
            switch (req.EnumFuncType)
            {
                case EnumFuncType.Module:
                    dto.EnumFuncType = EnumFuncType.Menu;
                    break;
                case EnumFuncType.Menu:
                    dto.EnumFuncType = EnumFuncType.Page;
                    break;
                case EnumFuncType.Page:
                    dto.EnumFuncType = EnumFuncType.Button;
                    break;
            }
            return sysDal.AddFunction(req);
        }
        
        public bool DeleteFuncs(int id)
        {
            var itemList = GetFunctionsByParentId(id);
            foreach (var item in itemList)
            {
                if(item.ParentId.HasValue&& item.ParentId!=null)
                {
                    var item1List = GetFunctionsByParentId(item.ParentId.Value);
                    foreach (var item1 in item1List)
                    {
                        sysDal.DeleteFuncByParentId(item1.FuncId);
                    }
                }
                sysDal.DeleteFuncByParentId(item.FuncId);
            }
            sysDal.DeleteFuncByParentId(id);
            return sysDal.DeleteFuncByFuncId(id);
        }
        
    }
}
