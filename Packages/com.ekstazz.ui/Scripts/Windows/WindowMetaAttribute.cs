namespace Ekstazz.Ui.Windows
{
    using System;

    
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class WindowMetaAttribute : Attribute
    {
        public readonly string Id;

        public WindowMetaAttribute(string id)
        {
            Id = id;
        }
    }
}
