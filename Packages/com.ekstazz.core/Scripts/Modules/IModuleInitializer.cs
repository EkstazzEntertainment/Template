namespace Ekstazz.Core.Modules
{
    public interface IModuleInitializer
    {
        /// <summary>
        /// Called right at time when logic components need to be initialized
        /// </summary>
        void Prepare();
    }
}
