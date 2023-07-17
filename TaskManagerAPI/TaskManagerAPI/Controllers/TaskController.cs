using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class TaskController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public TaskController(TaskManagerContext context)
        {
            _context = context;
        }

        // GET: api/Task
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Models.TaskDto>>> GetTasks([Required] Guid userId)
        {
            if (!_context.Tasks.Any())
            {
                return NotFound();
            }
            
            return (await _context.Tasks.Include(i => i.Status).ToListAsync())
                    .ConvertAll(c => new TaskDto(c));
        }

        // GET: api/Task/5
        [HttpGet]
        public async Task<ActionResult<Models.TaskDto>> GetTask([FromQuery][Required] Guid userId, 
            [FromQuery][Required] Guid taskId)
        {
            if (!_context.Tasks.Any())
            {
                return NotFound();
            }
            var task = await _context.Tasks.Include(i => i.Status)
                                            .FirstOrDefaultAsync(x => x.UserId == userId &&
                                                                         x.Id == taskId);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(new TaskDto(task));
        }

        // PUT: api/Task/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutTask([FromQuery][Required]Guid userId,
            [FromBody] Models.TaskDto taskDto)
        {
            var task = await _context.Tasks.Include(i => i.Status)
                .FirstOrDefaultAsync(x => x.UserId == userId &&
                                          x.Id == taskDto.Id);

            if (task == null)
            {
                return NotFound();
            }

            // title
            if (task.Title != taskDto.Title)
                task.Title = taskDto.Title;
            // description
            if (task.Description != taskDto.Description)
                task.Description = taskDto.Description;
            // start time
            if (task.StartTime != taskDto.StartTime)
                task.StartTime = taskDto.StartTime;
            // end time
            if (task.EndTime != taskDto.EndTime)
                task.EndTime = taskDto.EndTime;
            // date
            if (task.Date != taskDto.Date)
                task.Date = taskDto.Date;
            // status
            if (task.Status.Id != taskDto.Status.Id)
                task.StatusId = taskDto.Status.Id;

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return NoContent();
        }

        // POST: api/Task
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.TaskDto>> PostTask([FromBody][Required] Models.TaskModel task)
        {
            var newTask = new Models.Task()
            {
                Id = Guid.NewGuid(),
                Title = task.Title,
                Description = task.Description,
                StartTime = task.StartTime,
                EndTime = task.EndTime,
                Date = task.Date,
                UserId = task.UserId,
                StatusId = task.Status.Id
            };
            
            _context.Tasks.Add(newTask);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            var taskDto = new TaskDto((await _context.Tasks
                .Include(i => i.Status)
                .FirstOrDefaultAsync(x => x.Id == newTask.Id))!);

            return Ok(taskDto);
        }

        // DELETE: api/Task/5
        [HttpDelete]
        public async Task<IActionResult> DeleteTask([Required] Guid id)
        {
            if (!_context.Tasks.Any())
            {
                return NotFound();
            }
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
