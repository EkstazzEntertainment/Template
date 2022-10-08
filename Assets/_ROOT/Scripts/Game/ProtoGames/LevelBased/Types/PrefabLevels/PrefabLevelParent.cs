namespace Ekstazz.LevelBased.PrefabLevels
{
    using Logic;
    using Logic.State;
    using ProtoGames;
    using Ekstazz.LevelBased.Logic;
    using Ekstazz.ProtoGames;
    using UnityEngine;
    using Zenject;
    using Zenject.Extensions.Lazy;

    public class PrefabLevelParent : InjectableView<PrefabLevelParent>
    {
        [Inject] private ILevelStateProvider levelStateProvider;
        [Inject] private LevelGameProvider levelGameProvider;

        [Header("Settings")]
        [SerializeField] private bool forceRecreateLevel = false;

        public Area AreaInstance { get; private set; }

        private bool isAreaInstanceUsed;

        
        private void Start()
        {
            levelStateProvider.LevelStateChanged += OnLevelStateChanged;
        }

        public void SetLevel(Level level)
        {
            if (CheckIfCanKeepAreaFor(level) == false)
            {
                TryCleanUp();
                AreaInstance = levelGameProvider.CurrentLevelGame.PrefabLevelFactory.Create(level.AreaPrefab, transform);
                isAreaInstanceUsed = false;
            }
        }

        private void OnLevelStateChanged(LevelState fromState, LevelState toState)
        {
            if (toState == LevelState.Started)
            {
                isAreaInstanceUsed = true;
            }
        }

        private bool CheckIfCanKeepAreaFor(Level level)
        {
            if (!AreaInstance)
            {
                return false;
            }

            var sameArea = level.AreaPrefab.AreaId == AreaInstance.AreaId;
            return !forceRecreateLevel && sameArea && !isAreaInstanceUsed;
        }

        private void TryCleanUp()
        {
            if (AreaInstance)
            {
                Destroy(AreaInstance.gameObject);
            }
        }
    }
}