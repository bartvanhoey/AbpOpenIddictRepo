namespace BookStoreMaui.Services.SecureStorage
{
    public interface ISecureStorageService
    {

        
        Task SetAccessTokenAsync(string accessToken);
        Task SetRefreshTokenAsync(string refreshToken);
        Task SetIdentityTokenTokensAsync( string identityToken);
        Task ClearLoginTokensAsync();
        Task<string> GetIdentityTokenTokenAsync();
        Task<string> GetAccessTokenAsync();
        Task SaveFireBaseCredentialAsync(string credential);
        Task<string> GetFireBaseCredentialAsync();
        Task SaveFireBaseUserInfoAsync(string userInfo);
        Task<string> GetFireBaseUserInfoAsync();
        Task RemoveAccessTokenAsync();
        Task RemoveRefreshTokenAsync();
    }
}