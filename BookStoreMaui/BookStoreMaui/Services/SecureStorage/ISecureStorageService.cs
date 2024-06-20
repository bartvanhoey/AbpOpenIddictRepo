namespace BookStoreMaui.Services.SecureStorage
{
    public interface ISecureStorageService
    {
        Task SetAccessTokenAsync(string accessToken);
        Task SetRefreshTokenAsync(string refreshToken);
        Task SetIdentityTokenTokensAsync( string identityToken);
        Task<string> GetIdentityTokenTokenAsync();
        Task<string> GetAccessTokenAsync();
        Task RemoveAccessTokenAsync();
        Task RemoveRefreshTokenAsync();
    }
}