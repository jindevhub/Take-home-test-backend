using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Take_home_test_backend.Models;


namespace Take_home_test_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly DataService _dataService;

        public InvoicesController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("{invoiceNumber}")]
        public ActionResult<Invoice> GetInvoiceByInvoiceNumber(string invoiceNumber)
        {
            var invoice = _dataService.Invoices.FirstOrDefault(i => i.InvoiceNumber == invoiceNumber);
            if (invoice == null)
            {
                return NotFound();
            }
            return Ok(invoice);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Invoice>> GetInvoices([FromQuery] string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Ok(_dataService.Invoices);
            }

            var invoices = _dataService.Invoices
                .Where(invoice => _dataService.Clients.Any(client => client.ClientId == invoice.ClientId && client.Name == name))
                .ToList();

            if (invoices.Count == 0)
            {
                return NotFound();
            }

            return Ok(invoices);
        }

        [HttpPost]
        public ActionResult<Invoice> AddInvoice([FromBody] InvoiceRequest request)
        {
            int maxInvoiceNumber = _dataService.Invoices.Select(i => int.Parse(i.InvoiceNumber.Split('-')[1])).Max();

            string nextInvoiceNumber = $"INV-{maxInvoiceNumber + 1}";

            var client = _dataService.Clients.FirstOrDefault(c => c.Name == request.Name);
            if (client == null)
            {
                return BadRequest("Client not found");
            }

            if (request.Status != "Paid" && request.Status != "Pending" && request.Status != "Overdue")
            {
                return BadRequest("Invalid status");
            }

            var invoice = new Invoice
            {
                InvoiceNumber = nextInvoiceNumber,
                ClientId = client.ClientId,
                DueDate = request.DueDate,
                LineItems = request.LineItems,
                Status = request.Status
            };

            _dataService.Invoices.Add(invoice);
            return CreatedAtAction(nameof(GetInvoiceByInvoiceNumber), new { invoiceNumber = invoice.InvoiceNumber }, invoice);
        }
    }

    public class InvoiceRequest
    {
        public required string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public List<LineItem>? LineItems { get; set; }
        public string Status { get; set; } = "Pending";
    }
}