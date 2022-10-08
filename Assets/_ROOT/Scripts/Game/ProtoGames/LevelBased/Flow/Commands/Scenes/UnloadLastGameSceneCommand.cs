namespace Ekstazz.LevelBased.Flow
{
    using System.Threading.Tasks;
    using Ekstazz.LevelBased.SceneLoading;
    using Zenject;
    using UnityEngine;
    using Zenject.Extensions.Commands;

    
    public class UnloadLastGameSceneCommand : Command
    {
        [Inject] private ILevelParentProvider levelParentProvider;
        
        public override async Task Execute()
        {
            Debug.Log($"<color=green>Unloading level</color>");
            await levelParentProvider.LevelParent.UnloadLastGameScene();
        }
    }
}