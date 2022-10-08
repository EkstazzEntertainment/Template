namespace Ekstazz.LevelBased.Views.Windows
{
    using Input;
    using Input.Drags;
    using Flow.Signals;
    using Ui;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;


    public abstract class HomeWindow : Window
    {
        [Inject] private IInputProvider inputProvider;
        [Inject] protected SignalBus SignalBus;

        [SerializeField] private float minDeltaToReact = 5;
        [SerializeField] private Button startButton;
        [SerializeField] private bool dragStart = false;


        protected override void OnWindowOpened()
        {
            base.OnWindowOpened();

            ConfigureStartButtonOrDrag();
        }

        private void ConfigureStartButtonOrDrag()
        {
            if (dragStart)
            {
                inputProvider.OnDrag += OnDrag;
                startButton.gameObject.SetActive(false);
            }
            else
            {
                startButton.gameObject.SetActive(true);
            }
        }

        private void OnDrag(Drag dragInfo)
        {
            if (dragInfo.OverallDelta.magnitude > minDeltaToReact)
            {
                inputProvider.OnDrag -= OnDrag;
                Close();
            }
        }

        public void StartGame()
        {
            Close();
        }

        protected override void BeforeClosing()
        {
            base.BeforeClosing();
            FireLevelStarted();
        }

        protected abstract void FireLevelStarted();
    }


    public class HomeWindow<T> : HomeWindow where T : ILevelStarted, new()
    {
        protected override void FireLevelStarted()
        {
            SignalBus.AbstractFire<T>();
        }
    }
}