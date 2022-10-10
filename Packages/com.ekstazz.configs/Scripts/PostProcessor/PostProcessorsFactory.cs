namespace Ekstazz.Configs
{
    using System;
    using System.Reflection;
    
    
    internal class PostProcessorsFactory
    {
        public virtual IConfigPostProcessor Create(Type postProcessorType)
        {
            if (!typeof(IConfigPostProcessor).IsAssignableFrom(postProcessorType))
            {
                throw new ArgumentException("Argument should be a subtype of IConfigPostProcessor", nameof(postProcessorType));
            }

            var constructorInfo = postProcessorType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { }, null);
            var pp = constructorInfo.Invoke(new object[] { });
            return (IConfigPostProcessor) pp;
        }
    }
}