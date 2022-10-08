namespace Ekstazz.LevelBased.Debug
{
    using ProtoGames;
    using UnityEngine;
    using Zenject;

    public class LevelDebug : MonoBehaviour
    {
        [Inject] private LevelGameProvider levelGameProvider;

        
        public void Complete()
        {
            levelGameProvider.CurrentLevelGame.FireLevelCompleting();
        }
        
        public void Restart()
        {
            levelGameProvider.CurrentLevelGame.FireLevelRestarting();
        }

        public void Fail()
        {
            levelGameProvider.CurrentLevelGame.FireLevelFailing();
        }
    }
}