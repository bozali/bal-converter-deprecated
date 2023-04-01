using Windows.Storage.Pickers;
using Bal.Converter.Domain.Picker;
using Windows.Storage;

namespace Bal.Converter.Services;

public class DialogPickerService : IDialogPickerService
{
    public async Task<StorageFile?> OpenFileSave(FilePickerOptions options)
    {
        var picker = new FileSavePicker
        {
            SuggestedStartLocation = options.SuggestedStartLocation,
            SuggestedFileName = options.SuggestedFileName
        };

        foreach (var choice in picker.FileTypeChoices)
        {
            picker.FileTypeChoices.Add(choice);
        }

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        
        return await picker.PickSaveFileAsync();
    }
}