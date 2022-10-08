namespace Ekstazz.DebugPanel
{
    using UnityEngine;

    public abstract class DebugViewParameter<TValue> : MonoBehaviour
    {
        public abstract void ApplyValue(TValue value);
    }
}