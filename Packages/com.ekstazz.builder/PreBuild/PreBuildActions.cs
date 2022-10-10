namespace PreBuild
{
#if UNITY_EDITOR

    using System;
    using System.Linq;
    using Ekstazz.Editor.Build;
    using UnityEngine;


    public class PreBuildActions
    {
        public static void ExecutePreBuildActions()
        {
            var interfaceType = typeof(IPreProjectBuilderAction);
            var preBuildHooksTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => interfaceType.IsAssignableFrom(type) && type != interfaceType).ToList();

            foreach (var action in preBuildHooksTypes)
            {
                var instance = Activator.CreateInstance(action) as IPreProjectBuilderAction;

                Debug.Log($"Executing a pre build action: {action}");
                instance?.Execute();
            }
        }
    }
    
#endif

}
