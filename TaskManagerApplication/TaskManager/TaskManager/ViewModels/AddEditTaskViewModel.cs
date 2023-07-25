using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using ReactiveUI;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class AddEditTaskViewModel : ViewModelBase
{

    private TaskModel? _taskEdit;
    public TaskModel? TaskEdit
    {
        get => _taskEdit;
        set => this.RaiseAndSetIfChanged(ref _taskEdit, value);
    }
    
    private TaskModel? _taskEditOld;
    public TaskModel? TaskEditOld
    {
        get => _taskEditOld;
        set => this.RaiseAndSetIfChanged(ref _taskEditOld, value);
    }

    
    private TaskRequestModel? _taskAdding;
    public TaskRequestModel? TaskAdding
    {
        get => _taskAdding;
        set => this.RaiseAndSetIfChanged(ref _taskAdding, value);
    }

    private bool _isAdding = true;
    public bool IsAdding
    {
        get => _isAdding;
        set => this.RaiseAndSetIfChanged(ref _isAdding, value);
    }

    private ObservableCollection<TaskModel> _tasks { get; }

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    
    public ICommand DeleteCommand { get; }

    private readonly UserModel _user;

    private readonly TasksViewModel _rootVM;

    public AddEditTaskViewModel(TaskModel? task, TaskModel? taskOld, ObservableCollection<TaskModel> tasks, UserModel user,
        TasksViewModel tasksViewModel)
    {
        _tasks = tasks;
        _user = user;
        _rootVM = tasksViewModel;
        if (task == null)
        {
            _taskAdding = new TaskRequestModel();
            _taskAdding.Status = new TaskStatusModel() { Id = 1, Name = "В работе" };
            _taskAdding.UserId = _user.UserId;
            IsAdding = true;
        }
        else
        {
            _taskEdit = task;
            _taskEditOld = taskOld;
            IsAdding = false;
        }

        var canAddTask = this.WhenAnyValue(
            x => x.TaskAdding,
            x=> x.IsAdding,
            (taskAdding, isAdding) =>
                taskAdding != null &&
                isAdding &&
                !string.IsNullOrWhiteSpace(taskAdding.Title) &&
                taskAdding.TimeStart != null &&
                taskAdding.TimeEnd != null &&
                taskAdding.DateOffset != null 
            ).DistinctUntilChanged();
        
        AddCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var addedTask = await TaskModel.CreateTask(_taskAdding!);
            if (addedTask == null)
                return;
            
            _tasks.Add(addedTask);
            _rootVM.GoBackCommand.Execute(Unit.Default);
            
        });

        var canEditTask = this.WhenAnyValue(
            x => x.TaskEdit,
            x => x.TaskEdit.Title,
            x => x.IsAdding,
            (editTask, taskTitle, isAdding) =>
                editTask != null &&
                !string.IsNullOrWhiteSpace(taskTitle) &&
                !isAdding
        ).DistinctUntilChanged();

        EditCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var edited = await TaskModel.UpdateTask(TaskEdit!, _user.UserId);
            if(!edited)
                return;
            var index = _tasks.IndexOf(TaskEditOld!);
            _tasks[index] = TaskEdit!;
            _rootVM.GoBackCommand.Execute(Unit.Default);
        }, canEditTask);

        
        DeleteCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if(TaskEditOld == null)
                 return;
            
            var deleted = await TaskModel.DeleteTask(TaskEditOld.Id);
            if(!deleted)
                return;

            _tasks.Remove(TaskEditOld);
            _rootVM.GoBackCommand.Execute(Unit.Default);
        });
    }
}