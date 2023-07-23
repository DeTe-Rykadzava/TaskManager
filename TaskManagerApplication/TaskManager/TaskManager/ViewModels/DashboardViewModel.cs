using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class DashboardViewModel : ViewModelBase
{
    public ObservableCollection<TaskModel> Tasks { get; }

    public int CountTodayTasks { get; } = 0;

    public IEnumerable<ISeries> Series { get; set; }

    public LabelVisual Title { get; set; } = new LabelVisual()
    {
        Text = "Соотношение задач к их статусам",
        TextSize = 25,
        Padding = new Padding(10),
        Paint = new SolidColorPaint(SKColors.LightSlateGray)
    };
    
    public bool ShowDiagram { get; }

    public DashboardViewModel(ObservableCollection<TaskModel> tasks)
    {
        Tasks = tasks;
        var today = DateTime.Today;

        if (Tasks.Any())
        {
            CountTodayTasks = Tasks
                .Count(x => x.Date == new DateOnly(today.Year,today.Month, today.Day));

            var statuses = Tasks.Select(s => s.Status).Distinct().ToList();
            List<DiagramData> data = new List<DiagramData>();
            foreach (var status in statuses)
            {
                data.Add();
            }
        }
        else
            ShowDiagram = false;
    }
    
    // Приложение 3.1
    
    public class DiagramData 
    {
        public string StatusName { get; set; } = string.Empty;
        public int CountTasks { get; set; }
    }
}