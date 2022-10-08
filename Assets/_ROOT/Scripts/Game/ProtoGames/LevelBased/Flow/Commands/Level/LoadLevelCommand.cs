namespace Ekstazz.LevelBased.Flow
{
    using Ekstazz.LevelBased.SceneLoading;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class LoadLevelCommand : LockableCommand
    {
        [Inject] private ILevelParentProvider levelParentProvider;
        
        public override void Execute()
        {
            CreateLevelInstance();
        }

        private void CreateLevelInstance()
        {
            Lock();
            levelParentProvider.LevelParent.SetLevel(Unlock);
        }
    }
}