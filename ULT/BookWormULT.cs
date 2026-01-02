//using System.Security.Cryptography;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace ManhwaDimension.ULT
//{
//    public class BookWormULT
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
//        public static string RandomSecurityNumber(int length)
//        {
//            Random rnd = new Random();
//            int rand_num = rnd.Next(100000, length);
//            return rand_num.ToString();
//        }


//        public static string GetPostDefaultThumbnailPicture()
//        {
//            return "http://beta.Novatic.com/files/frontend/images/core/default.jpg";
//        }
//        public static string GetPostDefaultPictureProfessional()
//        {
//            return "http://auction.novatic.vn/files/frontend/images/core/defaultProfessional.png";
//        }

//        public static string Base64Decode(string base64EncodedData)
//        {
//            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
//            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
//        }

//        public static string Base64Encode(string plainText)
//        {
//            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
//            return System.Convert.ToBase64String(plainTextBytes);
//        }

//        public static string FormatDateTime(string inputDate)
//        {
//            string result = "";
//            if (inputDate != null)
//            {
//                string date = inputDate.Substring(0, inputDate.IndexOf("-"));
//                string month = inputDate.Substring(inputDate.IndexOf("-") + 1, 2);
//                string year = inputDate.Substring(inputDate.LastIndexOf("-") + 1);
//                result = year + "-" + month + "-" + date;
//            }
//            return result;
//        }

//        public static string FormarDateTimeVN(string inputDate)
//        {
//            string result = "";
//            if (inputDate != null)
//            {
//                if (inputDate.IndexOf("AM") > 0)
//                {
//                    result = inputDate.Replace("AM", "SA");
//                }
//                if (inputDate.IndexOf("PM") > 0)
//                {
//                    result = inputDate.Replace("PM", "CH");
//                }
//            }
//            return result;
//        }

//        public static bool IsValidEmail(string email)
//        {
//            try
//            {
//                var addr = new System.Net.Mail.MailAddress(email);
//                return addr.Address == email;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public static bool IsValidPhone(string Phone)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(Phone))
//                    return false;
//                var r = new Regex(@"^\d{10,}$");
//                return r.IsMatch(Phone);
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//        public static bool IsValidIdCardNumber(string number)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(number))
//                    return false;
//                var r = new Regex(@"^\d{9,}$");
//                return r.IsMatch(number);
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//        //public async void SendNotification(List<string> listParam,string keyNotification)
//        //{
//        //    ILanguageConfigRepository repositoryLanguageConfig;
//        //    listParam = new List<string>() { "1000001", "KeangNam", "1200000000", "5/3/2020 15:30:00" };
//        //    if (listParam.Count > 0)
//        //    {
//        //        LanguageConfigRepository obj = new LanguageConfigRepository();
//        //        //List<LanguageConfig> listLanguageConfig = await obj.DetailByCode(keyNotification);
//        //        List<LanguageConfig> listLanguageConfig = await repositoryLanguageConfig.DetailByCode(keyNotification);
//        //        if (listLanguageConfig != null)
//        //        {
//        //            string tempNotification = listLanguageConfig[0].Name;
//        //            string newTempNotification = "";
//        //            for (int i = 0; i < listParam.Count; i++)
//        //            {
//        //                string tempA = "";
//        //                string tempParam = "$$$param" + i;
//        //                tempA = listParam[i];
//        //                if (newTempNotification.Length == 0)
//        //                {
//        //                    newTempNotification = tempNotification.Replace(tempParam, tempA);
//        //                }
//        //                else
//        //                {
//        //                    newTempNotification = newTempNotification.Replace(tempParam, tempA);
//        //                }
//        //            }
//        //            string x = newTempNotification;
//        //        }
//        //    }
//        //}

//        public static string ConvertStringNoSign(string text)
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
//            text = text.Replace("\"", " ");
//            text = text.Replace("\'", " ");
//            text = text.Replace("%", " ");
//            text = text.Replace(",", " ");
//            text = text.Replace(".", " ");
//            return text;
//        }

//        public static string RemoveSpecialCharacters(string str)
//        {
//            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
//        }

//        public static string md5(string source_str)
//        {
//            var encrypter = new System.Security.Cryptography.MD5CryptoServiceProvider();
//            Byte[] original_bytes = System.Text.ASCIIEncoding.Default.GetBytes(source_str);
//            Byte[] encoded_bytes = encrypter.ComputeHash(original_bytes);
//            return BitConverter.ToString(encoded_bytes).Replace("-", "").ToLower();
//        }
//        public static string Mask(string? input)
//        {
//            if (string.IsNullOrEmpty(input)) return string.Empty;
//            if (input.Length <= 6) return new string('*', input.Length);
//            return input.Substring(0, 3) + new string('*', input.Length - 6) + input.Substring(input.Length - 3);
//        }
//    }
//}

