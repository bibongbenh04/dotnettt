
namespace SportStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private readonly StoreDbContext context;
        public EFStoreRepository(StoreDbContext ctx)
        {
            context = ctx;
        }

        IQueryable<Product> IStoreRepository.GetProducts => context.Products;

       
    }
}
