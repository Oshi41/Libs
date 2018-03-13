using System;
using System.Collections.Generic;
using System.Linq;

namespace IocContainer
{
    public class IoCContainer
    {
        private readonly Dictionary<RegistrationInfo, Func<object>> _map = new Dictionary<RegistrationInfo, Func<object>>();
        private static IoCContainer _instance;
        public static IoCContainer Instance => _instance ?? (_instance = new IoCContainer());


        private IoCContainer()
        {
            
        }

        #region Use date time now

        private void RegisterInstanceByType(Type type)
        {
            var info = new RegistrationInfo(type, DateTime.Now);
            var instance = Activator.CreateInstance(type);

            _map.Add(info, () => instance);
        }

        private void RegisterInstanceByType<T>()
        {
            RegisterInstanceByType(typeof(T));
        }

        private void RegisterExistingInstance<T>(T o)
        {
            var info = new RegistrationInfo(typeof(T), DateTime.Now);

            _map.Add(info, () => o);
        }

        #endregion

        #region Fast

        private void RegisterInstanceByTypeFast(Type type)
        {
            var info = new RegistrationInfo(type);
            var instance = Activator.CreateInstance(type);

            _map.Add(info, () => instance);
        }

        private void RegisterInstanceByTypeFast<T>()
        {
            RegisterInstanceByTypeFast(typeof(T));
        }

        private void RegisterExistingInstanceFast<T>(T o)
        {
            var info = new RegistrationInfo(typeof(T));

            _map.Add(info, () => o);
        }
        #endregion

        #region Strongly naming

        private void RegisterInstanceByName(Type type, string name)
        {
            var info = new RegistrationInfo(type, DateTime.Now);
            var instance = Activator.CreateInstance(type);

            _map.Add(info, () => instance);
        }

        #endregion

        #region Searching

        public bool ContainsType(Type type)
        {
            if (!_map.Any())
                return false;

            if (_map.Any(x => x.Key.ContainsType(type)))
                return true;

            return false;
        }

        public bool ContainsName(string name)
        {
            if (!_map.Any())
                return false;

            if (_map.Any(x => x.Key.ContainsName(name)))
                return true;

            return false;
        }

        public object FindByName(string name)
        {
            if (!_map.Any())
                return null;

            if (string.IsNullOrEmpty(name))
                return null;

            var find = _map.FirstOrDefault(x => Equals(x.Key.InstanceName, name));

            return find.Value?.Invoke();
        }

        public object FindInstance(Type type, string name)
        {
            if (!_map.Any())
                return null;

            if (string.IsNullOrEmpty(name))
            {
                // нашёл точный тип
                var first = _map.FirstOrDefault(x => x.Key.Type == type);
                if (first.Value != null)
                    return first.Value?.Invoke();

                // нашёл производные типы
                var find = _map.FirstOrDefault(x => x.Key.ContainsType(type));
                return find.Value?.Invoke();
            }
            else
            {
                return FindByName(name);
            }
        }

        public T Resolve<T>()
        {
            var find = FindInstance(typeof(T), null);
            if (find is T result)
                return result;

            return default(T);
        }

        //public T ResolveByType<T>()
        //{
        //    if (!_map.Any())
        //        return default(T);

        //    var first = _map.FirstOrDefault(x => x.Key.Type == typeof(T));
        //    if (first.Value != null)
        //        return (T)first.Value();
        //}

        #endregion
    }
}
