using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Future.Model.Utils
{
    public class PageResult<T>
    {
        public PageResult(List<T> list,int count=0)
        {
            if(list!=null&& list.Any())
            {
                rows = list;

                if (count > 0)
                {
                    total = count;
                }
                else
                {
                    total = rows.Count();
                }
            }
        }
        public int total { get; set; }

        public List<T> rows { get; set; }
    }
}
