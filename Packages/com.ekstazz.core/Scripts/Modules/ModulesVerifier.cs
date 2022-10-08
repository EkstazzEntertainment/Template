namespace Ekstazz.Core.Modules
{
    using System.Collections.Generic;
    using System.Linq;

    public class ModulesVerifier
    {
        public Dictionary<string, ModuleVerificationResult> ModuleVerification { get; } =
            new Dictionary<string, ModuleVerificationResult>();

        public bool AreModulesValid { get; set; }

        public bool Verify(List<IModuleInstaller> modules)
        {
            AreModulesValid = true;
            foreach (var module in modules)
            {
                var result = module.Verifier.Verify();
                ModuleVerification[module.Name] = result;
                if (!result.Verified)
                {
                    AreModulesValid = false;
                }
            }
            return AreModulesValid;
        }

        public void PrintMessages(LogFormat format = LogFormat.Unity)
        {
            PrintGeneralInformation(format);
            foreach (var pair in ModuleVerification)
            {
                var module = pair.Key;
                var result = pair.Value;
                if (HasMessages(result))
                {
                    PrintModuleInfo(module, result, format);
                    PrintVerificationMessage(result, format);
                }
            }
        }

        private void PrintGeneralInformation(LogFormat format)
        {
            if (AreModulesValid)
            {
                var logValid = format.GetLogMethod(MessageSeverity.Log);
                var messageCount = ModuleVerification.Aggregate(0, (acc, r) => acc + r.Value.Messages.Count);
                var additionalMessage = messageCount > 0 ? "See next logs for additional information" : "";
                logValid($"Modules are valid. {additionalMessage}");
                return;
            }
            var log = format.GetLogMethod(MessageSeverity.Error);
            log("Some modules weren't set up correctly. See next messages to find errors:");
        }

        private bool HasMessages(ModuleVerificationResult result)
        {
            return result.Messages.Count > 0;
        }

        private void PrintModuleInfo(string module, ModuleVerificationResult result, LogFormat format)
        {
            var moduleSeverity = result.Messages.Aggregate(MessageSeverity.Log,
                (s, m) => m.severity > s ? m.severity : s);
            var log = format.GetLogMethod(moduleSeverity);
            log($"{module}:");
        }

        private void PrintVerificationMessage(ModuleVerificationResult result, LogFormat format)
        {
            foreach (var message in result.Messages)
            {
                var log = format.GetLogMethod(message.severity);
                log($"\t{message.message}");
            }
        }
    }
}
