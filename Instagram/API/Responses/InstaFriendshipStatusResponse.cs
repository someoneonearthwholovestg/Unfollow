// Instagram Private API
// Based on InstaSharpApi, by @ramtinak

using System.Collections.Generic;

using Newtonsoft.Json;

namespace Madamin.Unfollow.Instagram.API.Responses
{
#if Compile_InstaFriendshipFullStatusResponse

    public class InstaFriendshipFullStatusContainerResponse
    {
        [JsonProperty("friendship_status")]
        public InstaFriendshipFullStatusResponse FriendshipStatus { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class InstaFriendshipFullStatusResponse
    {
        [JsonProperty("following")]
        public bool? Following { get; set; }
        [JsonProperty("followed_by")]
        public bool? FollowedBy { get; set; }
        [JsonProperty("blocking")]
        public bool? Blocking { get; set; }
        [JsonProperty("muting")]
        public bool? Muting { get; set; }
        [JsonProperty("is_private")]
        public bool? IsPrivate { get; set; }
        [JsonProperty("incoming_request")]
        public bool? IncomingRequest { get; set; }
        [JsonProperty("outgoing_request")]
        public bool? OutgoingRequest { get; set; }
        [JsonProperty("is_bestie")]
        public bool? IsBestie { get; set; }
    }
#endif

}