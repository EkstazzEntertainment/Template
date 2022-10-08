namespace Ekstazz.LevelBased.Debug
{
    using Ekstazz.DebugPanel;
    using Ekstazz.LevelBased.Logic;
    using Logic;
    using ProtoGames;
    using TMPro;
    using UnityEngine;
    using Zenject;

    
    public class ChooseLevelDebug : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;
        [Inject] private ILevelProvider levelProvider;
        [Inject] private LevelGameProvider levelGameProvider;

        [SerializeField] private TMP_InputField field;
        [SerializeField] private TMP_Text currentLevelNumber;

        private const string Key = "debug_level";

        private int currentLevel;
        
        private int CurrentLevel
        {
            get => currentLevel;
            set
            {
                currentLevel = value;
                PlayerPrefs.SetInt(Key, currentLevel);
            }
        }

        
        private void Start()
        {
            currentLevel = PlayerPrefs.GetInt(Key, 2);
            field.text = CurrentLevel.ToString();
            field.onEndEdit.AddListener(OnEndEdit);
            currentLevelNumber.text = $"Lvl: <b>{levelProvider.CurrentLevelNumber}</b>";
        }

        private void OnEndEdit(string value)
        {
            var level = int.Parse(value);
            CurrentLevel = Mathf.Max(level, 1);
            field.text = CurrentLevel.ToString();
        }

        public void MoveTo()
        {
            levelProvider.MoveToNextLevel(CurrentLevel);
            levelGameProvider.CurrentLevelGame.FireLevelRestarting();
            var panel = FindObjectOfType<DebugPanel>();
            Destroy(panel.gameObject);
        }
    }
}