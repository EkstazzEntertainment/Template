namespace Ekstazz.Game.Flow
{
    using System;

    public interface ISplash
    {
        void Init();
        void Show(Action onComplete, float duration = 0.4f);
        void Remove(Action onComplete, float duration = 0.9f);
    }
}