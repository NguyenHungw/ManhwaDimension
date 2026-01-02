namespace ManhwaDimension.Constants
{
    public class FolderUploadConstant
    {
        public static int[] INSTANT_FOLDERS = { 1001 };
        /// <summary>
        /// Folder upload id
        /// </summary>
        public const int FOLDER_UPLOAD_ID = 1001;
        /// <summary>
        /// Folder upload path
        /// </summary>
        public const string FOLDER_UPLOAD_PATH = @"\uploads";
        /// <summary>
        /// Folder upload name
        /// </summary>
        public const string FOLDER_UPLOAD_NAME = "uploads";
        /// <summary>
        /// Default folder thumb name
        /// </summary>
        public const string FOLDER_THUMB_NAME = "thumbnails";
        /// <summary>
        /// Mã folder ứng viên
        /// </summary>
        public const int FOLDER_CANDIDATE_ID = 1002;
        /// <summary>
        /// Tên folder ứng viên
        /// </summary>
        public const string FOLDER_CANDIDATE = "candidate";
        /// <summary>
        /// Đường dẫn folder ứng viên
        /// </summary>
        public const string FOLDER_CANDIDATE_PATH = @"\uploads\candidate";
        /// <summary>
        /// Mã folder nhà tuyển dụng
        /// </summary>
        public const int FOLDER_EMPLOYER_ID = 1003;
        /// <summary>
        /// Tên folder nhà tuyển dụng
        /// </summary>
        public const string FOLDER_EMPLOYER = "employer";
        /// <summary>
        /// Đường dẫn folder nhà tuyển dụng
        /// </summary>
        public const string FOLDER_EMPLOYER_PATH = @"\uploads\employer";
        /// <summary>
        /// DEFAULT EMPTY IMAGE
        /// </summary>
        public const string FOLDER_EMTPY_IMG = "";
        /// <summary>
        /// Tên thư mục chứa wwwrot của Source API
        /// </summary>
        public const string ROOT_API_NAME = "Jobi.API";
        /// <summary>
        /// Tên thư mục chứa wwwrot của Source Storage
        /// </summary>
        public const string ROOT_STORAGE_NAME = "Jobi.Storage";
        /// <summary>
        /// Tên thư mục root của web
        /// </summary>
        public const string ROOT_WEB_NAME = "wwwroot";
        /// <summary>
        /// Mã thư mục avartar của ứng viên
        /// </summary>
        public const int CANDIDATE_AVATAR_ID = 1005;
        /// <summary>
        /// Mã thư mục CV của ứng viên
        /// </summary>
		public const int CANDIDATE_CV_ID = 1006;
        /// <summary>
        /// Mã thư mục avata của nhà tuyển dụng
        /// </summary>
        public const int EMPLOYER_AVATAR_ID = 1007;
        /// <summary>
        /// Mã thư mục cấp phép của nhà tuyển dụng
        /// </summary>
		public const int EMPLOYER_LICENSE_ID = 1008;
        /// <summary>
        /// Mã thư mục cover photo của ứng viên
        /// </summary>
		public const int CANDIDATE_COVER_PHOTO_ID = 1014;
        /// <summary>
        /// Tên thư mục cover photo của ứng viên
        /// </summary>
        public const string CANDIDATE_COVER_PHOTO_NAME = COVER_PHOTO_TYPE;
        /// <summary>
        /// Mã thư mục cover photo của ứng viên
        /// </summary>
        public const int EMPLOYER_COVER_PHOTO_ID = 1015;
        public const string EMPLOYER_COVER_PHOTO_NAME = COVER_PHOTO_TYPE;
        /// <summary>
        /// type avatar when curent user upload file
        /// </summary>
        public const string AVATAR_TYPE = "avatars";
        /// <summary>
        /// type cover photo when curent user upload file
        /// </summary>
        public const int COMPANY_AVATAR_ID = 1017;
        public const int COMPANY_COVER_PHOTO_ID = 1018;
        public const int COMPANY_LICENSES_ID = 1019;
        public const string LICENSE_TYPE = "licenses";
        public const string COVER_PHOTO_TYPE = "cover-photo";
        public readonly static Dictionary<string, Dictionary<string, int>> MAPPING = new Dictionary<string, Dictionary<string, int>>{
            {"candidates" ,new Dictionary<string, int>(){
                {AVATAR_TYPE,CANDIDATE_AVATAR_ID },
                {"cvs",CANDIDATE_CV_ID },
                { COVER_PHOTO_TYPE,CANDIDATE_COVER_PHOTO_ID}
            }},
             {"employers" ,new Dictionary<string, int>(){
                {AVATAR_TYPE,EMPLOYER_AVATAR_ID },
                { COVER_PHOTO_TYPE,EMPLOYER_COVER_PHOTO_ID}
            }},
             {"companys" ,new Dictionary<string, int>(){
                {AVATAR_TYPE,COMPANY_AVATAR_ID },
                { COVER_PHOTO_TYPE,COMPANY_COVER_PHOTO_ID},
                 {LICENSE_TYPE, COMPANY_LICENSES_ID}
            }}
        };
    }
}

