using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace CitizenAppealApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _email;
    private string _appealText;
    private string _lastErrorText;
    private bool _isSendInProgress;

    public ICommand SendAppealCommand { get; }

    public MainWindowViewModel()
    {
        SendAppealCommand = ReactiveCommand.CreateFromTask(SendAppealToServerAsync);
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
                { "email", Email },
                { "text", AppealText }
            };
            var content = new FormUrlEncodedContent(values);

            LastErrorText = "Start reques";

            try
            {
                using HttpClient client = new();
                var response = await client.PostAsync("https://tts.api.cloud.yandex.net/speech/v1/tts:synthesize",
                    content);

                await Task.Delay(1000);

                var responses = await response.Content.ReadAsStringAsync();
                LastErrorText = $"Ok: {responses}";
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

    public string LastErrorText
    {
        get => _lastErrorText;
        private set => this.RaiseAndSetIfChanged(ref _lastErrorText, value);
    }
}