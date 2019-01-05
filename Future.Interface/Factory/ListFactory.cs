using Future.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Future.Interface
{
    public class ListFactory : ICtrlFactory
    {
        public string FactoryName { get; set; }

        public IInsert Insert
        {
            get { return SingletonProvider<ListQueryService>.Instance; }
        }

        public IDelete Delete => throw new NotImplementedException();

        public IUpdate Update => throw new NotImplementedException();

        public IQuery Query => throw new NotImplementedException();

        public IIntlBookingValidation BookingValidation
        {
            get { return SingletonProvider<IntlNewVendorsBookingValidation>.Instance; }
        }

    }
}
