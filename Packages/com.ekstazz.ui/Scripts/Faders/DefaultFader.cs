namespace Ekstazz.Ui.Faders
{
    using System;

    public class DefaultFader : UiFader
    {
        public override void FadeIn(Action onFinished = null)
        {
            onFinished?.Invoke();
        }

        public override void FadeOut(Action onFinished = null)
        {
            onFinished?.Invoke();
        }
    }
}
