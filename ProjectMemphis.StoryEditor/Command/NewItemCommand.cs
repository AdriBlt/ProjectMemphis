using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ProjectMemphis.StoryEditor.ViewModel;
using ProjectMemphis.StoryEditor.Services;

namespace ProjectMemphis.StoryEditor.Command
{
    public class NewItemCommand : Command
    {
        private readonly ObservableCollection<StateViewModel> _collection;
        private readonly IServiceCollection _serviceCollection;

        public NewItemCommand(IServiceCollection serviceCollection, ObservableCollection<StateViewModel> collection)
        {
            _serviceCollection = serviceCollection;
            this._collection = collection;
            Label = "Create a new item";
        }

        public override bool CanExecute(object parameter)
        {
            return _collection != null;
        }

        protected override Task OnExecuteAsync(object parameter)
        {
            return new Task(() => 
            {
                var vm = new StateViewModel(_serviceCollection);
                _collection.Add(vm);
                var st = _serviceCollection.GetService<IStateSingleton>();

                st.States.Add(new Data.State() { Guid = vm.Guid });

            });
        }
    }
}
