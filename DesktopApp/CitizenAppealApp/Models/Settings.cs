using System;

namespace CitizenAppealApp.Models;

/// <summary>
/// Настройка приложения
/// </summary>
[Serializable]
public class Settings
{
    /// <summary>
    /// Путь к АПИ сервера
    /// </summary>
    public string ServerUrl { get; set; }
}