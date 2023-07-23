using System;

namespace TaskManager.Models;

public class TaskRequestModel
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public DateOnly Date { get; set; }

    public Guid UserId { get; set; }

    public TaskStatusModel Status { get; set; }
}