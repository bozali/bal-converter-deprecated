using Bal.Converter.Common.Enums;

using LiteDB;

namespace Bal.Converter.Modules.Downloads;

public interface IDownloadJob
{
    [BsonId]
    int Id { get; set; }

    [BsonField]
    string Url { get; set; }

    [BsonField]
    MediaFileExtension TargetFormat { get; set; }
}
