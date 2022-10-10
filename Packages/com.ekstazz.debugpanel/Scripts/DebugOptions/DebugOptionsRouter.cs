namespace Ekstazz.Debug.DebugOptions
{
    using Zenject;

    
    public class DebugOptionsRouter
    {
        [Inject] private RuntimeOptions RuntimeOptions { get; set; }
        [Inject] private ToggleOptions ToggleOptions { get; set; }
        [Inject] public InvokableOptions InvokableOptions { get; set; }

        
        public void AddOption(RuntimeOption option)
        {
            RuntimeOptions.AddOption(option);
        }
        
        public void AddOption(ToggleOption option)
        {
            ToggleOptions.AddOption(option);
        }

        public void AddOption(InvokableOption option)
        {
            InvokableOptions.AddOption(option);
        }
    }
}