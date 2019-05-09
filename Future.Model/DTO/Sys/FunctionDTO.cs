using Future.Model.Entity.Sys;
using Infrastructure;
using System.Collections.Generic;

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
