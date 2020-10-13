// Instagram Private API
// Based on InstaSharpApi, by @ramtinak

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Madamin.Unfollow.Instagram.API.Responses;

namespace Madamin.Unfollow.Instagram.API
{
    partial class InstaApi
    {
        public async Task LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
                throw new ArgumentException();

            // SetUser
            _http_processor.RequestMessage.Username = username;
            _http_processor.RequestMessage.Password = password;

            // Prelogin
            await GetToken();
            await GetContactPointPrefill();
            await LauncherSyncPrivate();
            await QeSync();
            await GetPrefillCandidates();
            await Task.Delay(500);
            await QeSync();
            await Task.Delay(2500);

            await Task.Delay(5000);

            // Login
            await LoginAsync();

            // PostLogin
            await QeSync();
            //await FeedProcessor.GetUserTimelineFeedAsync(PaginationParameters.MaxPagesToLoad(1));
            //await StoryProcessor.GetStoryFeedAsync();
        }

        private async Task LoginAsync()
        {
            var cookies = _state.CookieContainer.GetCookies(
                _http_processor.Client.BaseAddress);
            var csrftoken = cookies[Constants.CSRFTOKEN]?.Value ?? string.Empty;
            _state.Session.CsrfToken = csrftoken;
            string devid;
            var signature = _http_processor.RequestMessage.GenerateSignature(
                _state.ApiVersion,
                _state.ApiVersion.SignatureKey,
                out devid);
            signature += $".{_http_processor.RequestMessage.GetMessageString()}";
            _state.Device.DeviceId = devid;
            var fields = new Dictionary<string, string>
            {
                {Constants.HEADER_IG_SIGNATURE, signature},
                {Constants.HEADER_IG_SIGNATURE_KEY_VERSION, Constants.IG_SIGNATURE_KEY_VERSION}
            };
            var request = _get_default_request(HttpMethod.Post, _login_uri, fields);
            var response = await _http_processor.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var loginFailReason = JsonConvert.DeserializeObject<InstaLoginBaseResponse>(json);

                if (loginFailReason.InvalidCredentials)
                {
                    if (loginFailReason.ErrorType == "bad_password")
                        throw new BadPasswordException();
                    throw new InvalidUserException();
                }

                if (loginFailReason.TwoFactorRequired)
                {
                    throw new TwoFactorAuthException();
                }
                if (loginFailReason.ErrorType == "checkpoint_challenge_required")
                {
                    throw new ChallengeException();
                }
                if (loginFailReason.ErrorType == "rate_limit_error")
                {
                    // msg: "Please wait a few minutes before you try again."
                    throw new LimitErrorException();
                }
                if (loginFailReason.ErrorType == "inactive user" || loginFailReason.ErrorType == "inactive_user")
                {
                    // $"{loginFailReason.Message}\r\nHelp url: {loginFailReason.HelpUrl}"
                    throw new InactiveUserException();
                }
                if (loginFailReason.ErrorType == "checkpoint_logged_out")
                {
                    throw new InstagramException($"{loginFailReason.ErrorType} {loginFailReason.CheckpointUrl}");
                }
                throw new InstagramException("<Unexpected>");
            }

            var loginInfo = JsonConvert.DeserializeObject<InstaLoginResponse>(json);
            _state.Session.Username = loginInfo.User?.UserName;
            _state.IsUserAuthenticated = loginInfo.User != null;
            if (loginInfo.User != null)
                _http_processor.RequestMessage.Username = loginInfo.User.UserName;
            _state.Session.User = new User(
                loginInfo.User.Pk,
                loginInfo.User.UserName,
                loginInfo.User.FullName);
            _state.Session.RankToken =
                $"{_state.Session.User.Id}_{_http_processor.RequestMessage.PhoneId}";
            if (string.IsNullOrEmpty(_state.Session.CsrfToken))
            {
                cookies = _http_processor.HttpHandler.CookieContainer.GetCookies(
                    _http_processor.Client.BaseAddress);
                _state.Session.CsrfToken = cookies[Constants.CSRFTOKEN]?.Value ?? string.Empty;
            }
        }

        private async Task GetToken(bool fromBaseUri = true)
        {
            try
            {
                var uri = !fromBaseUri ? 
                    _login_uri :
                    new Uri("https://www.instagram.com/.well-known/assetlinks.json");

                await _http_processor.SendAsync(
                    _get_default_request(HttpMethod.Get, uri));
            }
            catch
            {
                // ignore
            }
        }

        private async Task GetContactPointPrefill()
        {
            try
            {
                var uri = new Uri(Constants.BaseInstagramUri, Constants.ACCOUNTS_CONTACT_POINT_PREFILL);
                var cookies = _http_processor.HttpHandler.CookieContainer
                .GetCookies(_http_processor.Client.BaseAddress);
                var csrftoken = cookies[Constants.CSRFTOKEN]?.Value;
                //.{"phone_id":"----","usage":"prefill"}&
                var data = new Dictionary<string, string>
                {
                    {"phone_id", _state.Device.PhoneGuid.ToString()},
                };

                if (!string.IsNullOrEmpty(csrftoken))
                    data.Add("_csrftoken", csrftoken);
                data.Add("usage", "prefill");
                var request = _get_signed_request(HttpMethod.Post, uri, data);
                var response = await _http_processor.SendAsync(request);
            }
            catch (HttpRequestException ex)
            {
                // TODO: Log error
            }
            catch //(Exception ex)
            {
                // TODO: Log error
            }
        }

        private async Task LauncherSyncPrivate(bool second = false, bool third = false)
        {
            try
            {
                var data = new JObject();
                var cookies = _http_processor.HttpHandler.CookieContainer
                    .GetCookies(_http_processor.Client.BaseAddress);

                var csrftoken = cookies[Constants.CSRFTOKEN]?.Value;
                if (!string.IsNullOrEmpty(csrftoken))
                    data.Add("_csrftoken", csrftoken);
                if (IsUserAuthenticated && _state.Session.User != null)
                {
                    data.Add("id", _state.Device.DeviceGuid.ToString());
                    //data.Add("_uuid", _deviceInfo.DeviceGuid.ToString());
                }
                else
                    data.Add("id", _state.Device.DeviceGuid.ToString());
                data.Add("server_config_retrieval", "1");

                var uri = new Uri(Constants.BaseInstagramUri, Constants.LAUNCHER_SYNC);
                var request = _get_signed_request(HttpMethod.Post, uri, data);

                var response = await _http_processor.SendAsync(request);
                if (!third)
                {
                    _state.Session.PublicKey = 
                        string.Join("", response.Headers.GetValues("ig-set-password-encryption-pub-key"));
                    _state.Session.PublicKeyId = 
                        string.Join("", response.Headers.GetValues("ig-set-password-encryption-key-id"));
                }

            }
            catch (HttpRequestException ex)
            {
                // TODO: Log exception
            }
            catch //(Exception ex)
            {
                // TODO: Log exception
            }
        }
        private async Task QeSync()
        {
            try
            {
                var data = new JObject();
                var cookies = _http_processor.HttpHandler.CookieContainer
                .GetCookies(_http_processor.Client.BaseAddress);
                var csrftoken = cookies[Constants.CSRFTOKEN]?.Value;
                if (!string.IsNullOrEmpty(csrftoken))
                    data.Add("_csrftoken", csrftoken);
                else if (!string.IsNullOrEmpty(_state.Session.CsrfToken))
                    data.Add("_csrftoken", _state.Session.CsrfToken);
                if (IsUserAuthenticated && _state.Session.User != null)
                {
                    data.Add("id", _state.Device.DeviceGuid.ToString());
                    data.Add("_uid", _state.Device.DeviceGuid.ToString());
                    data.Add("server_config_retrieval", "1");
                    data.Add("experiments", Constants./*AFTER_*/LOGIN_EXPERIMENTS_CONFIGS);
                }
                else
                {
                    data.Add("id", _state.Device.DeviceGuid.ToString());
                    data.Add("server_config_retrieval", "1");
                    data.Add("experiments", Constants.LOGIN_EXPERIMENTS_CONFIGS);
                }


                var uri = new Uri(Constants.BaseInstagramUri, Constants.QE_SYNC);
                var request = _get_signed_request(HttpMethod.Post, uri, data);
                request.Headers.Add("X-DEVICE-ID", _state.Device.DeviceGuid.ToString());
                var response = await _http_processor.SendAsync(request);
            }
            catch (HttpRequestException ex)
            {
                // TODO: Log exception
            }
            catch //(Exception ex)
            {
                // TODO: Log exception
            }
        }

        private async Task GetPrefillCandidates()
        {
            try
            {
                var clientContactPoints = new JArray(new JObject
                {
                    {"type", "omnistring"},
                    {"value", _state.Session?.Username?.ToLower()},
                    {"source", "last_login_attempt"},
                });
                var data = new Dictionary<string, string>
                {
                    {"android_device_id", _state.Device.DeviceId},
                    {"client_contact_points", clientContactPoints.ToString(Formatting.None)},
                    {"phone_id", _state.Device.PhoneGuid.ToString()},
                    {"usages", "[\"account_recovery_omnibox\"]"},
                    {"logged_in_user_ids", "[]"},
                    {"device_id", _state.Device.DeviceGuid.ToString()},
                };

                var cookies = _http_processor.HttpHandler.CookieContainer
                    .GetCookies(_http_processor.Client.BaseAddress);

                var csrftoken = cookies[Constants.CSRFTOKEN]?.Value;
                if (!string.IsNullOrEmpty(csrftoken))
                    data.Add("_csrftoken", csrftoken);
                var instaUri = new Uri(Constants.BaseInstagramUri, Constants.ACCOUNTS_GET_PREFILL_CANDIDATES);
                var request = _get_signed_request(HttpMethod.Post, instaUri, data);
                var response = await _http_processor.SendAsync(request);
            }
            catch (HttpRequestException ex)
            {
                // TODO: Log exception
            }
            catch //(Exception ex)
            {
                // TODO: Log exception
            }
        }

        private readonly Random Rnd = new Random();

        private readonly Uri _login_uri =
            new Uri(Constants.BaseInstagramUri, Constants.ACCOUNTS_LOGIN);

    }
}
