using System;
using System.Collections.Generic;

namespace TaskManagerAPI.Models;

public partial class Task
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public DateOnly Date { get; set; }

    public Guid UserId { get; set; }

    public int StatusId { get; set; }

    public virtual TaskStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
