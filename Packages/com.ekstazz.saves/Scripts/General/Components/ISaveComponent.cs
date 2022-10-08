namespace Ekstazz.Saves
{
    using System;

    /// <summary>
    /// interface for partial saves such as DailyBonus, or tutorial
    /// which must be combined into one big SaveModel and saved
    /// </summary>
    public interface ISaveComponent
    {
    }

    public class SaveComponentAttribute : Attribute
    {
        /// <summary>
        /// This Name will be used as a top-level key in JSON tree of save.
        /// Name doesn't included in ISaveComponent interface to avoid using JsonIgnore in
        /// every component
        /// </summary>
        public string Name { get; set; }

        public SaveComponentAttribute(string name)
        {
            Name = name;
        }
    }
}