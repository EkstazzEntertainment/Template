namespace Ekstazz.Game.Flow
{
    using System.Collections;
    using System.Linq;
    using Ekstazz.Utils.Coroutine;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public abstract class SwitchSceneCommand : LockableCommand
    {
        [Inject] private ICoroutineProvider coroutineProvider;

        protected abstract string Scene { get; }
        protected virtual bool BlockExecution => true;

        
        public override void Execute()
        {
            coroutineProvider.StartCoroutine(Load());
            if (BlockExecution)
            {
                Lock();
            }
        }

        private IEnumerator Load()
        {
            var scenes = Enumerable.Range(0, SceneManager.sceneCount).Select(SceneManager.GetSceneAt);
            foreach (var scene in scenes)
            {
                if (scene.name == Scenes.Loader || scene.name == Scenes.Splash)
                {
                    continue;
                }

                yield return SceneManager.UnloadSceneAsync(scene);
            }

            var op = SceneManager.LoadSceneAsync(Scene, LoadSceneMode.Additive);
            while (!op.isDone)
            {
                // LoadingStageProgressChanged.Dispatch(LoadingStageType.LoadingScene, (int)(op.progress * 100));
                yield return new WaitForSeconds(0.1f);
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(Scene));
            // LoadingStageProgressChanged.Dispatch(LoadingStageType.LoadingScene, 100);
            Unlock();
        }
    }

    public class SwitchToGameCommand : SwitchSceneCommand
    {
        protected override string Scene => Scenes.Game;
    }

    public class StartGameSceneLoadingCommand : SwitchSceneCommand
    {
        protected override string Scene => Scenes.Game;
        protected override bool BlockExecution => false;
    }

    public class UnloadGameSceneCommand : LockableCommand
    {
        [Inject] private ICoroutineProvider coroutineProvider;

        public override void Execute()
        {
            Lock();
            coroutineProvider.StartCoroutine(Unload());
        }

        private IEnumerator Unload()
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(Scenes.Game));
            Unlock();
        }
    }
}
