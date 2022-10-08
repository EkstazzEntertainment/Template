namespace Ekstazz.Game.Flow
{
    using System;

    public class SimpleSplash : SplashView
    {
        public override void Init()
        {
        }

        public override void Show(Action onComplete, float duration)
        {
            gameObject.SetActive(true);
        }

        public override void Remove(Action onComplete, float duration)
        {
            gameObject.SetActive(false);
        }
    }
}