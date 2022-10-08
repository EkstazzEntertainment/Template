namespace Ekstazz.ProtoGames
{
    using System.Collections.Generic;
    using System.Linq;
    using Zenject;

    public class LevelGameProvider
    {
        [Inject] private List<LevelGame> levelGames;

        public LevelGame CurrentLevelGame { get; private set; }

        public void ChangeCurrentGame(string gameId)
        {
            CurrentLevelGame = levelGames.First(l => l.Id == gameId);
        }

        public void AddLevelGames(List<LevelGame> levelGames)
        {
            this.levelGames = levelGames;
        }
    }
}