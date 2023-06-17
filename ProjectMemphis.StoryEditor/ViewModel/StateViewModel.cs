using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectMemphis.StoryEditor.ViewModel
{
    public class StateViewModel : BindableBase
    {
        private IServiceCollection _serviceCollection;
        public string Name { get; set; }

        public Guid Guid { get; private set; }

        public ObservableCollection<Guid> Childs { get; private set; }

        public StateViewModel(IServiceCollection collections)
        {
            _serviceCollection = collections;
            Guid = Guid.NewGuid();
            Childs = new ObservableCollection<Guid>();
        }

        public override string ToString()
        {
            return Name + " " + Guid;
        } 

    }
}
