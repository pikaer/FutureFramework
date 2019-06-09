using System;
using System.Collections.Generic;
using System.Text;

namespace Future.Service
{
    public class LetterService
    {
        private readonly LetterRepository todayDal = SingletonProvider<LetterRepository>.Instance;
    }
}
