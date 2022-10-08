namespace Ekstazz.Game.Flow
{
    using System;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Shared class for working with splash screen in game
    /// </summary>
    public class Splash : ISplash
    {
        private SplashView SplashView { get; set; }

        public void Init()
        {
            SplashView = Object.FindObjectOfType<SplashView>();
            SplashView.Init();
        }

        public void Show(float duration = 0.4f)
        {
            Show(() => { }, duration);
        }

        public void Show(Action onComplete, float duration = 0.4f)
        {
            SplashView.Show(onComplete, duration);
        }

        public void Remove(float duration = 0.8f)
        {
            Remove(() => { }, duration);
        }

        public void Remove(Action onComplete, float duration = 0.9f)
        {
            SplashView.Remove(onComplete, duration);
        }
    }
}