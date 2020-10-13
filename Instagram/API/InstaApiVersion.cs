// Instagram Private API
// Based on InstaSharpApi, by @ramtinak

using System;

namespace Madamin.Unfollow.Instagram.API
{
    [Serializable]
    public class InstaApiVersion
    {
        public string SignatureKey { get; set; }
        public string AppVersion { get; set; }
        public string AppApiVersionCode { get; set; }
        public string Capabilities { get; set; }
        public string BloksVersionId { get; set; }

        public static readonly InstaApiVersion Version126 = new InstaApiVersion
        {
            AppApiVersionCode = "195435560",
            AppVersion = "126.0.0.25.121",
            Capabilities = "3brTvwM=",
            SignatureKey = "8e496c87a09d5e922f6e33df3f399ce298ddbd6f7d6d038417047cc6474a3542",
            BloksVersionId = "e538d4591f238824118bfcb9528c8d005f2ea3becd947a3973c030ac971bb88e"
        };
    }
}