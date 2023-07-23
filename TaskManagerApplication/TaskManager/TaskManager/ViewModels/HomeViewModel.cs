using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class HomeViewModel : ViewModelBase
{
    private readonly UserModel _user;
    
    private ViewModelBase? _viewModel;
    public ViewModelBase? ViewModel
    {
        get => _viewModel;
        set => this.RaiseAndSetIfChanged(ref _viewModel, value);
    }

    private ObservableCollection<TaskModel> Tasks { get; } = new();

    public ICommand ShowDashboardCommand { get; }
    public ICommand ShowTasksCommand { get; }
    
    public HomeViewModel(UserModel user)
    {
        _user = user;
        
        ShowTasksCommand = ReactiveCommand.Create(() =>
        {
            ViewModel = new TasksViewModel(Tasks);
        });
        ShowDashboardCommand = ReactiveCommand.Create(() =>
        {
            ViewModel = new DashboardViewModel(Tasks);
        });

        RxApp.MainThreadScheduler.Schedule(() => LoadTasks());
    }

    private async Task LoadTasks()
    {
        var tasks = await TaskModel.GetTasks(_user.UserId);
        
        if(!tasks.Any())
            return;

        foreach (var task in tasks)
        {
            Tasks.Add(task);
        }
        
    }
}