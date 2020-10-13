// Instagram Private API
// Based on InstaSharpApi, by @ramtinak

using Newtonsoft.Json;

namespace Madamin.Unfollow.Instagram.API.Responses
{
    public class BaseStatusResponse
    {
        [JsonProperty("status")] public string Status { get; set; }

        public bool IsOk()
        {
            return !string.IsNullOrEmpty(Status) && Status.ToLower() == "ok";
        }

        public bool IsFail()
        {
            return !string.IsNullOrEmpty(Status) && Status.ToLower() == "fail";
        }
    }
}