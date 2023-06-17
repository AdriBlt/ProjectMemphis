using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMemphis.StoryEditor.Services;

namespace ProjectMemphis.StoryEditor.Command
{
    class SignInCommand : Command
    {
        private readonly IServiceCollection _services;

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        protected override Task OnExecuteAsync(object parameter)
        {
            return new Task(() =>
            {
                _services.GetService<IAuthenticationService>().SignInUser();
            });
        }

        public SignInCommand(IServiceCollection services)
        {
            _services = services;
        }
    }
}
