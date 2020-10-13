// Instagram Private API
// Based on InstaSharpApi, by @ramtinak

using System;

using Newtonsoft.Json;

namespace Madamin.Unfollow.Instagram.API
{
    public class ApiRequestMessage
    {
        [JsonProperty("jazoest")]
        public string Jazoest { get; set; }
        
        [JsonProperty("country_codes")]
        public string CountryCodes { get; set; } = "[{\"country_code\":\"1\",\"source\":[\"default\"]},{\"country_code\":\"1\",\"source\":[\"uig_via_phone_id\"]}]";
        
        [JsonProperty("phone_id")]
        public string PhoneId { get { return _phoneId; } set { _phoneId = value; Jazoest = _generate_jazoest(value); } }
        
        [JsonProperty("username")]
        public string Username { get; set; }
        
        [JsonProperty("adid")]
        public string AdId { get; set; }
        
        [JsonProperty("guid")]
        public Guid Guid { get; set; }
        
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }
        
        [JsonProperty("_uuid")]
        public string Uuid => Guid.ToString();
        
        [JsonProperty("google_tokens")]
        public string GoogleTokens { get; set; } = "[]";
        
        [JsonProperty("password")]
        public string Password { get; set; }
        
        [JsonProperty("login_attempt_count")]
        public string LoginAttemptCount { get; set; } = "1";
        
        public static ApiRequestMessage CurrentDevice { get; private set; }
        
        internal string GetMessageString()
        {
            var json = JsonConvert.SerializeObject(this);
            return json;
        }
        
        internal string GetMessageStringForChallengeVerificationCodeSend(int Choice = 1)
        {
            return JsonConvert.SerializeObject(new { choice = Choice.ToString(), _csrftoken = "ReplaceCSRF", Guid, DeviceId });
        }
        
        internal string GetChallengeVerificationCodeSend(string verify)
        {
            return JsonConvert.SerializeObject(new { security_code = verify, _csrftoken = "ReplaceCSRF", Guid, DeviceId });
        }
        
        internal string GenerateSignature(InstaApiVersion apiVersion, string signatureKey, out string deviceid)
        {
            if (string.IsNullOrEmpty(signatureKey))
                signatureKey = apiVersion.SignatureKey;
            var res = Helper.CalculateHash(
                signatureKey,
                JsonConvert.SerializeObject(this));
            deviceid = DeviceId;
            return res;
        }
        
        
        internal bool IsEmpty()
        {
            if (string.IsNullOrEmpty(PhoneId)) return true;
            if (string.IsNullOrEmpty(DeviceId)) return true;
            if (Guid.Empty == Guid) return true;
            return false;
        }

        internal static string GenerateDeviceId()
        {
            return GenerateDeviceIdFromGuid(Guid.NewGuid());
        }

        internal static string GenerateUploadId()
        {
            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            var uploadId = (long)timeSpan.TotalSeconds;
            return uploadId.ToString();
        }
        
        public static ApiRequestMessage FromDevice(AndroidDevice device)
        {
            var requestMessage = new ApiRequestMessage
            {
                PhoneId = device.PhoneGuid.ToString(),
                Guid = device.DeviceGuid,
                DeviceId = device.DeviceId
            };
            return requestMessage;
        }

        public static string GenerateDeviceIdFromGuid(Guid guid)
        {
            var hashedGuid = Helper.CalculateMd5(guid.ToString());
            return $"android-{hashedGuid.Substring(0, 16)}";
        }

        private static string _generate_jazoest(string guid)
        {
            int ix = 0;
            var chars = guid.ToCharArray();
            foreach (var ch in chars)
                ix += (int)ch;
            return "2" + ix;
        }

        private string _phoneId;
    }
}