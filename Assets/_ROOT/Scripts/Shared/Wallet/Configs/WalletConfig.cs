namespace Ekstazz.Wallet.Configs
{
    using System.Collections.Generic;
    using Ekstazz.Configs;
    using Newtonsoft.Json;

    
    [AutoConfigurable]
    public class WalletConfig
    {
        [ConfigJsonProperty(Key = "wallet_defaults")]
        public WalletDefaults Defaults { get; set; }
    }

    public class WalletDefaults
    {
        public WalletSaveBehaviour SaveBehaviour { get; private set; }

        [JsonProperty("currencies")]
        public IDictionary<string, int> Currencies { get; private set; } = new Dictionary<string, int>();
    }
}
