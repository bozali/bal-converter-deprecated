using Bal.Converter.Modules.Conversion.Video.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Windows.Media.Core;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Dispatching;

namespace Bal.Converter.Modules.Conversion.Video.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideoConversionEditorPage : Page
    {
        public VideoConversionEditorPage()
        {
            this.ViewModel = App.GetService<VideoConversionEditorViewModel>();

            this.InitializeComponent();
            
            this.ViewModel.SetMediaPlayerSource = (path) => this.MediaPlayer.Source = MediaSource.CreateFromUri(new Uri(path));

            this.MediaPlayer.MediaPlayer.MediaOpened += (s, e) =>
            {
                App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, () =>
                {
                    this.ViewModel.Metadata.MaximumLength = Convert.ToInt32(this.MediaPlayer.MediaPlayer.NaturalDuration.TotalSeconds);

                    this.ViewModel.VideoConversionOptions.MinVideoLength = 0.0;
                    this.ViewModel.VideoConversionOptions.MaxVideoLength = this.MediaPlayer.MediaPlayer.NaturalDuration.TotalSeconds;
                });
            };
        }

        public VideoConversionEditorViewModel ViewModel { get; }

        private void OnTrimValueChanged(object? sender, RangeChangedEventArgs e)
        {
            if (e.ChangedRangeProperty == RangeSelectorProperty.MinimumValue)
            {
                this.ViewModel.VideoConversionOptions.MinVideoLength = e.NewValue;
            }
            else if (e.ChangedRangeProperty == RangeSelectorProperty.MaximumValue)
            {
                this.ViewModel.VideoConversionOptions.MaxVideoLength = e.NewValue;
            }
        }
    }
}
