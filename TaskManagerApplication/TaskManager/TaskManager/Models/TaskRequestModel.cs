using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using ReactiveUI;
using TaskManager.ViewModels;

namespace TaskManager.Models;

public class TaskRequestModel : INotifyPropertyChanged
{   
    [JsonIgnore]
    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set => SetField(ref _title, value);
    }
    
    [JsonIgnore]
    private string? _description;
    
    public string? Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public DateOnly Date { get; set; }

    public Guid UserId { get; set; }
    
    public TaskStatusModel Status { get; set; }
    
    [JsonIgnore]
    private TimeSpan? _timeStart;
    
    [JsonIgnore]
    public TimeSpan? TimeStart
    {
        get => _timeStart;
        set
        {
            SetField(ref _timeStart, value);
            if(value != null)
                StartTime = new TimeOnly(value.Value.Hours, value.Value.Minutes);
        }
    }

    [JsonIgnore]
    private TimeSpan? _timeEnd;
    [JsonIgnore]
    public TimeSpan? TimeEnd
    {
        get => _timeEnd;
        set
        {
            SetField(ref _timeEnd, value);
            if(value != null)
                EndTime = new TimeOnly(value.Value.Hours, value.Value.Minutes);
        }
    }
    [JsonIgnore]
    private DateTimeOffset? _dateOffset;
    
    [JsonIgnore]
    public DateTimeOffset? DateOffset
    {
        get => _dateOffset;
        set
        {
            SetField(ref _dateOffset, value);
            if(value != null)
                Date = new DateOnly(value.Value.Year, value.Value.Month, value.Value.Day);
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}