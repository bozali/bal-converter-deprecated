namespace Bal.Converter.Services ;

public interface IConfigurationService
{
    static string TempPath
    {
        get => Path.Combine(Path.GetTempPath(), "bal-converter");
    }

    static string ApplicationDataPath
    {
        get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "bal-converter");
    }

    static string ConfigurationPath
    {
        get => Path.Combine(ApplicationDataPath, "configuration.xml");
    }

}