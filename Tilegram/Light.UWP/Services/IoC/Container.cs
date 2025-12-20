using System;
using System.Collections.Generic;

namespace Light.UWP.Services.IoC
{
    public class Container
    {
        private readonly Dictionary<Type, Func<object>> _registrations = new Dictionary<Type, Func<object>>();

        private static readonly Lazy<Container> _instance = new Lazy<Container>(() => new Container());
        private Container() { }

        public static Container Instance => _instance.Value;

        // Registro por tipo (crea instancia con Activator)
        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _registrations[typeof(TInterface)] = () => Activator.CreateInstance(typeof(TImplementation));
        }

        public void Register<TInterface, TImplementation>(Func<Container, TImplementation> func) where TImplementation : TInterface
        {
            _registrations[typeof(TInterface)] = () => func(this);
        }

        public void Register<TImplementation>(Func<Container, TImplementation> func)
        {
            _registrations[typeof(TImplementation)] = () => func(this);
        }

        // Resolver
        public TInterface Resolve<TInterface>()
        {
            if (_registrations.TryGetValue(typeof(TInterface), out var creator))
                return (TInterface)creator();

            return default(TInterface);
        }
    }
}
