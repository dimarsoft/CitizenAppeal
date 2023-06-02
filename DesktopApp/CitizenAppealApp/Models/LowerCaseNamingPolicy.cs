using System.Text.Json;

namespace CitizenAppealApp.Models;

/// <summary>
/// ключи всегда с маленькой буквы
/// </summary>
public sealed class LowerCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) => name.ToLower();
}