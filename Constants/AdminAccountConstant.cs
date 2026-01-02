namespace ManhwaDimension.Constants
{
    public class AdminAccountConstant
    {
        public const string DEFAULT_PHOTO = "/uploads/admin/default_avatar.jpg";
        /// <summary>
        /// super admin id
        /// </summary>
        public const int SUPER_ADMIN_ID = 1001;
        public const int ADMIN_ID = 1001;

        public static int[] ADMIN_ROLE = new int[] { SUPER_ADMIN_ID, ADMIN_ID };
        public static class Status
        {
            public const int Active = 1001;
            public const int InActive = 1002;
        }
    }
}
