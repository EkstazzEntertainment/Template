namespace Ekstazz.Wallet.Debug
{
    using Ekstazz.Wallet;
    using Ekstazz.Currencies;
    using TMPro;
    using UnityEngine;
    using Zenject;

    
    public class DebugCurrencyIndicator : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;
        [Inject] private IWallet wallet;
        
        [SerializeField] private TMP_Text counter;
        
        private ICurrencyType currency;

        
        private void Start()
        {
            signalBus.Subscribe<TransactionCompleted>(OnAccountChanged);
            OnAccountChanged(new TransactionCompleted
            {
                Type = currency,
                Value = wallet.GetAmountOf(currency),
            });
        }

        public void Init(ICurrencyType currencyType)
        {
            currency = currencyType;
        }

        private void OnAccountChanged(TransactionCompleted args)
        {
            var currencyType = args.Type;
            var newValue = args.Value;
            
            if (currencyType != currency) return;

            counter.SetText($"{currencyType.Name} : {newValue}");
        }
        
        private void OnDestroy()
        {
            signalBus.TryUnsubscribe<TransactionCompleted>(OnAccountChanged);
        }
    }
}