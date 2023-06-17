using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectMemphis.StoryEditor.Command
{
    public abstract class Command : ICommand
    {
        internal String Label { get; set; }
        public abstract bool CanExecute(object parameter);

        public Task ExecuteAsync(object parameter)
        {
            var task = OnExecuteAsync(parameter);
            task.Start(TaskScheduler.FromCurrentSynchronizationContext());
            return task;
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        protected abstract Task OnExecuteAsync(object parameter);

        public event EventHandler CanExecuteChanged;
    }
}
