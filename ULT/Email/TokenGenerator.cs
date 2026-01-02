//using System.Security.Cryptography;

//namespace ManhwaDimension.ULT.Email
//{
//    public class TokenGenerator
//    {
//        private static readonly RandomNumberGenerator RandomGenerator = RandomNumberGenerator.Create();

//        public static string GenerateTokenEmail(int length)
//        {
//            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

//            var tokenData = new char[length];
//            var randomBytes = new byte[1];

//            using (var rng = RandomGenerator)
//            {
//                for (var i = 0; i < length; i++)
//                {
//                    rng.GetBytes(randomBytes);
//                    tokenData[i] = allowedChars[randomBytes[0] % allowedChars.Length];
//                }
//            }

//            return new string(tokenData);
//        }

//    }
//}
