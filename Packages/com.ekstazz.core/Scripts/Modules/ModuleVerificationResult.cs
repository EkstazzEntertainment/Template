namespace Ekstazz.Core.Modules
{
    using System.Collections.Generic;
    using System.Linq;

    public class ModuleVerificationResult
    {
        public bool Verified => Messages.All(m => m.severity <= MessageSeverity.Warning);

        public List<VerificationMessage> Messages { get; }

        private ModuleVerificationResult(List<VerificationMessage> messages)
        {
            Messages = messages;
        }

        public static ModuleVerificationResult Valid => new ModuleVerificationResult(new List<VerificationMessage>());

        public static ModuleVerificationResult Log(string message) => Logs(new List<string>() { message });

        public static ModuleVerificationResult Logs(List<string> messages) =>
            FromMessages(messages.Select(m => new VerificationMessage(MessageSeverity.Log, m)).ToList());

        public static ModuleVerificationResult Warning(string message) => Warnings(new List<string>() { message });

        public static ModuleVerificationResult Warnings(List<string> messages) =>
            FromMessages(messages.Select(m => new VerificationMessage(MessageSeverity.Warning, m)).ToList());

        public static ModuleVerificationResult HasError(string message) => HasErrors(new List<string>() { message });

        public static ModuleVerificationResult HasErrors(List<string> messages) =>
            FromMessages(messages.Select(m => new VerificationMessage(MessageSeverity.Error, m)).ToList());

        public static ModuleVerificationResult FromMessages(List<VerificationMessage> messages) =>
            new ModuleVerificationResult(messages);
    }

    public class VerificationMessage
    {
        public readonly MessageSeverity severity;
        public readonly string message;

        public VerificationMessage(MessageSeverity severity, string message)
        {
            this.severity = severity;
            this.message = message;
        }
    }

    public enum MessageSeverity
    {
        Log,
        Warning,
        Error
    }
}
