using BelegApp.Forms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelegApp.Forms.Tests
{
    class DependencyServiceMock : IDependencyService
    {
        private readonly IDictionary<Type, object> registeredServices = new Dictionary<Type, object>();

        public void Register<T>(object impl)
        {
            this.registeredServices[typeof(T)] = impl;
        }

        public T Get<T>() where T : class
        {
            return (T)registeredServices[typeof(T)];
        }
    }
}
