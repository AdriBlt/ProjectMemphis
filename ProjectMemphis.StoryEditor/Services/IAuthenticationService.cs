using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMemphis.StoryEditor.Services
{
    public enum SignInState
    {
        Succeeded,
        Failed
    }

    public interface IAuthenticationService
    {
        event EventHandler<SignInState> SignInChanged;

        String Name { get; set; }
        Uri Picture { get; set; }

        void SignInUser();
    }
}
