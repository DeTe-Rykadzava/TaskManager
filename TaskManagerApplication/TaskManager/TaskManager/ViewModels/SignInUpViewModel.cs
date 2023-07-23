using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class SignInUpViewModel : ViewModelBase
{
    private UserModel? _user;
    public UserModel? User
    {
        get => _user;
        set => this.RaiseAndSetIfChanged(ref _user, value);
    }
    
    private string _login = string.Empty;
    public string Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }
    
    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }
    
    private string _userName = string.Empty;
    public string UserName
    {
        get => _userName;
        set => this.RaiseAndSetIfChanged(ref _userName, value);
    }

    private bool _isLogin = true;
    public bool IsLogin
    {
        get => _isLogin;
        set => this.RaiseAndSetIfChanged(ref _isLogin, value);
    }

    public ICommand LoginCommand { get; }
    public ICommand RegistrationCommand { get; }
    
    // для отображения дополнений к коду или исправлений ошибок я нажимаю клавиши alt + enter
    public ReactiveCommand<string, Unit> SwitchSignStateCommand { get; }

    public SignInUpViewModel()
    {
        var canLoginExecute =
            this.WhenAnyValue(x => x.Login,
                x => x.Password,
                x => x.IsLogin,
                    (login, password, isLogin) =>
                        !string.IsNullOrWhiteSpace(login) &&
                        !string.IsNullOrWhiteSpace(password) &&
                        isLogin)
                .DistinctUntilChanged();
        var canRegistrationExecute = this.WhenAnyValue(x => x.UserName,
            x => x.Login,
            x => x.Password,
            x => x.IsLogin,
            (userName, login, password, isLogin) =>
                !string.IsNullOrWhiteSpace(userName) &&
                !string.IsNullOrWhiteSpace(login) &&
                !string.IsNullOrWhiteSpace(password) &&
                !isLogin)
            .DistinctUntilChanged();
        LoginCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            User = await UserModel.GetUser(Login, Password);
        }, canLoginExecute);
        RegistrationCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            User = await UserModel.CreateUser(UserName, Login, Password);
        }, canRegistrationExecute);

        SwitchSignStateCommand = ReactiveCommand.Create((string state) =>
        {
            // тот самый костыль для изменения того что будет отображаться
            switch (state)
            {
                case "Login":
                    UserName = string.Empty;
                    Login = string.Empty;
                    Password = string.Empty;
                    IsLogin = true;
                    break;
                case "Registration":
                    UserName = string.Empty;
                    Login = string.Empty;
                    Password = string.Empty;
                    IsLogin = false;
                    break;
                default:
                    break;
            }
        });

    }

}