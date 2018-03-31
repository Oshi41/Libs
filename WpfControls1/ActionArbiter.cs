using System;

namespace WpfControls
{
    public class ActionArbiter
    {
        private bool _isActing;
        public void Do(Action a)
        {
            if (_isActing || a == null)
                return;

            try
            {
                _isActing = true;
                a();
            }
            catch
            {
                // ignored
            }
            finally
            {
                _isActing = false;
            }
        }
    }
}
