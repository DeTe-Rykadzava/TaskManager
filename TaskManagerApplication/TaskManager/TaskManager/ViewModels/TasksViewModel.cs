using System.Collections.ObjectModel;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class TasksViewModel : ViewModelBase
{
    public ObservableCollection<TaskModel> Tasks { get; }

    public TasksViewModel(ObservableCollection<TaskModel> tasks)
    {
        Tasks = tasks;
    }
}