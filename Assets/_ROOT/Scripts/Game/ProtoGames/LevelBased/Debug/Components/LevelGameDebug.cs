namespace Ekstazz.LevelBased.Debug
{
    using Flow.Signals;
    using ProtoGames;
    using TMPro;
    using UnityEngine;
    using Zenject;

    public class LevelGameDebug : MonoBehaviour
    {
        [Inject] private LazyInject<LevelGameProvider> levelGameProvider;
        [Inject] private SignalBus signalBus;

        [SerializeField] private TextMeshProUGUI levelGameName;

        
        private void Start()
        {
            SetUpCurrentLevelGameInfo();
            
            signalBus.Subscribe<ILevelStarted>(OnLevelStarted);
        }

        private void OnLevelStarted()
        {
            SetUpCurrentLevelGameInfo();
        }

        private void SetUpCurrentLevelGameInfo()
        {
            levelGameName.text = $"current level game : {levelGameProvider.Value.CurrentLevelGame.Id}";
        }

        private void OnDestroy()
        {
            signalBus.Unsubscribe<ILevelStarted>(OnLevelStarted);
        }
    }
}