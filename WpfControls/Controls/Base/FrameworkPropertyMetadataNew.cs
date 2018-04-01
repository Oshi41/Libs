using System.Windows;
using System.Windows.Data;

namespace WpfControls.Controls.Base
{
    /// <summary>
    /// Переопределил класс, чтобы callback вызывался только после отприсовки
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FrameworkPropertyMetadataNew<T> : FrameworkPropertyMetadata
        where T : FrameworkElement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d">Гарантированно отрисованный контрол</param>
        /// <param name="e"></param>
        public delegate void PropertyChangedCallbackDirect(T d, DependencyPropertyChangedEventArgs e);


        
        public FrameworkPropertyMetadataNew(object defaultValue,
            PropertyChangedCallbackDirect propertyChangedCallback,
            bool twoWayBinding = true)
        {
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindsTwoWayByDefault = twoWayBinding;
            DefaultValue = defaultValue;

            SetCallBack(propertyChangedCallback);
        }

        private void SetCallBack(PropertyChangedCallbackDirect propertyChangedCallback)
        {
            PropertyChangedCallback = (o, args) =>
            {
                if (o is T control && control.IsLoaded)
                {
                    propertyChangedCallback?.Invoke(control, args);
                }
            };
        }
    }
}
