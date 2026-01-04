namespace ManhwaDimension.ULT
{
    public class SystemConstant
    {
        public static string ACTIVATION_PAGE_URL = "activation.html";
        public static string DEFAULT_URL = "https://localhost:44350/";
        public static string SECURITY_KEY_NAME = "GENE_STORY";
        public static string DEFAULT_PASSWORD = "Abc@123456";
        public static int FORGOT_PASSSWORD_SENT_EMAIL_TIME_LIMIT = 1;
        public static int FORGOT_PASSSWORD_SENT_EMAIL_LIMIT = 5;
        public static int FORGOT_PASSSWORD_TOKEN_EXPRIED = 1;
        public static string REGISTER_ACCOUNT_VERIFY_OTP = "REGISTER_ACCOUNT_VERIFY_OTP";
        public static string LOGIN_ACCOUNT_VERIFY_OTP = "LOGIN_ACCOUNT_VERIFY_OTP";
        public static string REGISTER_ACCOUNT_VERIFY_EMAIL = "REGISTER_ACCOUNT_VERIFY_EMAIL";
        #region Account Status Value
        public static int ACCOUNT_STATUS_ACTIVE = 1001;
        public static int ACCOUNT_STATUS_CANCELLED = 1002;
        #endregion
        #region Account Type Value
        public static int ACCOUNT_TYPE_DEFAULT = 1001;
        #endregion
        #region Role Value
        public static int ROLE_ANONYMOUS_USER = 0;
        public static int ROLE_SYSTEM_ADMIN = 1001;
        public static int ROLE_SUPER_ADMIN = 1001;
        public static int ROLE_HOSPITAL = 1002;
        public static int ROLE_USER = 1003;
        public static int ROLE_SYSTEM_ADMIN_GS_1 = 1004;
        public static int ROLE_SYSTEM_ADMIN_GS_2 = 1005;
        #endregion
        #region FormRequest
        public static long FORM_REQUEST_NEW = 1001;
        public static long FORM_REQUEST_RENEW = 1002;
        public static long FORM_REQUEST_UPDATE = 1003;

        public static readonly Dictionary<long, string> FORM_REQUEST_TYPES = new Dictionary<long, string>
        {
            { 1001, "Xác minh thân nhân" },
            { 1002, "Tìm kiếm tội phạm" }
        };
        #endregion

        #region Menu Client Config
        public static Dictionary<string, string> MENU_CLIENT_CONFIG_GS_1 = new Dictionary<string, string>()
        {
            { "tai-len-file-di-truyen", "Tải lên file di truyền" },
            { "thong-tin/dashboard", "Thông tin cá nhân" },
            { "quan-he-huyet-thong", "Quan hệ huyết thống" },
        };
        public static Dictionary<string, string> MENU_CLIENT_CONFIG_GS_2 = new Dictionary<string, string>()
        {
            { "tai-len-file-di-truyen", "Tải lên file di truyền" },
            { "dap-ung-thuoc", "Đáp ứng thuốc" },
            { "danh-sach-ket-qua-thong-ke", "Hồ sơ sức khỏe" },
        };
        public static Dictionary<string, Dictionary<string, string>> MENU_CLIENT_CONFIG =
        new Dictionary<string, Dictionary<string, string>>()
        {
            { "gs1", MENU_CLIENT_CONFIG_GS_1 },
            { "gs2", MENU_CLIENT_CONFIG_GS_2 }
        };

        #endregion
    }
}
