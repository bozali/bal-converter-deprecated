using Windows.Storage.Pickers;

namespace Bal.Converter.Domain.Picker;

public class FileOpenPickerOptions
{
    public FileOpenPickerOptions()
    {
        this.FileTypeFilter = new List<string>();
        this.SuggestedStartLocation = PickerLocationId.Desktop;
    }

    public PickerLocationId SuggestedStartLocation { get; set; }

    public IList<string> FileTypeFilter { get; set; }
}