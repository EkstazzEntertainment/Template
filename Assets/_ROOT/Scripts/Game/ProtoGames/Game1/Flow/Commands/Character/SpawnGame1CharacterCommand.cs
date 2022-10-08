namespace Ekstazz.ProtoGames.Game1.Flow
{
    using System.Linq;
    using ProtoGames.Character;
    using ProtoGames.Level;
    using ProtoGames.Settings;
    using Settings;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class SpawnGame1CharacterCommand : LockableCommand
    {
        [Inject] private LevelGameProvider levelGameProvider;
        [Inject] private PrefabSettings prefabSettings;


        public override void Execute()
        {
            var levelView = GetLevelView();
            var prefab = prefabSettings.players
                .First(i => i.gameId == levelGameProvider.CurrentLevelGame.Id).player;
            var player = levelGameProvider.CurrentLevelGame.CharacterFactory.Value.Create(prefab);
            SetUpPlayer(levelView, player);
        }

        protected virtual void SetUpPlayer(LevelView levelView, CharacterView player)
        {
            levelView.InitPlayer(player);
            var spawnPoint = levelView.PlayerSpawnPoint;
            var tr = player.transform;
            tr.position = spawnPoint.position;
            tr.SetParent(spawnPoint);
            tr.rotation = spawnPoint.rotation;
        }

        private LevelView GetLevelView()
        {
            var levelViewFinder = levelGameProvider.CurrentLevelGame.LevelViewFinder; 
            return levelViewFinder?.CurrentLevelView;
        }
    }
}