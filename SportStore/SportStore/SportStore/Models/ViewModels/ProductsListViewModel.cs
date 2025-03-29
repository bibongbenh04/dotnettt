namespace SportStore.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();

        public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();
        public string? CurrentCategory { get; set; }
    }
}
