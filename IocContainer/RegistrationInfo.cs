using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocContainer
{
    public class RegistrationInfo
    {
        #region Properties

        /// <summary>
        /// Имя инстанса
        /// </summary>
        public string InstanceName { get;   set; }

        /// <summary>
        /// Регистрируемый тип
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Время регистрации
        /// </summary>
        public DateTime RegistrationTime { get; private set; }

        #endregion

        #region Constructors

        public RegistrationInfo(Type type,
            string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = Guid.NewGuid().ToString();

            InstanceName = name;
            Type = type;
        }

        public RegistrationInfo(Type type,
            DateTime time,
            string name = null)
            : this(type, name)
        {
            RegistrationTime = DateTime.Now;
        }

        #endregion

        #region Methods

        public bool ContainsName(string name)
        {
            var result = Equals(name, InstanceName);
            return result;
        }

        public bool ContainsType(Type type)
        {
            if (Type == type)
                return true;

            if (Type.IsAssignableFrom(type))
                return true;

            return type.IsSubclassOf(Type);
        }

        public bool ContainsType<T>()
        {
            return ContainsType(typeof(T));
        }

        #endregion

        #region Overrided

        public override bool Equals(object obj)
        {
            if (!(obj is RegistrationInfo info))
                return false;

            var resut = Equals(InstanceName, info.InstanceName)
                        && Type == info.Type
                        && Equals(RegistrationTime, info.RegistrationTime);
            return resut;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = InstanceName.GetHashCode();
                result ^= Type.GetHashCode();
                result ^= RegistrationTime.GetHashCode();
                return result;
            }
        }

        public override string ToString()
        {
            var result =
                $"Name is: {InstanceName}, of type {Type}, registered at {RegistrationTime.ToShortDateString()}";
            return result;
        }

        #endregion
    }
}
