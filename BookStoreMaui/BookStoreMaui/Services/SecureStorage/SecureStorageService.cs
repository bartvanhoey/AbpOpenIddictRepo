namespace BookStoreMaui.Services.SecureStorage
{
    public class SecureStorageService : SecureStorageServiceBase, ISecureStorageService
    {
        public async Task SetLoginTokensAsync(string accessToken, string refreshToken)
        {
            await SaveString("accessToken", accessToken);
            await SaveString("refreshToken", refreshToken);
        }

        public async Task SetAccessTokenAsync(string accessToken)
        {
            await SaveString("accessToken", accessToken);
        }

        public async Task SetRefreshTokenAsync(string refreshToken)
        {
            await SaveString("refreshToken", refreshToken);
        }

        public async Task SetIdentityTokenTokensAsync(string identityToken)
        {
            await SaveString("identityToken", identityToken);
        }

        public async Task ClearLoginTokensAsync()
        {
            await ClearAsync("accessToken");
            await ClearAsync("refreshToken");
        }

        public async Task<string> GetIdentityTokenTokenAsync() 
            => await GetString("identityToken", "");

        public async Task<string> GetAccessTokenAsync()
            => await GetString("accessToken", "");

        public async Task SaveFireBaseCredentialAsync(string credential) 
            => await SaveString("fireBaseCredential", credential);

        public async Task<string> GetFireBaseCredentialAsync()
            => await GetString("fireBaseCredential", "");

        public async Task SaveFireBaseUserInfoAsync(string userInfo)
            => await SaveString("fireBaseUserInfo", userInfo);

        public async Task<string> GetFireBaseUserInfoAsync()
            => await GetString("fireBaseUserInfo", "");
        
        public async Task RemoveAccessTokenAsync()
            => await ClearAsync("accessToken");
        
        public async Task RemoveRefreshTokenAsync()
            => await ClearAsync("refreshToken");

    }
}