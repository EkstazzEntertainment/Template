namespace Ekstazz.Wallet
{
    using Newtonsoft.Json;

    public class AccountSave
    {
        [JsonProperty("currency")]
        public string Type { get; set; }
        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("spent")]
        public double Spent { get; set; }
    }
}