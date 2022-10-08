namespace Ekstazz.LevelBased.SceneLoading
{
    using Ekstazz.LevelBased.Flow.Signals;
    using UnityEngine;
    using Zenject;


    public class MonoBehaviorISC : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;

        [Inject]
        public void Initialization()
        {
            signalBus.Subscribe<SubContainersInjected>(OnSubContainersInjected);
            signalBus.Subscribe<ILevelCompleting>(OnFinish);
            signalBus.Subscribe<ILevelFailing>(OnFinish);
            signalBus.Subscribe<ILevelRestarting>(OnFinish);
        }

        protected virtual void OnSubContainersInjected()
        {
        }

        private void OnFinish()
        {
            signalBus.TryUnsubscribe<SubContainersInjected>(OnSubContainersInjected);
            signalBus.TryUnsubscribe<ILevelCompleting>(OnFinish);
            signalBus.TryUnsubscribe<ILevelFailing>(OnFinish);
            signalBus.TryUnsubscribe<ILevelRestarting>(OnFinish);
        }
    }
}