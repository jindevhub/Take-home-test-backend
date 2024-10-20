using System.Text.Json;
using Take_home_test_backend.Models;


namespace Take_home_test_backend
{
    public class DataService
    {
        public virtual List<Invoice> Invoices { get; private set; }
        public virtual List<Client> Clients { get; private set; }

        public DataService()
        {
            Invoices = LoadInvoices() ?? new List<Invoice>();
            Clients = LoadClients() ?? new List<Client>();
        }

        private List<Invoice>? LoadInvoices()
        {
            var json = File.ReadAllText("Data/invoices-attelas.json");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            return JsonSerializer.Deserialize<List<Invoice>>(json, options);
        }

        private List<Client>? LoadClients()
        {
            var json = File.ReadAllText("Data/clients-attelas.json");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            return JsonSerializer.Deserialize<List<Client>>(json, options);
        }
    }
}