// Instagram Private API
// Based on InstaSharpApi, by @ramtinak

using System.Collections.Generic;

using Newtonsoft.Json;

namespace Madamin.Unfollow.Instagram.API.Responses
{
    public class InstaUserShortResponse : BaseStatusResponse
    {
        [JsonProperty("username")] public string UserName { get; set; }

        [JsonProperty("profile_pic_url")] public string ProfilePicture { get; set; }

        [JsonProperty("profile_pic_id")] public string ProfilePictureId { get; set; } = "unknown";

        [JsonProperty("full_name")] public string FullName { get; set; }

        [JsonProperty("is_verified")] public bool IsVerified { get; set; }

        [JsonProperty("is_private")] public bool IsPrivate { get; set; }

        [JsonProperty("pk")] public long Pk { get; set; }
    }

    public class InstaUserListShortResponse : BaseStatusResponse
    {
        [JsonProperty("users")] public List<InstaUserShortResponse> Items { get; set; }

        [JsonProperty("big_list")] public bool IsBigList { get; set; }

        [JsonProperty("page_size")] public int PageSize { get; set; }

        [JsonProperty("next_max_id")] public string NextMaxId { get; set; }
    }
}