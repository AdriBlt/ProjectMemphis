using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows.Input;
using Windows.ApplicationModel.Chat;
using ProjectMemphis.StoryEditor.ViewModel;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ProjectMemphis.StoryEditor.Command
{
    public class ShowAddChildPopCommand : Command
    {
        private readonly ObservableCollection<StateViewModel> _collection;
        private readonly IServiceCollection _services;

        public ShowAddChildPopCommand(IServiceCollection services, ObservableCollection<StateViewModel> collection)
        {
            _services = services;
            _collection = collection;
            this.Label = "Add a new child";
        }

        public override bool CanExecute(object parameter)
        {
            return parameter != null;
        }

        protected override Task OnExecuteAsync(object parameter)
        {
            return new Task(() =>
            {
                var state = parameter as StateViewModel;

                var command = new NewChildCommand(_services);

                var textBox = new TextBox {PlaceholderText = "Enter a guid here"};
                textBox.SetBinding(TextBox.TextProperty, new Binding
                {
                    Path = new PropertyPath("Text"),
                    Source = command,
                    Mode = BindingMode.TwoWay
                });

                var dialog = new ContentDialog
                {
                    Title = "Add a child",
                    Content = textBox,
                    PrimaryButtonCommand = command,
                    PrimaryButtonText = "Ok",
                    PrimaryButtonCommandParameter = state
                };

                var a = dialog.ShowAsync();
               
            });
        }
    }
}