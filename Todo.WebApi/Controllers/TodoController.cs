using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Todo.Model;
using Todo.Interfaces;
using Todo.WebApi.Utility;
using Microsoft.AspNetCore.Cors;
using Service.Messaging;

namespace Todo.WebApi.Controllers
{
    [Route("api/v1/todos")]
    [EnableCors("SiteCorsPolicy")]
    [ServiceFilter(typeof(MyFilterAttribute))]
    public class TodoController : Controller
    {
        private IRepository<TodoItem> todoRepository;
        private IDataContext dataContext;
        public TodoController(IRepository<TodoItem> todoRepository, IDataContext dataContext)
        {
            this.todoRepository = todoRepository;
            this.dataContext = dataContext;
        }

        [HttpGet]
        //[ResponseCache(Duration = 5)]
        public IActionResult Get()
        {
            
            var items = this.todoRepository.GetAll();

            return Ok(items);
        }

        [HttpGet("{id}")]
        [NotFoundFilter]
        public IActionResult Get(int id)
        {
            var item = this.todoRepository.GetByID(id);

           
            return Ok(item);
 
        }

        [HttpPost]
        
        public IActionResult Post([FromBody]TodoItem value)
        {
            var path = Request.Scheme +"://" + Request.Host.Value + Request.Path;
            this.todoRepository.Insert(value);
            this.dataContext.Save();
            return Created(new Uri(path + "/" + value.Id), value);
            
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TodoItem value)
        {
            this.todoRepository.Update(value);
            this.dataContext.Save();

            return Accepted();
         
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.todoRepository.Delete(id);
            this.dataContext.Save();
            

            return NoContent();
            
        }
    }
}
