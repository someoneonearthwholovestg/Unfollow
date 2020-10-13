// Instagram Private API
// Based on InstaSharpApi, by @ramtinak

using Newtonsoft.Json;

namespace Madamin.Unfollow.Instagram.API.Responses
{
    internal class InstaLoginBaseResponse
    {
        #region InvalidCredentials

        [JsonProperty("invalid_credentials")] public bool InvalidCredentials { get; set; }

        [JsonProperty("error_type")] public string ErrorType { get; set; }

        [JsonProperty("message")] public string Message { get; set; }

        [JsonProperty("help_url")] public string HelpUrl { get; set; }
        #endregion

        #region 2 Factor Authentication

        [JsonProperty("two_factor_required")] public bool TwoFactorRequired { get; set; }

        [JsonProperty("two_factor_info")] public InstaTwoFactorLoginInfo TwoFactorLoginInfo { get; set; }

        #endregion

        #region Challenge

        [JsonProperty("challenge")] public InstaChallengeLoginInfo Challenge { get; set; }

        #endregion

        [JsonProperty("lock")] public bool? Lock { get; set; }

        [JsonProperty("checkpoint_url")] public string CheckpointUrl { get; set; }
    }

    public class InstaTwoFactorLoginInfo
    {
        [JsonProperty("obfuscated_phone_number")]
        public string ObfuscatedPhoneNumber { get; set; }

        [JsonProperty("show_messenger_code_option")]
        public bool? ShowMessengerCodeOption { get; set; }

        [JsonProperty("two_factor_identifier")]
        public string TwoFactorIdentifier { get; set; }

        [JsonProperty("username")] public string Username { get; set; }

        [JsonProperty("phone_verification_settings")]
        public InstaPhoneVerificationSettings PhoneVerificationSettings { get; set; }

        public static InstaTwoFactorLoginInfo Empty => new InstaTwoFactorLoginInfo();
    }

    public class InstaChallengeLoginInfo
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("api_path")]
        public string ApiPath { get; set; }
        [JsonProperty("hide_webview_header")]
        public bool HideWebviewHeader { get; set; }
        [JsonProperty("lock")]
        public bool Lock { get; set; }
        [JsonProperty("logout")]
        public bool Logout { get; set; }
        [JsonProperty("native_flow")]
        public bool NativeFlow { get; set; }

    }

    public class InstaPhoneVerificationSettings
    {
        [JsonProperty("max_sms_count")] public string MaxSmsCount { get; set; }

        [JsonProperty("resend_sms_delay_sec")] public int? ResendSmsDelaySeconds { get; set; }

        [JsonProperty("robocall_after_max_sms")]
        public bool? RobocallAfterMaxSms { get; set; }

        [JsonProperty("robocall_count_down_time")]
        public int? RobocallCountDownTime { get; set; }
    }

    public class InstaLoginResponse
    {
        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("logged_in_user")] public InstaUserShortResponse User { get; set; }
    }
}
