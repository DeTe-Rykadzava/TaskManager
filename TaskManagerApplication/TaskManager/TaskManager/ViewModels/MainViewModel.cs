using ReactiveUI;
using TaskManager.Models;
using System;

namespace TaskManager.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase? _viewModel;
    public ViewModelBase? ViewModel
    {
        get => _viewModel;
        set => this.RaiseAndSetIfChanged(ref _viewModel, value);
    }

    private UserModel? _user;
    public UserModel? User
    {
        get => _user;
        set => this.RaiseAndSetIfChanged(ref _user, value);
    }

    public MainViewModel()
    {
        this.WhenAnyValue(x => x.User)
            .Subscribe(s =>
            {
                if (s == null)
                {
                    var VM = new SignInUpViewModel();
                    ViewModel = VM;
                    
                    //  надеюсь будет работать
                    VM.WhenAnyValue(x => x.User)
                        .Subscribe(s =>
                        {
                            User = s;
                        });
                }
                else
                {
                    ViewModel = new HomeViewModel(User);
                }

                
            });
    }
}