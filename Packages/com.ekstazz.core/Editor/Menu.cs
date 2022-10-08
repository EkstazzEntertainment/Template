namespace Ekstazz.Core.Editor
{
    using System;
    using System.Collections.Generic;
    using Modules;
    using UnityEditor;
    using UnityEngine;

    public class Menu
    {
        [MenuItem("Ekstazz/Modules/Verify All")]
        public static void VerifyAllModules()
        {
            Debug.Log($"Verifying all modules");
            var moduleSearcher = new ModuleSearcher();
            Verify(moduleSearcher.FindAllModules());
        }

        [MenuItem("Ekstazz/Modules/Verify Active")]
        public static void VerifyActiveModules()
        {
            Debug.Log($"Verifying active modules");
            var moduleSearcher = new ModuleSearcher();
            Verify(moduleSearcher.FindActiveModules());
        }

        public static void VerifyModulesInBuilds()
        {
            FormatExtensions.LogTeamCity("Verifying active modules in build");
            var moduleSearcher = new ModuleSearcher();
            var areModulesValid = Verify(moduleSearcher.FindActiveModulesInBuild(), LogFormat.TeamCity);
            if (!areModulesValid)
            {
                Debug.Log("##teamcity[buildStatus status='FAILURE' text='Unity build completed successfully, but some modules had errors which may cause invalid behaviour']");
            }
        }

        private static bool Verify(List<IModuleInstaller> modules, LogFormat format = LogFormat.Unity)
        {
            var moduleVerifier = new ModulesVerifier();
            var areModulesValid = moduleVerifier.Verify(modules);
            moduleVerifier.PrintMessages(format);
            return areModulesValid;
        }
    }
}
