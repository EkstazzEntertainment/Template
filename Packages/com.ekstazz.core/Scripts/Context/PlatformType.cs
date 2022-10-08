namespace Ekstazz.Core
{
    using System;

    [Flags]
    public enum PlatformType
    {
        Editor = 1,
        IOS = 2,
        Android = 4,
        Mobile = IOS | Android,
        Any = Editor | Mobile
    }
}
