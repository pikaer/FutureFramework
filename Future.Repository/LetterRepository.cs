using System;
using System.Collections.Generic;
using System.Text;

namespace Future.Repository
{
    public class LetterRepository:BaseRepository
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.LetterService;
        }
    }
}
