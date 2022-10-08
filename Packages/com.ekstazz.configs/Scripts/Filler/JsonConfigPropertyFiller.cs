namespace Ekstazz.Configs
{
    using System;
    using System.Reflection;

    internal class JsonConfigPropertyFiller : ConfigPropertyFiller
    {
        protected override void ThrowIfTypeOfPropertyIsNotCorrect(PropertyInfo propertyInfo)
        {
            // Well it's kind of always correct
        }

        protected override MethodInfo GetParserMethodForTypeOf(PropertyInfo propertyInfo)
        {
            if (Parser is null)
            {
                throw new NullReferenceException("Parser is null. Property filler wasn't initialized properly");
            }
            
            if (propertyInfo is null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }
            
            var propertyType = propertyInfo.PropertyType;
            var openGenericMethodInfo = Parser.GetType().GetMethod(nameof(Parser.ParseJson), BindingFlags.Public | BindingFlags.Instance);

            if (openGenericMethodInfo is null)
            {
                throw new Exception("No Parser.ParseJson() method found");
            }
            return openGenericMethodInfo.MakeGenericMethod(propertyType);
        }
    }
}