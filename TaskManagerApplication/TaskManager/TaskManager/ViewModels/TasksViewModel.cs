using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class TasksViewModel : ViewModelBase
{
    public ObservableCollection<TaskModel> Tasks { get; }

    private ViewModelBase? _addEditViewModel = null;
    public ViewModelBase? AddEditViewModel
    {
        get => _addEditViewModel;
        set => this.RaiseAndSetIfChanged(ref _addEditViewModel, value);
    }

    public ReactiveCommand<Guid?, Unit> ShowAddEditCommand { get; }

    public ICommand GoBackCommand { get; }

    private readonly UserModel _user;
    
    public TasksViewModel(ObservableCollection<TaskModel> tasks, UserModel user)
    {
        Tasks = tasks;
        _user = user;
        ShowAddEditCommand = ReactiveCommand.Create((Guid? id) =>
        {
            var taskFromTasks = Tasks.FirstOrDefault(x => x.Id == id);
            if(taskFromTasks == null && id != null)
                return;
            
            TaskModel? task = null;
            if (taskFromTasks != null)
            {
                task = new TaskModel();
                task.Id = taskFromTasks.Id;
                task.Title = taskFromTasks.Title;
                task.Description = taskFromTasks.Description;
                task.StartTime = taskFromTasks.StartTime;
                task.EndTime = taskFromTasks.EndTime;
                task.Date = taskFromTasks.Date;
                task.Status = taskFromTasks.Status;
            }

            AddEditViewModel = new AddEditTaskViewModel(task, taskFromTasks,Tasks, _user, this);
        });
        GoBackCommand = ReactiveCommand.Create(() =>
        {
            AddEditViewModel = null;
        });
    }
}