namespace Ekstazz.Wallet.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ekstazz.Currencies;
    using UnityEngine;

    
    [CreateAssetMenu(menuName = "Ekstazz/Icons/Wallet", fileName = "WalletIcons")]
    public class WalletIcons : ScriptableObject
    {
        public List<TypedIcon> icons;

        public Sprite IconByType(ICurrencyType type)
        {
            return icons.First(icon => icon.type == type.Name).icon;
        }
        
        [Serializable]
        public class TypedIcon
        {
            public string type;
            
            public Sprite icon;
        }
    }
}