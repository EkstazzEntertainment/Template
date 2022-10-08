namespace Ekstazz.LevelBased.Saves
{
    using Ekstazz.Saves;

    [SaveComponent("LevelSave")]
    public class LevelSave : ISaveComponent
    {
        public int Level { get; set; }
    }
}