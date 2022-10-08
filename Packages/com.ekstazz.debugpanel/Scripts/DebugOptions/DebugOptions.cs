namespace Ekstazz.Debug.DebugOptions
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class DebugOptions<TOption> where TOption : IDebugOption
    {
        public void AddOption(TOption option)
        {
#if DEBUG
            option.Init();
            if (Options.All(runtimeOption => runtimeOption.Name != option.Name))
            {
                Options.Add(option);
            }
#endif
        }

        public List<TOption> Options { get; } = new List<TOption>();
    }
}