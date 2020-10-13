// Instagram Private API
// Based on InstaSharpApi, by @ramtinak

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Madamin.Unfollow.Instagram.API.Responses;

namespace Madamin.Unfollow.Instagram.API
{
    partial class InstaApi
    {
        private async Task<InstaUserListShortResponse> GetUserListByUriAsync(Uri uri)
        {
            var request = _get_default_request(HttpMethod.Get, uri);
            var response = await _http_processor.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new InstagramException("<Unexpected>");

            var instaUserListResponse = JsonConvert.DeserializeObject<InstaUserListShortResponse>(json);
            if (instaUserListResponse.IsOk())
                return instaUserListResponse;

            throw new InstagramException("<Unexpected>");
        }
    }
}