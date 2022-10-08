namespace Ekstazz.Ui.Faders
{
    using System;
    using UnityEngine;

    public abstract class UiFader : MonoBehaviour
    {
        public abstract void FadeIn(Action onFinished = null);

        public abstract void FadeOut(Action onFinished = null);
    }
}
