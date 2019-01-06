using Future.Model.Entity.Sys;
using Future.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Future.Model.DTO.Sys
{
    public class FunctionDTO: Function
    {
        public string FuncType
        {
            get
            {
                return EnumFuncType.ToDescription();
            }
                
        }
        public List<FunctionDTO> Children { get; set; }
    }
}
