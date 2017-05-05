namespace UniStore.Services
{
    using Data.UnitOfWork;

    public class BaseService
    {
        protected IUniStoreContext Context;

        public BaseService(IUniStoreContext context)
        {
            this.Context = context;
        }
    }
}