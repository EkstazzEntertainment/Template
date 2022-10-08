namespace Ekstazz.ProtoGames.Flow
{
    using UnityEngine;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class DestroyCurrentCharacterCommand : LockableCommand
    {
        [Inject] private LevelGameProvider levelGameProvider;
        
        public override void Execute()
        {
            var playerView = levelGameProvider.CurrentLevelGame.LevelViewFinder.CurrentLevelView.PlayerView;
            Object.Destroy(playerView.gameObject);
        }
    }
}