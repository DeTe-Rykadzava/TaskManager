namespace TaskManagerAPI.Models;

public class TaskDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public DateOnly Date { get; set; }
    
    public TaskStatusDto Status { get; set; }

    public TaskDto()
    {
    }

    public TaskDto(Task task)
    {
        Id = task.Id;
        Title = task.Title;
        Description = task.Description;
        StartTime = task.StartTime;
        EndTime = task.EndTime;
        Date = task.Date;
        Status = new TaskStatusDto()
        {
            Id = task.Status.Id,
            Name = task.Status.Name
        };
    }
}