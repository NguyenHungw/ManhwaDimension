//using System.Security.Cryptography;
//using System.Text;

//namespace ManhwaDimension.ULT
//{
//    public class RandomSecurityULT
//    {
//        public static string ConvertToURL(string text)
//        {
//            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
//                "đ",
//                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
//                "í","ì","ỉ","ĩ","ị",
//                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
//                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
//                "ý","ỳ","ỷ","ỹ","ỵ",};
//            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
//                "d",
//                "e","e","e","e","e","e","e","e","e","e","e",
//                "i","i","i","i","i",
//                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
//                "u","u","u","u","u","u","u","u","u","u","u",
//                "y","y","y","y","y",};
//            for (int i = 0; i < arr1.Length; i++)
//            {
//                text = text.Replace(arr1[i], arr2[i]);
//                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
//            }
//            text = text.Replace(" ", "-");
//            text = text.Replace("+", "-");
//            text = text.Replace("/", "-");
//            text = text.Replace("%", "");
//            return text;
//        }

//        private static Random random = new Random();
//        public static string RandomString(int length)
//        {
//            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
//            return new string(Enumerable.Repeat(chars, length)
//              .Select(s => s[random.Next(s.Length)]).ToArray());
//        }
//        public static string RandomSecurityString(int length)
//        {
//            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
//            StringBuilder res = new StringBuilder();
//            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
//            {
//                byte[] uintBuffer = new byte[sizeof(uint)];
//                while (length-- > 0)
//                {
//                    rng.GetBytes(uintBuffer);
//                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
//                    res.Append(valid[(int)(num % (uint)valid.Length)]);
//                }
//            }
//            return res.ToString();
//        }
//        public static string RandomSecurityPasswordString(int length)
//        {
//            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
//            StringBuilder res = new StringBuilder();
//            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
//            {
//                byte[] uintBuffer = new byte[sizeof(uint)];
//                while (length-- > 0)
//                {
//                    rng.GetBytes(uintBuffer);
//                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
//                    res.Append(valid[(int)(num % (uint)valid.Length)]);
//                }
//            }
//            return res.ToString();
//        }
//        public static string RandomSecurityNumber(int length)
//        {
//            Random rnd = new Random();
//            int rand_num = rnd.Next(100000, length);
//            return rand_num.ToString();
//        }
//    }
//}

