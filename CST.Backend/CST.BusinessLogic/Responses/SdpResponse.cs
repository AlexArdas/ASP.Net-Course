using Newtonsoft.Json;

namespace CST.BusinessLogic.Responses
{
    public class SdpResponse
    {
        [JsonProperty("access_token")] 
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
