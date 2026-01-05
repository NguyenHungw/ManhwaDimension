
using ManhwaDimension.Models.Response;

namespace ManhwaDimension.Util.Entities
{
    public class SignIningResponse
    {
        /// <summary>
        /// Authorization token
        /// </summary>
        public string AccessToken { get; set; } = null!;

        public string Data { get; set; } = null!;

    }
    public class SignInResponse
    {
        /// <summary>
        /// Authorization token
        /// </summary>
        public string AccessToken { get; set; } = null!;

        public AccountProfileResponseDTO Profile { get; set; } = null!;
        public DateTime ExpireTime { get; set; }
        public string ExpireTimeFormatted { get; set; }
    }
}
