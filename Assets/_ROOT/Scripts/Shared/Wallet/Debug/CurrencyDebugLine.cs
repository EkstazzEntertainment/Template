namespace Ekstazz.Wallet.Debug
{
    using Ekstazz.Wallet;
    using Ekstazz.Currencies;
    using Ekstazz.Saves;
    using TMPro;
    using UnityEngine;
    using Zenject;

    
    public class CurrencyDebugLine : MonoBehaviour
    {
        [Inject] private IWallet wallet;
        [Inject] private ISaveScheduler saveScheduler;

        [SerializeField] private DebugCurrencyIndicator currencyIndicator;
        [SerializeField] private TMP_InputField field;

        private string Key => $"DebugCurrency{currencyName}";

        private int currentInputAmount;
        private const int StartDefaultValue = 5000;
        private ICurrencyType currencyType;
        private string currencyName;
        
        private int CurrentInputAmount
        {
            get => currentInputAmount;
            set
            {
                currentInputAmount = value;
                PlayerPrefs.SetInt(Key, currentInputAmount);
            }
        }


        private void Start()
        {
            CurrentInputAmount = PlayerPrefs.GetInt(Key, StartDefaultValue);
            field.text = $"{CurrentInputAmount}";
            field.onEndEdit.AddListener(OnEndEdit);
        }

        public void Init(ICurrencyType type)
        {
            currencyType = type;
            currencyName = currencyType.Name;
            currencyIndicator.Init(type);
        }

        public void Add()
        {
            var amount = (Amount) CurrentInputAmount;
            wallet.Add(new Money(currencyType, amount), $"{Key}");
            saveScheduler.ScheduleSave();
        }

        public void Retrieve()
        {
            var amount = (Amount) CurrentInputAmount;
            wallet.TrySpend(new Money(currencyType, amount), $"{Key}");
            saveScheduler.ScheduleSave();
        }

        private void OnEndEdit(string value)
        {
            var amount = int.Parse(value);
            CurrentInputAmount = Mathf.Max(amount, 0);
            field.text = $"{CurrentInputAmount}";
        }
    }
}