using Future.Model.DTO.Sys;
using Future.Model.Entity.Sys;
using Future.Model.Enum.Sys;
using Future.Model.Utils;
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

        public ResponseContext<Function> GetFunctionByFuncId(int id)
        {
            var func= sysDal.GetFunctionByFuncId(id);
            return new ResponseContext<Function>(func);
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
                var moduleList = GetFunctionsByParentId(module.Id);
                if(moduleList != null&& moduleList.Any())
                {
                    foreach(var menu in moduleList)
                    {
                        var menuList = GetFunctionsByParentId(menu.Id);
                        if(menuList!=null&& menuList.Any())
                        {
                            foreach(var page in menuList)
                            {
                                var pageList = GetFunctionsByParentId(page.Id);
                                if(pageList!=null&& pageList.Any())
                                {
                                    foreach(var button in pageList)
                                    {
                                        var buttonList = GetFunctionsByParentId(button.Id);
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

        public ResponseContext<bool> AddEqFunc(Function req)
        {
            req.Text = "新增项";
            req.CreateTime = DateTime.Now;
            var success= sysDal.AddFunction(req);
            return new ResponseContext<bool>(success);
        }

        public ResponseContext<bool> UpdateFunc(Function req)
        {
            req.ModifyTime = DateTime.Now;

            var success = sysDal.UpdateFunc(req);

            return new ResponseContext<bool>(success);
        }

        public ResponseContext<bool> AddSubFunc(Function req)
        {
            var dto = new Function()
            {
                ParentId= req.Id,
                Text = "新增项",
                IconCls= req.IconCls,
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
            var success= sysDal.AddFunction(dto);

            return new ResponseContext<bool>(success);
        }
        
        public ResponseContext<bool> DeleteFuncs(int id)
        {
            var itemList = GetFunctionsByParentId(id);
            foreach (var item in itemList)
            {
                if(item.ParentId.HasValue&& item.ParentId!=null)
                {
                    var item1List = GetFunctionsByParentId(item.ParentId.Value);
                    foreach (var item1 in item1List)
                    {
                        sysDal.DeleteFuncByParentId(item1.Id);
                    }
                }
                sysDal.DeleteFuncByParentId(item.Id);
            }
            sysDal.DeleteFuncByParentId(id);
            var success=sysDal.DeleteFuncByFuncId(id);

            return new ResponseContext<bool>(success);
        }
        
    }
}
