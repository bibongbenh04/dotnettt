namespace SportStore.Models
{
    public interface IStoreRepository
    {
        IQueryable<Product> GetProducts { get; }
    }
}
