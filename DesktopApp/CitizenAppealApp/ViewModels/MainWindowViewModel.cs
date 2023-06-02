using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
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

    public ICommand SendAppealCommand { get; }

    public MainWindowViewModel()
    {
        SendAppealCommand = ReactiveCommand.CreateFromTask(SendAppealToServerAsync);

        ServerUrl = "https://tts.api.cloud.yandex.net/speech/v1/tts:synthesize";
    }


    public bool IsSendInProgress
    {
        get => _isSendInProgress;
        private set => this.RaiseAndSetIfChanged(ref _isSendInProgress, value);
    }

    private async Task SendAppealToServerAsync()
    {
        try
        {
            IsSendInProgress = true;
            var values = new Dictionary<string, string>
            {
                { "name", YourName },
                { "email", Email },
                { "message", AppealText }
            };
            var content = new FormUrlEncodedContent(values);

            LastErrorText = $"Запрос к {ServerUrl}";

            try
            {
                using HttpClient client = new();
                
                var response = await client.PostAsync(ServerUrl, content);

                await Task.Delay(1000);

                var responses = await response.Content.ReadAsStringAsync();
                LastErrorText = $"Ответ: {responses}";
            }
            catch (Exception e)
            {
                LastErrorText = $"Ошибка: {e.Message}";
            }
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