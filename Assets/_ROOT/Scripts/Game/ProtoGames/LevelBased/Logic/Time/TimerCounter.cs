namespace Ekstazz.LevelBased.Logic
{
    using Zenject;

    public interface ITimeCounter
    {
        float Time { get; }
        bool IsPaused { get; }
        void Init();
        void Pause();
        void Resume();
    }

    public class TimeCounter : ITimeCounter, ITickable
    {
        public float Time { get; private set; }
        public bool IsPaused { get; private set; }

        [Inject]
        public TimeCounter(TickableManager tickableManager)
        {
            tickableManager.Add(this);
        }
        
        public void Init()
        {
            Time = 0;
            IsPaused = false;
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }

        public void Tick()
        {
            Time += IsPaused ? 0 : UnityEngine.Time.unscaledDeltaTime;
        }
    }
}