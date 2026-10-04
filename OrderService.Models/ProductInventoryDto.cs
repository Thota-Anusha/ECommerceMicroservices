namespace OrderService.Models
{
    public class ProductInventoryDto
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}