using System;
using System.Threading;

namespace MVVM
{
    public class ViewModelBase : ObservableObject
    {
        private readonly SynchronizationContext _syncContext;

        public ViewModelBase()
        {
            _syncContext = SynchronizationContext.Current;
        }

        protected void SafeExecute(Action a)
        {
            if (a == null)
                return;
            
            _syncContext.Post(state => a(), this);
        }
    }
}
