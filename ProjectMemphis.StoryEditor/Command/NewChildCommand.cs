using ProjectMemphis.StoryEditor.Services;
using ProjectMemphis.StoryEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ProjectMemphis.StoryEditor.Command
{
    public class NewChildCommand : Command
    {
        public String Text { get; set; }

        private readonly IServiceCollection _serviceCollection;

        public NewChildCommand(IServiceCollection services)
        {
            _serviceCollection = services;
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

                Guid resultGuid;
                if (Guid.TryParse(Text, out resultGuid))
                {
                    var st = _serviceCollection.GetService<IStateSingleton>();
                    var result = st.States.Exists(x => x.Guid == resultGuid);

                    if(result)
                    {
                        state.Childs.Add(resultGuid);
                    }
                }
            });
        }
    }
}
