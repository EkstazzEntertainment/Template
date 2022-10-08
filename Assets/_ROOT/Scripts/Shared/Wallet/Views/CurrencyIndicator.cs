namespace Ekstazz.Wallet.Views
{
    using DG.Tweening;
    using Currencies;
    using Ekstazz.Game.Flow;
    using TMPro;
    using UnityEngine;
    using Zenject;

    
    public class CurrencyIndicator : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;
        [Inject] private IWallet wallet;
        [Inject] private GameStartedAwaiter gameStartedAwaiter;

        [SerializeField, Currency] protected string currencyName;
        [SerializeField] private TMP_Text counter;

        private double value;
        private ICurrencyType currency;

        
        private void Start()
        {
            gameStartedAwaiter.ExecuteAfterGameStarted(OnGameStart);
        }

        private void OnGameStart()
        {
            currency = MoneyExt.Factory.TypeOf(currencyName);

            signalBus.Subscribe<TransactionCompleted>(OnAccountChanged);
            OnAccountChanged(new TransactionCompleted
            {
                Type = currency,
                Value = wallet.GetAmountOf(currency),
            });
        }

        private void OnAccountChanged(TransactionCompleted transaction)
        {
            var currencyType = transaction.Type;
            var newValue = transaction.Value;

            if (currencyType != currency)
            {
                return;
            }

            DOTween.Kill(gameObject);

            DOTween.To(() => value, UpdateValue, newValue, 0.2f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => counter.SetText(newValue.ToString()));
            value = newValue;
        }

        private void UpdateValue(double amount)
        {
            value = amount;
            counter.SetText(((Amount)amount).ToString());
        }

        private void OnDestroy()
        {
            signalBus.TryUnsubscribe<TransactionCompleted>(OnAccountChanged);
        }
    }
}
