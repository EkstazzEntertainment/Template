namespace Ekstazz.Core.Modules
{
    using System;
    using JetBrains.Annotations;
    using UnityEngine.Scripting;

    [AttributeUsage(AttributeTargets.Class), MeansImplicitUse]
    public class AutoInstalledModuleAttribute : PreserveAttribute
    {
    }
}
