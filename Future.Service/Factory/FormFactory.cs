using Future.Service.Interface;
using Future.Service.Service.FormService;
using Future.Utility;

namespace Future.Service.Factory
{
    public class FormFactory: ICtrlFactory
    {
        public string FactoryName { get; set; }

        public IInsert Insert
        {
            get { return SingletonProvider<FormInsertService>.Instance; }
        }

        public IDelete Delete
        {
            get { return SingletonProvider<FormDeleteService>.Instance; }
        }

        public IUpdate Update
        {
            get { return SingletonProvider<FormUpdateService>.Instance; }
        }

        public IQuery Query
        {
            get { return SingletonProvider<FormQueryService>.Instance; }
        }
    }
}
