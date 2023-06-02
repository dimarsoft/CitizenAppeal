using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CitizenAppealApp.Models;

/// <summary>
/// Инструмент для чтения/сохранения настройки приложения
/// </summary>
public static class SettingsTools
{
    private static readonly XmlSerializerNamespaces EmptyNamespaces = new (new[] { new XmlQualifiedName() });
    /// <summary>
    /// Путь к файлу с настройкой приложения
    /// </summary>
    private static readonly string SettingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.xml");
    
    /// <summary>
    /// Чтение настройки из файла
    /// </summary>
    /// <returns></returns>
    public static Settings Read()
    {
        var serializer = new XmlSerializer(typeof(Settings));

        try
        {
            using var stream = new XmlTextReader(SettingsFilePath);
            var result = (Settings)serializer.Deserialize(stream);
            return result;
        }
        catch (Exception)
        {
            return new Settings();
        }        
    }

    /// <summary>
    /// Сохранение настройки в файл
    /// </summary>
    /// <param name="settings"></param>
    public static void Save(Settings settings)
    {
        var serializer = new XmlSerializer(typeof(Settings));

        using var fs = new XmlTextWriter(SettingsFilePath, Encoding.UTF8)
        {
            Formatting = Formatting.Indented
        };
        serializer.Serialize(fs, settings, EmptyNamespaces);
    }
}