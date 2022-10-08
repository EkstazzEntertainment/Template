namespace Ekstazz.Saves.Data
{
    internal class InitialSaveData : SaveData
    {
        public override bool IsInitial => true;

        public InitialSaveData() : base(SaveParsingResult.Empty)
        {
        }
    }
}