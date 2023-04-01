using Windows.Storage.Pickers;

namespace Bal.Converter.Domain.Picker;

public class FilePickerOptions
{
    public FilePickerOptions()
    {
        this.SuggestedStartLocation = PickerLocationId.Desktop;
        this.FileTypeChoices = new Dictionary<string, IList<string>>();
    }

    public PickerLocationId SuggestedStartLocation { get; set; }

    public IDictionary<string, IList<string>> FileTypeChoices { get; set; }

    public string SuggestedFileName { get; set; }
}