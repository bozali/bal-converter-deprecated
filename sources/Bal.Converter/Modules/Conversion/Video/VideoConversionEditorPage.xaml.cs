using System.Collections.Specialized;
using Windows.Media.Core;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bal.Converter.Modules.Conversion.Video
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideoConversionEditorPage : Page
    {
        public VideoConversionEditorPage()
        {
            this.ViewModel = App.GetService<VideoConversionEditorViewModel>();
            this.ViewModel.AvailableFilters.CollectionChanged += (sender, args) =>
            {
                // NOTE We need to add the items for the MenuFlyout from codebehind. We need to refactor this if ItemSources are available.
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    if (args.NewItems != null)
                    {
                        string filter = args.NewItems[0] as string;

                        var item = new MenuFlyoutItem
                        {
                            Text = filter,
                            Command = this.ViewModel.AddFilterCommand,
                            CommandParameter = filter
                        };

                        this.Flyout.Items.Add(item);
                    }
                }
            };

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

        private void FlyoutClearButtonClicked(object sender, RoutedEventArgs e)
        {
            this.ClearFlyout.Hide();
        }
    }
}
