namespace Ekstazz.LevelBased.Logic
{
    using System;
    using System.Threading.Tasks;
    using Ekstazz.LevelBased.Logic;

    public interface ILevelParent
    {
        Area Area { get; }
        public Task PreloadLevel(string sceneId);
        public Task SetLevel(Action callback);
        public Task UnloadLastGameScene();
    }
}