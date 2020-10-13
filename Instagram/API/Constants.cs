// Instagram Private API
// Based on InstaSharpApi, by @ramtinak

using System;

using Newtonsoft.Json.Linq;

internal static class Constants
{
    #region Main
    public const string ACCEPT_ENCODING2 = "gzip, deflate";
    public const string HEADER_PIGEON_SESSION_ID = "X-Pigeon-Session-Id";
    public const string HEADER_PIGEON_RAWCLINETTIME = "X-Pigeon-Rawclienttime";
    public const string HEADER_X_IG_CONNECTION_SPEED = "X-IG-Connection-Speed";
    public const string HEADER_X_IG_BANDWIDTH_SPEED_KBPS = "X-IG-Bandwidth-Speed-KBPS";
    public const string HEADER_X_IG_BANDWIDTH_TOTALBYTES_B = "X-IG-Bandwidth-TotalBytes-B";
    public const string HEADER_X_IG_BANDWIDTH_TOTALTIME_MS = "X-IG-Bandwidth-TotalTime-MS";
    public const string HEADER_X_FB_HTTP_ENGINE = "X-FB-HTTP-Engine";
    public const string HEADER_X_IG_APP_LOCALE = "X-IG-App-Locale";
    public const string HEADER_X_IG_DEVICE_LOCALE = "X-IG-Device-Locale";
    public const string HEADER_X_MID = "X-MID";
    public const string HEADER_AUTHORIZATION = "Authorization";
    public const string HEADER_RESPONSE_AUTHORIZATION = "ig-set-authorization";
    public const string HEADER_X_WWW_CLAIM = "X-IG-WWW-Claim";
    public const string HEADER_X_WWW_CLAIM_DEFAULT = "0";
    public const string HEADER_RESPONSE_X_WWW_CLAIM = "x-ig-set-www-claim";
    public const string HEADER_X_FB_TRIP_ID = "X-FB-TRIP-ID";
    public const string COOKIES_MID = "mid";
    public const string COOKIES_DS_USER_ID = "ds_user_id";
    public const string COOKIES_SESSION_ID = "sessionid";
    public const string HEADER_IG_APP_STARTUP_COUNTRY = "X-IG-App-Startup-Country";
    public static string HEADER_IG_APP_STARTUP_COUNTRY_VALUE = "US"; // IR
    public const string HEADER_X_IG_EXTENDED_CDN_THUMBNAIL_CACHE_BUSTING_VALUE = "X-IG-Extended-CDN-Thumbnail-Cache-Busting-Value";
    public const string HEADER_X_IG_BLOKS_VERSION_ID = "X-Bloks-Version-Id";
    public const string HEADER_X_IG_BLOKS_IS_LAYOUT_RTL = "X-Bloks-Is-Layout-RTL";
    public const string HEADER_X_IG_BLOKS_ENABLE_RENDERCODE = "X-Bloks-Enable-RenderCore";
    public const string HEADER_X_IG_DEVICE_ID = "X-IG-Device-ID";
    public const string HEADER_X_IG_ANDROID_ID = "X-IG-Android-ID";
    public const string CURRENT_BLOKS_VERSION_ID = "e538d4591f238824118bfcb9528c8d005f2ea3becd947a3973c030ac971bb88e";
    public const string HOST = "Host";
    public const string HOST_URI = "i.instagram.com";
    public const string ACCEPT_ENCODING = "gzip, deflate, sdch";
    public const string API = "/api";
    public const string API_SUFFIX = API + API_VERSION;
    public const string API_SUFFIX_V2 = API + API_VERSION_V2;
    public const string API_VERSION = "/v1";
    public const string API_VERSION_V2 = "/v2";
    public const string BASE_INSTAGRAM_API_URL = INSTAGRAM_URL + API_SUFFIX + "/";
    public const string COMMENT_BREADCRUMB_KEY = "iN4$aGr0m";
    public const string CSRFTOKEN = "csrftoken";
    public const string HEADER_ACCEPT_ENCODING = "gzip, deflate, sdch";
    public const string HEADER_ACCEPT_LANGUAGE = "Accept-Language";
    public const string HEADER_COUNT = "count";
    public const string HEADER_EXCLUDE_LIST = "exclude_list";
    public const string HEADER_IG_APP_ID = "X-IG-App-ID";
    public const string HEADER_IG_CAPABILITIES = "X-IG-Capabilities";
    public const string HEADER_IG_CONNECTION_TYPE = "X-IG-Connection-Type";
    public const string HEADER_IG_SIGNATURE = "signed_body";
    public const string HEADER_IG_SIGNATURE_KEY_VERSION = "ig_sig_key_version";
    public const string HEADER_MAX_ID = "max_id";
    public const string HEADER_PHONE_ID = "phone_id";
    public const string HEADER_QUERY = "q";
    public const string HEADER_RANK_TOKEN = "rank_token";
    public const string HEADER_TIMEZONE = "timezone_offset";
    public const string HEADER_USER_AGENT = "User-Agent";
    public const string HEADER_X_INSTAGRAM_AJAX = "X-Instagram-AJAX";
    public const string HEADER_X_REQUESTED_WITH = "X-Requested-With";
    public const string HEADER_XCSRF_TOKEN = "X-CSRFToken";
    public const string HEADER_XGOOGLE_AD_IDE = "X-Google-AD-ID";
    public const string HEADER_XML_HTTP_REQUEST = "XMLHttpRequest";
    public const string IG_APP_ID = "567067343352427";
    public const string IG_CONNECTION_TYPE = "WIFI";
    public const string IG_SIGNATURE_KEY_VERSION = "4";
    public const string INSTAGRAM_URL = "https://i.instagram.com";
    public const string P_SUFFIX = "p/";
    public const string SUPPORTED_CAPABALITIES_HEADER = "supported_capabilities_new";

    public const string USER_AGENT = "Instagram {6} Android ({7}/{8}; {0}; {1}; {2}; {3}; {4}; {5}; en_US; {9})";
    public static readonly JArray SupportedCapabalities = new JArray
        {
            new JObject
            {
                {"name","SUPPORTED_SDK_VERSIONS"},
                {"value","45.0,46.0,47.0,48.0,49.0,50.0,51.0,52.0,53.0,54.0,55.0,56.0,57.0,58.0,59.0,60.0,61.0," +
                    "62.0,63.0,64.0,65.0,66.0,67.0,68.0,69.0,70.0,71.0,72.0,73.0,74.0,75.0,76.0,77.0,78.0,79.0,80.0"}
            },
            new JObject
            {
                {"name","FACE_TRACKER_VERSION"},
                {"value","12"}
            },
            new JObject
            {
                {"name","COMPRESSION"},
                {"value","ETC2_COMPRESSION"}
            },
            new JObject
            {
                {"name","WORLD_TRACKER"},
                {"value","WORLD_TRACKER_ENABLED"}
            }
        };
    public const string LOGIN_EXPERIMENTS_CONFIGS = "ig_android_fci_onboarding_friend_search,ig_android_device_detection_info_upload,ig_android_account_linking_upsell_universe,ig_android_direct_main_tab_universe_v2,ig_android_sign_in_help_only_one_account_family_universe,ig_android_sms_retriever_backtest_universe,ig_android_direct_add_direct_to_android_native_photo_share_sheet,ig_android_spatial_account_switch_universe,ig_growth_android_profile_pic_prefill_with_fb_pic_2,ig_account_identity_logged_out_signals_global_holdout_universe,ig_android_prefill_main_account_username_on_login_screen_universe,ig_android_login_identifier_fuzzy_match,ig_android_mas_remove_close_friends_entrypoint,ig_android_shared_email_reg_universe,ig_android_video_render_codec_low_memory_gc,ig_android_custom_transitions_universe,ig_android_push_fcm,multiple_account_recovery_universe,ig_android_show_login_info_reminder_universe,ig_android_email_fuzzy_matching_universe,ig_android_one_tap_aymh_redesign_universe,ig_android_direct_send_like_from_notification,ig_android_suma_landing_page,ig_android_prefetch_debug_dialog,ig_android_smartlock_hints_universe,ig_android_black_out,ig_activation_global_discretionary_sms_holdout,ig_android_video_ffmpegutil_pts_fix,ig_android_multi_tap_login_new,ig_save_smartlock_universe,ig_android_caption_typeahead_fix_on_o_universe,ig_android_enable_keyboardlistener_redesign,ig_android_nux_add_email_device,ig_android_direct_remove_view_mode_stickiness_universe,ig_android_hide_contacts_list_in_nux,ig_android_new_users_one_tap_holdout_universe,ig_android_ingestion_video_support_hevc_decoding,ig_android_mas_notification_badging_universe,ig_android_secondary_account_in_main_reg_flow_universe,ig_android_secondary_account_creation_universe,ig_android_account_recovery_auto_login,ig_android_pwd_encrytpion,ig_android_bottom_sheet_keyboard_leaks,ig_android_sim_info_upload,ig_android_shorten_sac_for_one_eligible_main_account_universe,ig_android_mobile_http_flow_device_universe,ig_android_hide_fb_button_when_not_installed_universe,ig_android_targeted_one_tap_upsell_universe,ig_android_gmail_oauth_in_reg,ig_android_vc_interop_use_test_igid_universe,ig_android_notification_unpack_universe,ig_android_quickcapture_keep_screen_on,ig_android_registration_confirmation_code_universe,ig_android_device_based_country_verification,ig_android_log_suggested_users_cache_on_error,ig_android_reg_modularization_universe,ig_android_device_verification_separate_endpoint,ig_android_universe_noticiation_channels,ig_android_account_linking_universe,ig_android_hsite_prefill_new_carrier,ig_android_one_login_toast_universe,ig_android_retry_create_account_universe,ig_android_family_apps_user_values_provider_universe,ig_android_reg_nux_headers_cleanup_universe,ig_android_get_cookie_with_concurrent_session_universe,ig_android_device_info_foreground_reporting,ig_android_shortcuts_2019,ig_android_device_verification_fb_signup,ig_android_onetaplogin_optimization,ig_android_passwordless_account_password_creation_universe,ig_android_black_out_toggle_universe,ig_video_debug_overlay,ig_android_ask_for_permissions_on_reg,ig_assisted_login_universe,ig_android_security_intent_switchoff,ig_android_device_info_job_based_reporting,ig_android_add_account_button_in_profile_mas_universe,ig_android_passwordless_auth,ig_android_direct_main_tab_account_switch,ig_android_recovery_one_tap_holdout_universe,ig_android_modularized_dynamic_nux_universe,ig_android_sac_follow_from_other_accounts_nux_universe,ig_android_fb_account_linking_sampling_freq_universe,ig_android_fix_sms_read_lollipop,ig_android_access_flow_prefill";

    public static string ACCEPT_LANGUAGE = "en-US";

    public const string WEB_USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 OPR/57.0.3098.116";

    public const string ERROR_OCCURRED = "Oops, an error occurred";

    public static readonly Uri BaseInstagramUri = new Uri(BASE_INSTAGRAM_API_URL);
    #endregion Main

    #region Account endpoints constants
    public const string ACCOUNTS_2FA_LOGIN = API_SUFFIX + "/accounts/two_factor_login/";
    public const string ACCOUNTS_2FA_LOGIN_AGAIN = API_SUFFIX + "/accounts/send_two_factor_login_sms/";
    public const string ACCOUNTS_CHANGE_PROFILE_PICTURE = API_SUFFIX + "/accounts/change_profile_picture/";
    public const string ACCOUNTS_CHECK_PHONE_NUMBER = API_SUFFIX + "/accounts/check_phone_number/";
    public const string ACCOUNTS_CONTACT_POINT_PREFILL = API_SUFFIX + "/accounts/contact_point_prefill/";
    public const string ACCOUNTS_CREATE = API_SUFFIX + "/accounts/create/";
    public const string ACCOUNTS_CREATE_VALIDATED = API_SUFFIX + "/accounts/create_validated/";
    public const string ACCOUNTS_DISABLE_SMS_TWO_FACTOR = API_SUFFIX + "/accounts/disable_sms_two_factor/";
    public const string ACCOUNTS_EDIT_PROFILE = API_SUFFIX + "/accounts/edit_profile/";
    public const string ACCOUNTS_ENABLE_SMS_TWO_FACTOR = API_SUFFIX + "/accounts/enable_sms_two_factor/";
    public const string ACCOUNTS_GET_COMMENT_FILTER = API_SUFFIX + "/accounts/get_comment_filter/";
    public const string ACCOUNTS_LOGIN = API_SUFFIX + "/accounts/login/";
    public const string ACCOUNTS_LOGOUT = API_SUFFIX + "/accounts/logout/";
    public const string ACCOUNTS_READ_MSISDN_HEADER = API_SUFFIX + "/accounts/read_msisdn_header/";
    public const string ACCOUNTS_REGEN_BACKUP_CODES = API_SUFFIX + "/accounts/regen_backup_codes/";
    public const string ACCOUNTS_REMOVE_PROFILE_PICTURE = API_SUFFIX + "/accounts/remove_profile_picture/";
    public const string ACCOUNTS_REQUEST_PROFILE_EDIT = API_SUFFIX + "/accounts/current_user/?edit=true";
    public const string ACCOUNTS_SECURITY_INFO = API_SUFFIX + "/accounts/account_security_info/";
    public const string ACCOUNTS_SEND_CONFIRM_EMAIL = API_SUFFIX + "/accounts/send_confirm_email/";
    public const string ACCOUNTS_SEND_RECOVERY_EMAIL = API_SUFFIX + "/accounts/send_recovery_flow_email/";
    public const string ACCOUNTS_SEND_SIGNUP_SMS_CODE = API_SUFFIX + "/accounts/send_signup_sms_code/";
    public const string ACCOUNTS_SEND_SMS_CODE = API_SUFFIX + "/accounts/send_sms_code/";
    public const string ACCOUNTS_SEND_TWO_FACTOR_ENABLE_SMS = API_SUFFIX + "/accounts/send_two_factor_enable_sms/";
    public const string ACCOUNTS_SET_BIOGRAPHY = API_SUFFIX + "/accounts/set_biography/";
    public const string ACCOUNTS_SET_PHONE_AND_NAME = API_SUFFIX + "/accounts/set_phone_and_name/";
    public const string ACCOUNTS_SET_PRESENCE_DISABLED = API_SUFFIX + "/accounts/set_presence_disabled/";
    public const string ACCOUNTS_UPDATE_BUSINESS_INFO = API_SUFFIX + "/accounts/update_business_info/";
    public const string ACCOUNTS_USERNAME_SUGGESTIONS = API_SUFFIX + "/accounts/username_suggestions/";
    public const string ACCOUNTS_VALIDATE_SIGNUP_SMS_CODE = API_SUFFIX + "/accounts/validate_signup_sms_code/";
    public const string ACCOUNTS_VERIFY_SMS_CODE = API_SUFFIX + "/accounts/verify_sms_code/";
    public const string CHANGE_PASSWORD = API_SUFFIX + "/accounts/change_password/";
    public const string CURRENTUSER = API_SUFFIX + "/accounts/current_user/?edit=true";
    public const string SET_ACCOUNT_PRIVATE = API_SUFFIX + "/accounts/set_private/";
    public const string SET_ACCOUNT_PUBLIC = API_SUFFIX + "/accounts/set_public/";
    public const string ACCOUNTS_CONVERT_TO_PERSONAL = API_SUFFIX + "/accounts/convert_to_personal/";
    public const string ACCOUNTS_CREATE_BUSINESS_INFO = API_SUFFIX + "/accounts/create_business_info/";
    public const string ACCOUNTS_GET_PRESENCE = API_SUFFIX + "/accounts/get_presence_disabled/";
    public const string ACCOUNTS_GET_BLOCKED_COMMENTERS = API_SUFFIX + "/accounts/get_blocked_commenters/";
    public const string ACCOUNTS_SET_BLOCKED_COMMENTERS = API_SUFFIX + "/accounts/set_blocked_commenters/";

    #endregion Account endpoint constants

    #region Other endpoints constants
    public const string LAUNCHER_SYNC = API_SUFFIX + "/launcher/sync/";
    public const string QE_SYNC = API_SUFFIX + "/qe/sync/";
    public const string ACCOUNTS_GET_PREFILL_CANDIDATES = API_SUFFIX + "/accounts/get_prefill_candidates/";
    #endregion

    #region Friendship endpoints constants
    public const string FRIENDSHIPS_UNFOLLOW_USER = API_SUFFIX + "/friendships/destroy/{0}/";
    public const string FRIENDSHIPS_USER_FOLLOWING = API_SUFFIX + "/friendships/{0}/following/?rank_token={1}";
    public const string FRIENDSHIPS_USER_FOLLOWERS = API_SUFFIX + "/friendships/{0}/followers/?rank_token={1}";
    #endregion
}
