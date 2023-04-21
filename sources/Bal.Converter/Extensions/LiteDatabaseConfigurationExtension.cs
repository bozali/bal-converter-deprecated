using Bal.Converter.Services;

using LiteDB;

using Microsoft.Extensions.DependencyInjection;

namespace Bal.Converter.Extensions ;

public static class LiteDatabaseConfigurationExtension
{
    public static IServiceCollection ConfigureLiteDatabase(this IServiceCollection collection)
    {
        var file = new FileInfo(Path.Combine(ILocalSettingsService.AppDataPath, "Data", "Storage.db"));

        if (file.Directory is { Exists: false })
        {
            file.Directory.Create();
        }

        var database = new LiteDatabase(file.FullName);
        return collection.AddSingleton<ILiteDatabase>(database);
    }
}