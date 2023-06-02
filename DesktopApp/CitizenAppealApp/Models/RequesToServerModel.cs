using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CitizenAppealApp.Models;

/// <summary>
/// Модель для выполения запроса к серверу
/// </summary>
public sealed class RequesToServerModel
{
    private readonly string _server;
    private readonly RequestBody _requestBody;

    public RequesToServerModel(string name, string email, string message, string server)
    {
        _requestBody = new RequestBody(name, email, message);
        _server = server;
    }

    /// <summary>
    /// Отправка запроса на сервер
    /// </summary>
    /// <returns></returns>
    public async Task<string> Send()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new LowerCaseNamingPolicy(),
            WriteIndented = true
        };
        var jsonBody = JsonSerializer.Serialize(_requestBody, options);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, _server);

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