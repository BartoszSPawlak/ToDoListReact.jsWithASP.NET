using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using System.Xml.Linq;
using todoAPI.Models;

namespace todoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        private static List<Item> Items = new List<Item>(){};
        private readonly ILogger<ToDoController> _logger;
        private readonly todoAPI.ToDoDbContext _context;
        public ToDoController(ILogger<ToDoController> logger, todoAPI.ToDoDbContext context) { 
            _logger = logger;
            _context = context;
        }

        [EnableCors("Policy")]    
        [HttpGet]
        public List<Item> Get()
        {
            Items = _context.Items.ToList();
            return Items;
        }

        [EnableCors("Policy")]
        [HttpPost]
        public Item Post(JsonElement newItem)
        {
            Item item = JsonSerializer.Deserialize<Item>(newItem);

            Items.Add(item);

            _context.Items.Add(item);
            _context.SaveChanges();

            return item;
        }

        [EnableCors("Policy")]
        [HttpPatch]
        public List<Item> Patch(JsonElement idAndIsDoneOfItemToPatchJson) 
        {
            var idAndIsDoneOfItemToPatch = JsonSerializer.Deserialize<Item>(idAndIsDoneOfItemToPatchJson);
            Predicate<Item> predicate = (itemToCheck) => itemToCheck.Id == idAndIsDoneOfItemToPatch.Id;
            var findItem = Items.Find(predicate);
            findItem.IsDone = idAndIsDoneOfItemToPatch.IsDone;

            _context.Update(findItem);
            _context.SaveChanges();

            return Items;
        }

        [EnableCors("Policy")]
        [HttpDelete]
        public List<Item> Delete(JsonElement idOfItemToPatchJson)
        {
            var idOfItemToPatch = JsonSerializer.Deserialize<int>(idOfItemToPatchJson);
            Predicate<Item> predicate = (itemToCheck) => itemToCheck.Id == idOfItemToPatch;
            var findItem = Items.Find(predicate);
            Items.Remove(findItem);

            _context.Remove(findItem);
            _context.SaveChanges();

            return Items;
        }
    }
}
