namespace Ekstazz.Wallet.Debug
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Ekstazz.Currencies;
    using UnityEngine;
    using Zenject;

    
    public class WalletDebug : MonoBehaviour
    {
        [Inject] private MoneyFactory moneyFactory;
        [Inject] private DiContainer diContainer;

        [SerializeField] private RectTransform panelRect;
        [SerializeField] private CurrencyDebugLine linePrefab;

        private List<CurrencyDebugLine> walletDebugLines = new List<CurrencyDebugLine>();
        private List<ICurrencyType> currencyTypes;

        private bool IsAnyCurrencies => currencyTypes.Count != 0;

        
        private void Start()
        {
            LoadConfigAndFields();

            if (!IsAnyCurrencies)
            {
                DestroyDebugComponent();
            }
            else
            {
                CreateCurrencyLines();
                ResizeView();
            }
        }

        private void LoadConfigAndFields()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var field = moneyFactory.GetType().GetField("currenciesByName", flags);
            var currenciesDictionary = (Dictionary<string, ICurrencyType>) field?.GetValue(moneyFactory);
            currencyTypes = currenciesDictionary?.Values.ToList();
        }

        private void DestroyDebugComponent()
        {
            var index = transform.GetSiblingIndex();
            
            var header = transform.parent.GetChild(index + 1);
            var separator = transform.parent.GetChild(index - 1);
            
            Destroy(header.gameObject);
            Destroy(separator.gameObject);
            Destroy(gameObject);
        }

        private void CreateCurrencyLines()
        {
            foreach (var currencyType in currencyTypes)
            {
                var walletLine = diContainer.InstantiatePrefabForComponent<CurrencyDebugLine>(linePrefab, transform);
                walletLine.Init(currencyType);
                walletDebugLines.Add(walletLine);
            }
        }

        private void ResizeView()
        {
            var optionHeight = linePrefab.GetComponent<RectTransform>().rect.height;

            var size = panelRect.sizeDelta;
            size.y += optionHeight * walletDebugLines.Count;
            panelRect.sizeDelta = size;
        }
    }
}