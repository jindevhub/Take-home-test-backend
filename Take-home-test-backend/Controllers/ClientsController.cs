using Microsoft.AspNetCore.Mvc;
using Take_home_test_backend.Models;


namespace Take_home_test_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly DataService _dataService;

        public ClientsController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("{id}")]
        public ActionResult<Client> GetClientInfo(string id)
        {
            var client = _dataService.Clients.FirstOrDefault(c => c.ClientId == id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Client>> GetClients([FromQuery] string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Ok(_dataService.Clients);
            }

            var clients = _dataService.Clients.Where(c => c.Name == name);
            if (!clients.Any())
            {
                return NotFound();
            }

            return Ok(clients);
        }
    }
}