namespace Ekstazz.Wallet.Views
{
    using Ekstazz.Currencies;
    using TMPro;
    using UnityEngine;
    using Zenject;

    
    public class PriceIndicator : MonoBehaviour
    {
        [Inject] private IWallet wallet;

        [SerializeField] private TMP_Text text;

        private IHasPrice priceComponent;

        
        private void Start()
        {
            priceComponent = GetComponent<IHasPrice>();
        }

        private void Update()
        {
            var price = priceComponent.Price;
            var color = ColorByPrice(price);
            text.color = color;
            text.SetText(price.Amount + "$");
        }

        private Color ColorByPrice(in Money price)
        {
            return wallet.CanSpend(price) ? Color.green : Color.red;
        }
    }
}