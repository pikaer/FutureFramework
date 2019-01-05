using Future.Service.Interface;
using Future.Service.ListService;
using Future.Utility;
using System;

namespace Future.Service.Factory
{
    public class ListFactory : ICtrlFactory
    {
        public string FactoryName { get; set; }

        public IInsert Insert
        {
            get { return SingletonProvider<ListInsertService>.Instance; }
        }

        public IDelete Delete
        {
            get { return SingletonProvider<ListDeleteService>.Instance; }
        }

        public IUpdate Update
        {
            get { return SingletonProvider<ListUpdateService>.Instance; }
        }

        public IQuery Query
        {
            get { return SingletonProvider<ListQueryService>.Instance; }
        }
    }
}
