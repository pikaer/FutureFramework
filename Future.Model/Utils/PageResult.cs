using Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Future.Model.Utils
{
    public class PageResult<T>
    {
        public PageResult(List<T> list,int count=0)
        {
            if(list.NotEmpty())
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

        public PageResult()
        {
            total = 0;
            rows = null;
        }

        public int total { get; set; }

        public List<T> rows { get; set; }
    }
}
