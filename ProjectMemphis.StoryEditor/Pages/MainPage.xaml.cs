using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using ProjectMemphis.StoryEditor.ViewModel;
using ProjectMemphis.StoryEditor.Services;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace ProjectMemphis.StoryEditor.Pages
{
    public sealed partial class MainPage : Page
    {
        MainViewModel ViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel = (e.Parameter as MainViewModel);
            
            var popup = new Popup()
            {
                Child = new PopupNotification()
            };
            popup.IsOpen = true;

            this.MainGrid.Children.Add(popup);
        }
    }
}
