namespace Ekstazz.ProtoGames.Utils
{
    using UnityEngine;
    using Zenject;

    public class MonoProvided<TSelf, TInterface> : MonoBehaviour where TSelf : MonoProvided<TSelf, TInterface>, TInterface
    {
        [Inject] private IRegistry<TInterface> registry;

        protected virtual void Start()
        {
            registry.Registry((TInterface)(TSelf)this);
        }
    }

    public class MonoProvided<TSelf> : MonoProvided<TSelf, TSelf> where TSelf : MonoProvided<TSelf, TSelf>
    {
    }

    public interface IProvider<out T>
    {
        T Instance { get; }
    }

    public interface IRegistry<in T>
    {
        void Registry(T instance);
    }

    public class Provider<T> : IProvider<T>, IRegistry<T> where T : class
    {
        public T Instance { get; private set; }

        public void Registry(T instance)
        {
            Instance = instance;
        }
    }
}
