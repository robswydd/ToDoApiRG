using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoApiRG.Models;

namespace TodoApiRG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }



        }

        // RG180703: note the fudge from https://github.com/aspnet/Docs/issues/6145: ActionResult

        // can't work with types: <list<ToDoItem>>:
        [HttpGet]
        public ActionResult GetAll()
        {
            return new ObjectResult(_context.TodoItems.ToList());
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult GetById(long id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }
    }



    internal class ApiControllerAttribute : Attribute
    {
    }
}