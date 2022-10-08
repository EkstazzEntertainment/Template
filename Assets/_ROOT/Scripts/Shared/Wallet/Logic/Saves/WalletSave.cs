namespace Ekstazz.Wallet
{
    using System.Collections.Generic;
    using Ekstazz.Saves;
    using Newtonsoft.Json;

    [SaveComponent("Wallet")]
    public class WalletSave : ISaveComponent
    {
        [JsonProperty("accounts")]
        public List<AccountSave> Accounts { get; set; }
    }
}