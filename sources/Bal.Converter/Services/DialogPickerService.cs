using Windows.Storage.Pickers;
using Bal.Converter.Domain.Picker;
using Windows.Storage;
using ABI.Windows.Devices.AllJoyn;

namespace Bal.Converter.Services;

public class DialogPickerService : IDialogPickerService
{
    public async Task<StorageFile?> OpenFileSave(FileSavePickerOptions options)
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

    public async Task<StorageFile?> OpenFile(FileOpenPickerOptions options)
    {
        var picker = new FileOpenPicker
        {
            SuggestedStartLocation = options.SuggestedStartLocation,
        };

        foreach (string type in options.FileTypeFilter)
        {
            picker.FileTypeFilter.Add(type);
        }

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        return await picker.PickSingleFileAsync();
    }
}