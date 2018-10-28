namespace TodoApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using TodoApi.Models;

    //Routing and URL paths
    [Route("api/[controller]")] //take the template string in the controller's Route attribute
    [ApiController]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
            if(_context.TodoItems.Count()==0)
            {
                /*Creates a new Todo item when TodoItems is empty.
                 * You won't be able to delete all the Todo items 
                 * because the constructor creates a new one if TodoItems is empty.
                 */
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        //Get to-do items
        //The [HttpGet] attribute denotes a method that responds to an HTTP GET request.

        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        //In the following GetById method, "{id}" is a placeholder variable 
        //for the unique identifier of the to-do item.

        [HttpGet("{id}",Name ="GetTodo")]
        public ActionResult<TodoItem> GetById(long id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        //CRUD

        //Create 
        //an HTTP POST method, as indicated by the [HttpPost] attribute. MVC gets the value of the to-do item from the body of the HTTP request.
        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        //Update
        [HttpPut("{id}")]
        public IActionResult Update(long id,TodoItem item)
        {
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return NoContent(); 

            /*Update is similar to Create,
             * except it uses HTTP PUT. The response is 204 (No Content). 
             * According to the HTTP specification, a PUT request requires 
             * the client to send the entire updated entity, not just the deltas.
             * To support partial updates, use HTTP PATCH.
             */
        }

        //Delete
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

        //// GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
   // }
//}
