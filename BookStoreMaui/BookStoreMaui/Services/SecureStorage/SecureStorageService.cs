namespace BookStoreMaui.Services.SecureStorage
{
    public class SecureStorageService : SecureStorageServiceBase, ISecureStorageService
    {
        public async Task SetAccessTokenAsync(string accessToken) 
            => await SaveString("accessToken", accessToken);

        public async Task SetRefreshTokenAsync(string refreshToken) 
            => await SaveString("refreshToken", refreshToken);

        public async Task SetIdentityTokenTokensAsync(string identityToken) 
            => await SaveString("identityToken", identityToken);

        public async Task<string> GetIdentityTokenTokenAsync() 
            => await GetString("identityToken", "");

        public async Task<string> GetAccessTokenAsync()
            => await GetString("accessToken", "");
        
        public async Task RemoveAccessTokenAsync()
            => await ClearAsync("accessToken");
        
        public async Task RemoveRefreshTokenAsync()
            => await ClearAsync("refreshToken");
    }
}