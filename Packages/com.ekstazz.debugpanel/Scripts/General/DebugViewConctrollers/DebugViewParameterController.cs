namespace Ekstazz.DebugPanel
{
    public class DebugViewParameterController<TInstance, TValue> where TInstance : DebugViewParameter<TValue>
    {
        private TInstance instance;
        private TValue value;

        
        public void SetInstance(TInstance instance)
        {
            this.instance = instance;
            Synchronize();
        }
        
        public void SetNewAlpha(TValue value)
        {
            this.value = value;
            Synchronize();
        }

        private void Synchronize()
        {
            if (instance != null)
            {
                instance.ApplyValue(value);
            }
        }
    }
}