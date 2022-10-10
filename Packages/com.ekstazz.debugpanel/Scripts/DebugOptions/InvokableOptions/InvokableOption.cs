namespace Ekstazz.Debug.DebugOptions
{
    using System;
    using System.Collections.Generic;

    
    public class InvokableOption : IDebugOption
    {
        public string Name { get; set; }
        public List<NamedInvocation> InvocationsList { get; set; }
        
        
        public void Init()
        {
        }

        public class NamedInvocation
        {
            public string Name { get; }
            public Action OnInvoked { get; }

            
            public NamedInvocation(string name, Action onInvoked)
            {
                Name = name;
                OnInvoked = onInvoked;
            }
        }
    }
}