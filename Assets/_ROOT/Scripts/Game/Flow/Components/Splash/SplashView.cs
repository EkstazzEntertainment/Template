namespace Ekstazz.Game.Flow
{
    using System;
    using UnityEngine;

    public abstract class SplashView : MonoBehaviour
    {
        public abstract void Init();
        public abstract void Show(Action onComplete, float duration);
        public abstract void Remove(Action onComplete, float duration);
    }
}