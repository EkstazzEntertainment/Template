namespace Ekstazz.Input.HotKey
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Zenject;

    public interface IHotKeyDebug
    {
        void RegisterAction(DebugAction action);
    }

    public class HotKeyDebug : IHotKeyDebug, IInitializable, ITickable
    {
        [Inject] private DiContainer container;

        private readonly List<DebugAction> actions = new List<DebugAction>();

        
        public void Initialize()
        {
        }

        public void RegisterAction(DebugAction action)
        {
            container.Inject(action);
            actions.Add(action);
        }

        public void Tick()
        {
#if UNITY_EDITOR
            UpdateActions();
#endif
        }

        private void UpdateActions()
        {
            foreach (var action in actions.Where(action => action.IsTriggered))
            {
                Debug.Log($"<color=cyan>{action}</color>");
                action.Action();
            }
        }
    }
}