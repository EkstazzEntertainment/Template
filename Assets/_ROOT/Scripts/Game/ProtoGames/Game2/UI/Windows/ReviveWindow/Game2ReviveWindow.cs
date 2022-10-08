namespace Ekstazz.ProtoGames.Game2.Ui.Windows
{
    using System;
    using System.Collections;
    using Ekstazz.LevelBased.Configs;
    using Ekstazz.Ui;
    using JetBrains.Annotations;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    
    public class Game2ReviveWindow : Window<Game2ReviveWindowOptions>
    {
        [Inject] private LevelBasedConfigs levelBasedConfigs;

        [SerializeField] private TMP_Text secondsLabel;
        [SerializeField] private Image fillImage;
        [SerializeField] private Button reviveButton;

        private string Placement => "Revive";
        private float startTime;
        private float time;
        private bool IsTimeUp => time <= 0;

        
        private void Start()
        {
            startTime = time = levelBasedConfigs.ReviveConfig.Duration;
            StartCoroutine(nameof(UpdateTimer));
            reviveButton.onClick.AddListener(TryRevive);
        }

        protected override void Prepare()
        {
        }

        private IEnumerator UpdateTimer()
        {
            while (true)
            {
                time -= Time.deltaTime;
                UpdateWindowView();

                if (IsTimeUp)
                {
                    Refuse();
                    yield break;
                }
                yield return null;
            }
        }

        private void UpdateWindowView()
        {
            secondsLabel.text = $"{Math.Ceiling(time)}";
            fillImage.fillAmount = time / startTime;
        }


        [UsedImplicitly]
        public void Refuse()
        {
            Options.OnReject.Invoke();
            Stop();
        }

        private async void TryRevive()
        {
            reviveButton.interactable = false;
            StopCoroutine(nameof(UpdateTimer));

            Revive();

            StartCoroutine(nameof(UpdateTimer));
        }

        private void Revive()
        {
            Stop();
            Options.OnSuccess.Invoke();
        }

        private void Stop()
        {
            reviveButton.interactable = false;
            StopCoroutine(nameof(UpdateTimer));
            Close();
        }
    }

    public class Game2ReviveWindowOptions : IWindowOptions
    {
        public Action OnReject;
        public Action OnSuccess;
    }
}