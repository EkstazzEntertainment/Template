namespace Zenject.Extensions.Lazy
{
    using UnityEngine;
    using Zenject;

    public class InjectableView<T> : MonoBehaviour where T : InjectableView<T>
    {
        [Inject]
        public DiContainer Container { get; set; }

        protected virtual void Awake()
        {
            // When starting from GameScene in Editor SceneContext is not being autorun
            // and Container is not injected here.
#if UNITY_EDITOR
            if (Container == null)
            {
                return;
            }
#endif
            
            Container.ParentContainers[0].Rebind<T>().FromInstance((T) this).AsCached();
            Container.QueueForInject(this);
        }
    }
}
