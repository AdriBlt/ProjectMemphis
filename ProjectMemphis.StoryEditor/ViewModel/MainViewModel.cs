using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;
using ProjectMemphis.StoryEditor.Command;
using ProjectMemphis.StoryEditor.Services;

namespace ProjectMemphis.StoryEditor.ViewModel
{
    class MainViewModel : BindableBase
    {
        public NewItemCommand NewItemCommand { get; private set; }
        public ShowAddChildPopCommand NewChildCommand { get; private set; }
        public SignInCommand SignInCommand { get; private set; }
        public ObservableCollection<StateViewModel> ListSource { get; private set; }

        private StateViewModel _selectedState;
        public StateViewModel SelectedItem
        {
            set
            {
                if (value != _selectedState)
                {
                    _selectedState = value;
                    OnPropertyChanged();
                }
            }

            get
            {
                return _selectedState;
            }
        }

        private String _name;
        public String Name
        {
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
            get { return _name; }
        }

        private Uri _image;

        public Uri Image
        {
            set
            {
                if (value != _image)
                {
                    _image = value;
                    OnPropertyChanged();
                }
            }

            get { return _image; }
        }

        public MainViewModel(IServiceCollection services)
        {
            ListSource = new ObservableCollection<StateViewModel>();

            NewItemCommand = new NewItemCommand(services, ListSource);
            NewChildCommand = new ShowAddChildPopCommand(services,ListSource);
            SignInCommand = new SignInCommand(services);

            var auth = services.GetService<IAuthenticationService>();
            auth.SignInChanged += (object o, SignInState s) =>
            {
                Name = (o as IAuthenticationService).Name;
                Image = (o as IAuthenticationService).Picture;
            };
        }



    }
}
