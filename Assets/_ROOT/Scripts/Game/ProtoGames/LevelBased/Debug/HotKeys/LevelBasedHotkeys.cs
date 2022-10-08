namespace Ekstazz.LevelBased.Debug
{
    using System;
    using Ekstazz.Input.HotKey;
    using Flow.Signals;
    using UnityEngine.InputSystem;
    using Zenject;

    
    public class LevelFailAction : KeyboardAction
    {
        [Inject] private SignalBus signalBus;

        public override Action Action => OnTriggered;

        public override Key Key => Key.F;

        private void OnTriggered()
        {
            signalBus.Fire<ILevelEnding>();
        }
    }

    public class LevelCompleteAction : KeyboardAction
    {
        [Inject]
        public SignalBus SignalBus { get; set; }

        public override Action Action => OnTriggered;

        public override Key Key => Key.C;

        private void OnTriggered()
        {
            SignalBus.Fire<ILevelCompleting>();
        }
    }

    public class LevelRestartAction : KeyboardAction
    {
        [Inject]
        public SignalBus SignalBus { get; set; }

        public override Action Action => OnTriggered;

        public override Key Key => Key.R;

        private void OnTriggered()
        {
            SignalBus.Fire<ILevelRestarting>();
        }
    }
}
