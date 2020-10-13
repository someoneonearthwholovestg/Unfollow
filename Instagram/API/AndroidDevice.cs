// Instagram Private API
// Based on InstaSharpApi, by @ramtinak

using System;

using Android.OS;
using Android.Util;

namespace Madamin.Unfollow.Instagram.API
{
    [Serializable]
    public class AndroidDevice
    {
        public Guid PhoneGuid { get; set; } = Guid.NewGuid();
        public Guid DeviceGuid { get; set; } = Guid.NewGuid();
        public Guid GoogleAdId { get; set; } = Guid.NewGuid();
        public Guid RankToken { get; set; } = Guid.NewGuid();
        public Guid AdId { get; set; } = Guid.NewGuid();
        public Guid PigeonSessionId { get; set; } = Guid.NewGuid();
        public Guid PushDeviceGuid { get; set; } = Guid.NewGuid();
        public Guid FamilyDeviceGuid { get; set; } = Guid.NewGuid();
        public AndroidVersion AndroidVer { get; set; }

        public string AndroidBoardName { get; set; }
        public string AndroidBootloader { get; set; }
        public string DeviceBrand { get; set; }
        public string DeviceId { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceModelBoot { get; set; }
        public string DeviceModelIdentifier { get; set; }
        public string FirmwareBrand { get; set; }
        public string FirmwareFingerprint { get; set; }
        public string FirmwareTags { get; set; }
        public string FirmwareType { get; set; }
        public string HardwareManufacturer { get; set; }
        public string HardwareModel { get; set; }
        public string Resolution { get; set; } = "1080x1812";
        public string Dpi { get; set; } = "480dpi";
        public string IGBandwidthSpeedKbps { get; set; } = "-1000";
        public string IGBandwidthTotalBytesB { get; set; } = "0";
        public string IGBandwidthTotalTimeMS { get; set; } = "0";

        public string GenerateUserAgent(InstaApiVersion apiVersion)
        {
            return string.Format(
                Constants.USER_AGENT, 
                Dpi,
                Resolution,
                HardwareManufacturer,
                DeviceModelIdentifier,
                FirmwareBrand, 
                HardwareModel,
                apiVersion.AppVersion,
                AndroidVer.APILevel,
                AndroidVer.VersionNumber, 
                apiVersion.AppApiVersionCode);
        }

        public static AndroidDevice GetCurrentDevice()
        {
            var display = new DisplayMetrics();
            return new AndroidDevice
            {
                AndroidVer = new AndroidVersion
                {
                    Codename = Build.VERSION.Codename,
                    VersionNumber = Build.VERSION.Release,
                    APILevel = ((int)Build.VERSION.SdkInt).ToString(),
                },
                AndroidBoardName = Build.Board,
                AndroidBootloader = Build.Bootloader,
                DeviceBrand = Build.Brand,
                DeviceModel = Build.Model,
                DeviceModelBoot = Build.Hardware,
                DeviceModelIdentifier = Build.Device,
                Resolution = $"{display.WidthPixels}x{display.HeightPixels}",
                Dpi = ((int)(display.Density * 160f)) + "dpi",
                FirmwareBrand = Build.Brand,
                FirmwareFingerprint = Build.Fingerprint,
                FirmwareTags = Build.Tags,
                FirmwareType = Build.Type,
                HardwareManufacturer = Build.Manufacturer,
                HardwareModel = Build.Model,
            };
            /*return new AndroidDevice
            {
                AndroidVer = new AndroidVersion
                {
                    Codename = Build.VERSION.Codename,
                    VersionNumber = Build.VERSION.Release,
                    APILevel = ((int)Build.VERSION.SdkInt).ToString(),
                },
                AndroidBoardName = "universal7420",
                AndroidBootloader = "G920FXXU3DPEK",
                DeviceBrand = "samsung",
                DeviceModel = "zeroflte",
                DeviceModelBoot = "qcom",
                DeviceModelIdentifier = "SM-G920F",
                FirmwareBrand = "zerofltexx",
                FirmwareFingerprint = "samsung/zerofltexx/zeroflte:6.0.1/MMB29K/G920FXXU3DPEK:user/release-keys",
                FirmwareTags = "dev-keys",
                FirmwareType = "user",
                HardwareManufacturer = "samsung",
                HardwareModel = "samsungexynos7420",
                DeviceGuid = Guid.NewGuid(),
                PhoneGuid = Guid.NewGuid(),
                Resolution = "1440x2560",
                Dpi = "640dpi"
            };*/
        }
    }

    [Serializable]
    public class AndroidVersion
    {
        internal AndroidVersion()
        {
        }

        public string Codename { get; set; }
        public string VersionNumber { get; set; }
        public string APILevel { get; set; }
    }
}