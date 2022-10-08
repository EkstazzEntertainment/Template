namespace Ekstazz.Core
{
    using System;

    [Flags]
    public enum BuildType
    {
        Debug = 1, 
        Release = 2,
        Any = Debug | Release
    }
}
