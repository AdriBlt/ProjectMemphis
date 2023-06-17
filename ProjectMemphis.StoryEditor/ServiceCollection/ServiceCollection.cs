using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ProjectMemphis.StoryEditor.ServiceCollection
{
    /// <summary>
    /// Class which provide easy-access to a collection of service.
    /// </summary>
    public class ServiceCollection : IServiceCollection
    {
        private readonly Dictionary<Type, Object> _collection;

        public ServiceCollection()
        {
            _collection = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Add a new service to the services collection.
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <typeparam name="TO">Real type of the object to register</typeparam>
        /// <param name="o">The object to store in the collection</param>
        public void AddService<T,TO>(TO o) where T : class where TO : class, T
        {
            _collection.Add(typeof(T), o);
        }

        /// <summary>
        /// Add a new service to the services collection. Does not require any object, will instanciate the default constructor and store it.
        /// </summary>
        public void AddService<T, TO>() where T : class where TO : class, T
        {
            AddService<T, TO>(Activator.CreateInstance<TO>());
        }

        /// <summary>
        /// Add a new service to the services collection. Does not require any object, will instanciate the default constructor and store it.
        /// </summary>
        /// <typeparam name="T">Real type of the object to register.</typeparam>
        public void AddService<T>() where T : class, new()
        {
            AddService<T,T>(Activator.CreateInstance<T>());
        }

        /// <summary>
        /// Get a service based on the registered type.
        /// </summary>
        /// <typeparam name="T">The type of the object which was registered</typeparam>
        /// <returns>The instance of the object which was registered</returns>
        public T GetService<T>() where T : class
        {
            return _collection[typeof(T)] as T;
        }
    }
}
