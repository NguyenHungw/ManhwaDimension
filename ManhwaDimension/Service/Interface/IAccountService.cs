using ManhwaDimension.Models;
using ManhwaDimension.Models.DTO.Account;
using ManhwaDimension.Models.Response;
using Microsoft.Playwright;

namespace ManhwaDimension.Service.Interface
{
    public interface IAccountService
    {

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <returns></returns>
        Task<ManhwaDimensionResponse> Login(LoginAccountDTO model);
        /// <summary>
        /// Đăng ký tài khoản 
        /// </summary>
        /// <returns></returns>
        Task<ManhwaDimensionResponse> Register(InsertAccountDTO dto);
        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        Task<ManhwaDimensionResponse> ChangePassword(ChangePasswordDTO obj, long accountId);
        /// <summary>
        /// Cập nhật thông tin tài khoản
        /// </summary>
        /// <returns></returns>
        Task<ManhwaDimensionResponse> UpdateProfile(UpdateAccountDTO model);
        /// <summary>
        /// Cập nhật ảnh đại diện
        /// </summary>
        /// <returns></returns>
        Task<ManhwaDimensionResponse> ChangeAvatar(User model, IFormFile file);
        /// <summary>
        /// Thông tin tài khoản
        /// </summary>
        /// <returns></returns>
        Task<AccountProfileDTO> GetProfile(long id = 0);
        /// <summary>
        /// Quên mật khẩu
        /// </summary>
        /// <returns></returns>
        Task<ManhwaDimensionResponse> ForgotPassword(string value);
        /// <summary>
        /// Xác thực quên mật khẩu
        /// </summary>
        /// <returns></returns>
        Task<List<User>> ListIncludeAddress();
        Task<ManhwaDimensionResponse> VerifyChangePassword(ForgotPasswordDTO dto);
        Task<ManhwaDimensionResponse> UpdateAdmin(User model);
        //Task<InfoProfileViewModel> GetInfoAccountById(long accountId);
        Task<ManhwaDimensionResponse> SignInWithGoogle(SignInWithSocialNetworkDTO obj);
        Task<ManhwaDimensionResponse> SignInWithFacebook(SignInWithSocialNetworkDTO obj);
        Task<ManhwaDimensionResponse> SignInWithApple(SignInWithSocialNetworkDTO obj);
        Task<bool> UpdateInfoById(long accountId, string fullName, string photo);
        //Task<bool> UpdatePasswordById(long accountId, ChangePasswordDTO changePassword);

        //Task<ManhwaDimensionResponse> UpdateStatus(UpdateAccountStatusRequest request);
        Task<ManhwaDimensionResponse> CheckStatus(long accountId);


        Task<List<User>> ListByRole(long roleId);
        Task<ManhwaDimensionResponse> RefreshTokenAsync(RefreshTokenRequestDTO obj);
    }
}
