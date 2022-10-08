namespace Ekstazz.LevelBased.Flow
{
    using System.Collections;
    using Ekstazz.LevelBased.Configs;
    using Ekstazz.Utils.Coroutine;
    using UnityEngine;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public abstract class WaitAfterLevelEndingCommand : LockableCommand
    {
        [Inject] protected ICoroutineProvider CoroutineProvider;
        [Inject] protected LevelBasedConfigs LevelBasedConfigs;

        protected abstract float Seconds { get; }

        
        public override void Execute()
        {
            CoroutineProvider.StartCoroutine(WaitUntilEnd());
        }

        private IEnumerator WaitUntilEnd()
        {
            Lock();
            yield return new WaitForSeconds(Seconds);
            Unlock();
        }
    }

    public class WaitAfterLevelCompletingCommand : WaitAfterLevelEndingCommand
    {
        protected override float Seconds => LevelBasedConfigs.LevelSettings.LevelCompleteDelay;
    }

    public class WaitAfterLevelFailingCommand : WaitAfterLevelEndingCommand
    {
        protected override float Seconds => LevelBasedConfigs.LevelSettings.LevelFailDelay;
    }
}