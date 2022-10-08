namespace Ekstazz.LevelBased.SceneLoading
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ekstazz.LevelBased.Logic;
    using Ekstazz.ProtoGames;
    using Ekstazz.ProtoGames.Level;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;
    using Zenject;

    
    public class AddressableSceneLevelParent : MonoBehaviour, ILevelParent
    {
        [Inject] private ILevelParentContainer levelParentContainer;
        [Inject] private LevelGameProvider levelGameProvider;

        private Scene initialActiveScene;
        private SceneInstance sceneInstance;
        private Area areaInstance;
        
        private AsyncOperationHandle<SceneInstance> currentLoadingHandle;
        private Queue<AsyncOperationHandle<SceneInstance>> loadedSceneInstances = new Queue<AsyncOperationHandle<SceneInstance>>();

        public Area Area => areaInstance;

        
        private void Start()
        {
            initialActiveScene = gameObject.scene;
            levelParentContainer.UpdateParentTo(this);
        }

        public async Task PreloadLevel(string sceneId)
        {
            var levelSceneName = sceneId;
            var levelScenePath = string.Format(levelGameProvider.CurrentLevelGame.ScenesPath, levelSceneName);
            currentLoadingHandle = Addressables.LoadSceneAsync(levelScenePath, LoadSceneMode.Additive, false);
            loadedSceneInstances.Enqueue(currentLoadingHandle);
            await currentLoadingHandle.Task;
        }

        public async Task SetLevel(Action callback)
        {
            await currentLoadingHandle.Task;
            sceneInstance = currentLoadingHandle.Result;
            await sceneInstance.ActivateAsync();

            var scene = sceneInstance.Scene;
            var mainParentInScene = scene
                .GetRootGameObjects()
                .First(x => x.name == nameof(SceneLocationMainParent));
            
            areaInstance = mainParentInScene.GetComponentInChildren<Area>();
            // SceneManager.SetActiveScene(scene); //uncomment this to set the lighting settings to the newly created scene
            
            levelGameProvider.CurrentLevelGame.Container.InjectGameObject(mainParentInScene);
            mainParentInScene.SetActive(true);

            callback?.Invoke();
        }
        
        public async Task UnloadLastGameScene()
        {
            if (loadedSceneInstances.Count <= 1)
            {
                return;
            }

            SceneManager.SetActiveScene(initialActiveScene);
            await Addressables.UnloadSceneAsync(loadedSceneInstances.Dequeue()).Task;
        }
    }
}
