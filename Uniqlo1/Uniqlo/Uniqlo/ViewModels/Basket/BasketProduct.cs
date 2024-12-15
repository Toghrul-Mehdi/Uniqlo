namespace Uniqlo.ViewModel.Basket
{
    public class BasketProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Discount { get; set; }
        public decimal SellPrice { get; set; }
        public string ImageUrl { get; set; }
    }
}
