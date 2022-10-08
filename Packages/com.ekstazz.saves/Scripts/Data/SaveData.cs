namespace Ekstazz.Saves.Data
{
    /// <summary>
    /// Contains SaveModel and some save metadata we may need to know
    /// (such as is it save local or not)
    /// </summary>
    internal abstract class SaveData
    {
        public virtual bool IsLocalSave { get; }

        public virtual bool IsInitial { get; }

        public SaveModel Result { get; set; }

        public bool IsEmpty => Result == null || Result.Components.Count == 0;
        
        public bool HasErrors { get; private set; }

        public bool SaveCameFromNewerBuild => Result != null && Result.CameFromNewBuild;
        
        protected SaveData(SaveParsingResult result)
        {
            Result = result.SaveModel;
            HasErrors = result.HasErrors;
        }
        
        protected SaveData(SaveModel model)
        {
            Result = model;
        }
    }
}