// Instagram Private API
// Based on InstaSharpApi, by @ramtinak

using System;
using System.Collections.Generic;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Madamin.Unfollow.Instagram.API
{
    partial class InstaApi
    {
        private HttpRequestMessage _get_default_request(HttpMethod method, Uri uri, Dictionary<string, string> data)
        {
            //var request = _get_default_request(HttpMethod.Post, uri);
            var request = _get_default_request(method, uri);
            request.Content = new FormUrlEncodedContent(data);
            return request;
        }

        private HttpRequestMessage _get_default_request(HttpMethod method, Uri uri)
        {
            var userAgent = _state.Device.GenerateUserAgent(_state.ApiVersion);
            var request = new HttpRequestMessage(method, uri);
            var cookies = _http_processor.HttpHandler.CookieContainer.GetCookies(
                _http_processor.Client.BaseAddress);
            var mid = cookies[Constants.COOKIES_MID]?.Value ?? string.Empty;
            var dsUserId = cookies[Constants.COOKIES_DS_USER_ID]?.Value ?? string.Empty;
            var sessionId = cookies[Constants.COOKIES_SESSION_ID]?.Value ?? string.Empty;

            request.Headers.Add(Constants.HEADER_X_IG_APP_LOCALE, Constants.ACCEPT_LANGUAGE.Replace("-", "_"));
            request.Headers.Add(Constants.HEADER_X_IG_DEVICE_LOCALE, Constants.ACCEPT_LANGUAGE.Replace("-", "_"));
            request.Headers.Add(Constants.HEADER_PIGEON_SESSION_ID, _state.Device.PigeonSessionId.ToString());
            request.Headers.Add(Constants.HEADER_PIGEON_RAWCLINETTIME, $"{DateTime.UtcNow.ToUnixTime()}.0{Rnd.Next(10, 99)}");
            request.Headers.Add(Constants.HEADER_X_IG_CONNECTION_SPEED, "-1kbps");
            request.Headers.Add(Constants.HEADER_X_IG_BANDWIDTH_SPEED_KBPS, _state.Device.IGBandwidthSpeedKbps);
            request.Headers.Add(Constants.HEADER_X_IG_BANDWIDTH_TOTALBYTES_B, _state.Device.IGBandwidthTotalBytesB);
            request.Headers.Add(Constants.HEADER_X_IG_BANDWIDTH_TOTALTIME_MS, _state.Device.IGBandwidthTotalTimeMS);
            request.Headers.Add(Constants.HEADER_IG_APP_STARTUP_COUNTRY, Constants.HEADER_IG_APP_STARTUP_COUNTRY_VALUE);

            ////request.Headers.Add(InstaApiConstants.HEADER_X_IG_EXTENDED_CDN_THUMBNAIL_CACHE_BUSTING_VALUE, "1000");
            //if (!string.IsNullOrEmpty(_apiVersion.BloksVersionId))
            //    request.Headers.Add(InstaApiConstants.HEADER_X_IG_BLOKS_VERSION_ID, _apiVersion.BloksVersionId);
            //else
            //    request.Headers.Add(InstaApiConstants.HEADER_X_IG_BLOKS_VERSION_ID, InstaApiConstants.CURRENT_BLOKS_VERSION_ID);
            var wwwClaim = _state.Session.WwwClaim;

            if (string.IsNullOrEmpty(wwwClaim))
                request.Headers.Add(Constants.HEADER_X_WWW_CLAIM, Constants.HEADER_X_WWW_CLAIM_DEFAULT);
            else
                request.Headers.Add(Constants.HEADER_X_WWW_CLAIM, wwwClaim);

            var authorization = _state.Session.Authorization;
            if (!string.IsNullOrEmpty(dsUserId) && !string.IsNullOrEmpty(authorization))
                request.Headers.Add(Constants.HEADER_AUTHORIZATION, authorization);

            request.Headers.Add(Constants.HEADER_X_IG_BLOKS_IS_LAYOUT_RTL, "false");
            request.Headers.Add(Constants.HEADER_X_IG_BLOKS_ENABLE_RENDERCODE, "false");
            request.Headers.Add(Constants.HEADER_X_IG_DEVICE_ID, _state.Device.DeviceGuid.ToString());
            request.Headers.Add(Constants.HEADER_X_IG_ANDROID_ID, _state.Device.DeviceId);

            request.Headers.Add(Constants.HEADER_IG_CONNECTION_TYPE, Constants.IG_CONNECTION_TYPE);
            request.Headers.Add(Constants.HEADER_IG_CAPABILITIES, _state.ApiVersion.Capabilities);
            request.Headers.Add(Constants.HEADER_IG_APP_ID, Constants.IG_APP_ID);

            request.Headers.Add(Constants.HEADER_USER_AGENT, userAgent);
            request.Headers.Add(Constants.HEADER_ACCEPT_LANGUAGE, Constants.ACCEPT_LANGUAGE);

            if (!string.IsNullOrEmpty(mid))
                request.Headers.Add(Constants.HEADER_X_MID, mid);
            request.Headers.TryAddWithoutValidation(Constants.HEADER_ACCEPT_ENCODING, Constants.ACCEPT_ENCODING2);

            request.Headers.Add(Constants.HOST, Constants.HOST_URI);

            request.Headers.Add(Constants.HEADER_X_FB_HTTP_ENGINE, "Liger");
            return request;
        }

        private HttpRequestMessage _get_signed_request(
            HttpMethod method,
            Uri uri,
            Dictionary<string, string> data)
        {
            var hash = Helper.CalculateHash(
                _state.ApiVersion.SignatureKey,
                JsonConvert.SerializeObject(data));
            var payload = JsonConvert.SerializeObject(data);
            var signature = $"{hash}.{payload}";

            var fields = new Dictionary<string, string>
            {
                {Constants.HEADER_IG_SIGNATURE, signature},
                {Constants.HEADER_IG_SIGNATURE_KEY_VERSION, Constants.IG_SIGNATURE_KEY_VERSION}
            };
            // original code: (why?)
            //   var request = _get_default_request(HttpMethod.Post, uri);
            var request = _get_default_request(method, uri);
            request.Content = new FormUrlEncodedContent(fields);
            request.Properties.Add(Constants.HEADER_IG_SIGNATURE, signature);
            request.Properties.Add(
                Constants.HEADER_IG_SIGNATURE_KEY_VERSION,
                Constants.IG_SIGNATURE_KEY_VERSION);
            return request;
        }

        public HttpRequestMessage _get_signed_request(
            HttpMethod method,
            Uri uri,
            JObject data)
        {
            var hash = Helper.CalculateHash(
                _state.ApiVersion.SignatureKey,
                data.ToString(Formatting.None));
            var payload = data.ToString(Formatting.None);
            var signature = $"{hash}.{payload}";
            var fields = new Dictionary<string, string>
            {
                {Constants.HEADER_IG_SIGNATURE, signature},
                {Constants.HEADER_IG_SIGNATURE_KEY_VERSION, Constants.IG_SIGNATURE_KEY_VERSION}
            };
            //var request = _get_default_request(HttpMethod.Post, uri);
            var request = _get_default_request(method, uri);
            request.Content = new FormUrlEncodedContent(fields);
            request.Properties.Add(Constants.HEADER_IG_SIGNATURE, signature);
            request.Properties.Add(
                Constants.HEADER_IG_SIGNATURE_KEY_VERSION,
                Constants.IG_SIGNATURE_KEY_VERSION);
            return request;
        }
    }
}