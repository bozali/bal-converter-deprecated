using Windows.Storage;

using Bal.Converter.Domain.Picker;

namespace Bal.Converter.Services;

public interface IDialogPickerService
{
    Task<StorageFile?> OpenFileSave(FilePickerOptions options);
}