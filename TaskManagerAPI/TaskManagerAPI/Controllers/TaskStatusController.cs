using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Contexts;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatusController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public TaskStatusController(TaskManagerContext context)
        {
            _context = context;
        }

        // GET: api/TaskStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.TaskStatusDto>>> GetTaskStatuses()
        {
            if (!_context.TaskStatuses.Any())
            {
                return NotFound();
            }

            return (await _context.TaskStatuses.ToListAsync())
                .ConvertAll(c => new TaskStatusDto()
                {
                    Id = c.Id,
                    Name = c.Name
                });
        }
    }
}
