namespace Take_home_test_backend.Models
{
    public class Invoice
    {
        public required string InvoiceNumber { get; set; }
        public required string ClientId { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = "Pending";
        public List<LineItem>? LineItems { get; set; }
    }

    public class LineItem
    {
        public required string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}