namespace Ekstazz.Saves
{
    using System;
    
    
    public interface ISaveComponent
    {
    }

    public class SaveComponentAttribute : Attribute
    {
        public string Name { get; set; }

        
        public SaveComponentAttribute(string name)
        {
            Name = name;
        }
    }
}