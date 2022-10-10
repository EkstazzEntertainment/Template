namespace Ekstazz.Debug.DebugOptions
{
    public interface IDebugOption
    {
        public void Init();
        public string Name { get; set; }
    }
}