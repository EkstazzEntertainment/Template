namespace Ekstazz.Saves.Worker
{
    using Data;

    internal interface ISaveWorker
    {
        void Write(SaveData saveData);
    }
}