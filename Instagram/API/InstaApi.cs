// Instagram Private API
// Based on InstaSharpApi, by @ramtinak

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Madamin.Unfollow.Instagram.API.Responses;

namespace Madamin.Unfollow.Instagram.API
{
    internal partial class InstaApi
    {
        public InstaApi()
        {
            _http_processor = new HttpRequestProcessor(
                    new HttpClient() { BaseAddress = new Uri(Constants.INSTAGRAM_URL) },
                    new HttpClientHandler(),
                    new ApiRequestMessage() 
                    {
                        PhoneId = _state.Device.PhoneGuid.ToString(),
                        Guid = _state.Device.DeviceGuid,
                        AdId = _state.Device.AdId.ToString(),
                        DeviceId = ApiRequestMessage.GenerateDeviceId()
                    }
                );
        }

        public async Task LogoutAsync()
        {
            if (!IsUserAuthenticated)
                throw new UserNotAuthenticatedException();

            var data = new Dictionary<string, string>
                {
                    {"phone_id", _state.Device.PhoneGuid.ToString()},
                    {"_csrftoken", _state.Session.CsrfToken},
                    {"guid", _state.Device.DeviceGuid.ToString()},
                    {"device_id", _state.Device.DeviceId},
                    {"_uuid", _state.Device.DeviceGuid.ToString()}
                };
            var instaUri = new Uri(Constants.BaseInstagramUri, Constants.ACCOUNTS_LOGOUT);
            var request = _get_default_request(HttpMethod.Post, instaUri, data);
            var response = await _http_processor.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new InstagramException("<Unexpected>");

            var logoutInfo = JsonConvert.DeserializeObject<BaseStatusResponse>(json);

            if (logoutInfo.Status == "ok")
                _state.IsUserAuthenticated = false;
        }

        public async Task UnfollowAsync(long userid)
        {
            if (!IsUserAuthenticated)
                throw new UserNotAuthenticatedException();
            var uri = new Uri(
                Constants.BaseInstagramUri,
                string.Format(Constants.FRIENDSHIPS_UNFOLLOW_USER, userid));
            var fields = new Dictionary<string, string>
                {
                    {"_uuid", _state.Device.DeviceGuid.ToString()},
                    {"_uid", _state.Session.User.Id.ToString()},
                    {"_csrftoken", _state.Session.CsrfToken},
                    {"user_id", userid.ToString()},
                    {"radio_type", "wifi-none"}
                };
            var request = _get_signed_request(HttpMethod.Post, uri, fields);
            var response = await _http_processor.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK ||
                string.IsNullOrEmpty(json))
                throw new InstagramException("<Unexpected>");

#if Compile_InstaFriendshipFullStatusResponse
            var friendshipStatus = JsonConvert.DeserializeObject<InstaFriendshipFullStatusContainerResponse>(json);
#endif
        }

        /*public async Task<User> GetCurrentUserDataAsync()
        {
            throw new NotImplementedException();
        }*/

        public async Task<User[]> GetFollowingsAsync(long userid)
        {
            var following = new List<User>();
            var next_max_id = "";

            do
            {
                var uri = _create_user_following_uri(
                    userid, _state.Session.RankToken, "", next_max_id);
                var userListResponse = await GetUserListByUriAsync(uri);

                following.AddRange(from user_response in userListResponse.Items
                                   select new User(
                                       user_response.Pk,
                                       user_response.UserName,
                                       user_response.FullName
                                       )
                                   );

                next_max_id = userListResponse.NextMaxId;
            }
            while (!string.IsNullOrEmpty(next_max_id));

            return following.ToArray();
        }

        public async Task<User[]> GetFollowersAsync(long userid)
        {
            var followers = new List<User>();
            var next_max_id = "";

            do
            {
                var uri = _create_user_follower_uri(
                    userid, _state.Session.RankToken, "", next_max_id);
                var userListResponse = await GetUserListByUriAsync(uri);

                followers.AddRange(from user_response in userListResponse.Items
                                   select new User(
                                       user_response.Pk,
                                       user_response.UserName,
                                       user_response.FullName
                                       )
                                   );

                next_max_id = userListResponse.NextMaxId;
            }
            while (!string.IsNullOrEmpty(next_max_id));

            return followers.ToArray();
        }

        public object State
        {
            get
            {
                return _state;
            }
            set
            {
                if (value is StateData state)
                {
                    _state = state;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        public bool IsUserAuthenticated => _state.IsUserAuthenticated;

        public User User => _state.Session.User;

        private static Uri _create_user_following_uri(
            long userid, string rank_token, string query, string maxid)
        {
            return new Uri(
                    Constants.BaseInstagramUri,
                    string.Format(
                        Constants.FRIENDSHIPS_USER_FOLLOWING,
                        userid,
                        rank_token))
                .AddQueryParameterIfNotEmpty("max_id", maxid)
                .AddQueryParameterIfNotEmpty("query", query);
        }

        private static Uri _create_user_follower_uri(
            long userid, string rank_token, string query, string maxid)
        {
            return new Uri(
                    Constants.BaseInstagramUri,
                    string.Format(
                        Constants.FRIENDSHIPS_USER_FOLLOWERS,
                        userid,
                        rank_token))
                .AddQueryParameterIfNotEmpty("max_id", maxid)
                .AddQueryParameterIfNotEmpty("query", query);
        }

        private StateData _state = new StateData();
        private HttpRequestProcessor _http_processor;

        [Serializable]
        private class StateData
        {
            public bool IsUserAuthenticated { get; set; }
            public InstaApiVersion ApiVersion { get; set; } = InstaApiVersion.Version126;
            public AndroidDevice Device { get; set; } = AndroidDevice.GetCurrentDevice();
            public CookieContainer CookieContainer { get; set; } = new CookieContainer();
            public SessionData Session { get; set; } = new SessionData();
        }
    }

    [Serializable]
    class SessionData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PublicKey { get; set; }
        public string PublicKeyId { get; set; }
        public string WwwClaim { get; set; }
        public string FbTripId { get; set; }
        public string Authorization { get; set; }

        public string RankToken { get; set; }
        public string CsrfToken { get; set; }

        public User User { get; set; }
    }
}
