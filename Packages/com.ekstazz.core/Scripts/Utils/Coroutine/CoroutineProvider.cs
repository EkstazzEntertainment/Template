namespace Ekstazz.Utils.Coroutine
{
    using System.Collections;
    using UnityEngine;

    public interface ICoroutineProvider
    {
        Coroutine StartCoroutine(IEnumerator enumerator);

        void StopCoroutine(Coroutine coroutine);
    }

    public class CoroutineProvider : MonoBehaviour, ICoroutineProvider
    {
    }
}
