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
                Rows = list;

                if (count > 0)
                {
                    Total = count;
                }
                else
                {
                    Total = Rows.Count();
                }
            }
        }

        public PageResult()
        {
            Total = 0;
            Rows = null;
        }

        public int Total { get; set; }

        public List<T> Rows { get; set; }
    }
}
