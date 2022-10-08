using System;

namespace Ekstazz.Ui.Windows
{
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
