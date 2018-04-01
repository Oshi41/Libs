using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfControls.Helper;

namespace WpfControls.Controls
{
    public class BaseThematisedControl<T> : Control
        where T : Control
    {
        #region Fields

        private ActionArbiter _actionArbiter;

        #endregion

        #region Properties

        /// <summary>
        /// Арбитр действий, создается при первом исползовании, зря память не ест
        /// </summary>
        protected ActionArbiter ActionArbiter => _actionArbiter ?? (_actionArbiter = new ActionArbiter());


        #endregion

        #region Contructor

        static BaseThematisedControl()
        {
            TryAddBaseResources();
        }

        public BaseThematisedControl()
        {
            OverridesDefaultStyle = true;
            DefaultStyleKey = typeof(T);

            Loaded += SubscribeOnce;
        }


        #endregion

        #region Methods

        #region Private

        private void SubscribeOnce(object sender, RoutedEventArgs e)
        {
            Loaded -= SubscribeOnce;

            Dispatcher.Invoke(Subscribe, DispatcherPriority.Background);

            //Dispatcher.Invoke(UpdateDependencyValues, DispatcherPriority.Background);
        }

        private static void TryAddBaseResources()
        {
            var uri = new Uri(@"/WpfControls;component/Themes/Generic.xaml", UriKind.Relative);

            if (Application.LoadComponent(uri) is ResourceDictionary resourceDictionary)
            {
                var allMerged = Helper.Helper.SelectRecursive(Application.Current.Resources.MergedDictionaries,
                    x => x.MergedDictionaries);
                if (!allMerged.Contains(resourceDictionary, new ResourceDictonaryCompare()))
                {
                    Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                }
            }

        }

        #endregion

        #region Protected

        /// <summary>
        /// Тут подписываемся на события, так как контрол уже загружен
        /// </summary>
        protected virtual void Subscribe()
        {

        }

        /// <summary>
        /// Ищет именнованные контролы данного типа
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T1 TryFindTemplatePart<T1>(string name)
            where T1 : FrameworkElement
        {
            if (string.IsNullOrEmpty(name)
                || Template == null)
            {
                return null;
            }

            T1 result = Template.FindName(name, this) as T1;
            if (result != null)
                return result;

            if (Template.LoadContent() is FrameworkElement child)
            {
                result = child.FindName(name) as T1;
                if (result != null)
                    return result;
            }

            return null;
        }

        /// <summary>
        /// Обновляет значения всех пропертей
        /// </summary>
        protected void UpdateDependencyValues()
        {
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(this,
                new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) }))
            {
                var descr = DependencyPropertyDescriptor.FromProperty(pd);
                if (descr != null && !descr.IsReadOnly)
                {
                    var dp = descr.DependencyProperty;
                    var temp = GetValue(dp);

                    //SetValue(dp, dp.DefaultMetadata.DefaultValue);
                    //SetValue(dp, temp);

                    OnPropertyChanged(new DependencyPropertyChangedEventArgs(dp, dp.DefaultMetadata.DefaultValue, temp));
                }
            }
        }

        #endregion

        #endregion

    }
}
