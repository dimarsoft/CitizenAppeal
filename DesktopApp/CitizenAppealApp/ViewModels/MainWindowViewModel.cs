using System;
//using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Input;
using CitizenAppealApp.Models;
using ReactiveUI;

namespace CitizenAppealApp.ViewModels;

public sealed class MainWindowViewModel : ReactiveObject
{
    private string _email;
    private string _appealText;
    private string _lastErrorText;
    private bool _isSendInProgress;
    private string _serverUrl;
    private string _yourName;
    private readonly Settings _settings;

    public ICommand SendAppealCommand { get; }

    public MainWindowViewModel()
    {
        // Команда для кнопки, она выполяется асинхронно
        SendAppealCommand = ReactiveCommand.CreateFromTask(SendAppealToServerAsync);

        // читаем настроку приложения
        _settings = SettingsTools.Read();

        //нам нужен путь к АПИ сервера
        ServerUrl = _settings.ServerUrl;
    }


    public bool IsSendInProgress
    {
        get => _isSendInProgress;
        private set => this.RaiseAndSetIfChanged(ref _isSendInProgress, value);
    }

    /// <summary>
    /// Выполение команды при нажатии на кнопку отправить
    /// </summary>
    private async Task SendAppealToServerAsync()
    {
        try
        {
            // сохраним путь к АПИ
            _settings.ServerUrl = ServerUrl;
            SettingsTools.Save(_settings);
            
            IsSendInProgress = true;
            RequesToServerModel requestModel = new(YourName, Email, AppealText, ServerUrl);

            LastErrorText = $"Запрос к {ServerUrl}";
            LastErrorText = await requestModel.Send();
        }
        catch (Exception e)
        {
            LastErrorText = e.Message;
        }
        finally
        {
            IsSendInProgress = false;
        }
    }

    public string YourName
    {
        get => _yourName;
        set => this.RaiseAndSetIfChanged(ref _yourName, value);
    }

    //[Required]
    // [EmailAddress]
    public string Email
    {
        get => _email;
        set => this.RaiseAndSetIfChanged(ref _email, value);
    }
    //[Required]
    public string AppealText
    {
        get => _appealText;
        set => this.RaiseAndSetIfChanged(ref _appealText, value);
    }

    public string ServerUrl
    {
        get => _serverUrl;
        set => this.RaiseAndSetIfChanged(ref _serverUrl, value);
    }

    public string LastErrorText
    {
        get => _lastErrorText;
        private set => this.RaiseAndSetIfChanged(ref _lastErrorText, value);
    }
}