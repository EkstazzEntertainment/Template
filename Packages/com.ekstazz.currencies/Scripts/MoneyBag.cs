namespace Ekstazz.Currencies
{
    using System.Collections.Generic;
    using System.Linq;

    
    public class MoneyBag
    {
        private readonly List<Money> moneyList;

        
        public MoneyBag(params Money[] money)
        {
            moneyList = new List<Money>(money);
        }

        public bool Has<T>() where T : ICurrencyType
        {
            return moneyList.Any(m => m.Type is T);
        }

        public bool Has<T>(Amount amount) where T : ICurrencyType
        {
            var neededCurrency = moneyList.FirstOrDefault(m => m.Type is T);
            if (neededCurrency.Type == null)
            {
                return false;
            }

            return neededCurrency.Amount >= amount;
        }

        public void Add(Money m)
        {
            for (var i = 0; i < moneyList.Count; i++)
            {
                if (moneyList[i].Type == m.Type)
                {
                    moneyList[i] = moneyList[i].AddSame(m);
                    return;
                }
            }

            moneyList.Add(m);
        }
    }
}