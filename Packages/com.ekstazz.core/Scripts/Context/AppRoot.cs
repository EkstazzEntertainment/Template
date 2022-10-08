namespace Ekstazz.Core
{
    using UnityEngine;
    using Zenject;

    public class AppRoot : MonoBehaviour
    {
        [Inject]
        public SignalBus SignalBus { get; set; }

        private void Start()
        {
            Application.runInBackground = true;
            //without this line, iOS default FPS will be set to 30
            Application.targetFrameRate = 60;

            DontDestroyOnLoad(gameObject);
            SignalBus.Fire<StartApp>();
        }
    }
}
