using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CitizenAppealApp.Models;

/// <summary>
/// Модель для выполения запроса к серверу
/// </summary>
public sealed class RequestToServerModel(string name, string email, string message, string server)
{
    private readonly RequestBody _requestBody = new(name, email, message);

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = new LowerCaseNamingPolicy(),
        WriteIndented = true
    };
    /// <summary>
    /// Отправка запроса на сервер
    /// </summary>
    /// <returns></returns>
    public async Task<string> Send()
    {
        var jsonBody = JsonSerializer.Serialize(_requestBody, _options);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, server);

        request.Content = content;
        request.Headers.Add("accept", "application/json");
        
        try
        {
            using HttpClient client = new();
                
            var response = await client.SendAsync(request);

            // просто задержка для проверки отзывчивости UI
            await Task.Delay(1000);

            var responses = await response.Content.ReadAsStringAsync();
            return $"Ответ: {responses}";
        }
        catch (Exception e)
        {
            return $"Ошибка: {e.Message}";
        }        
    }
}