using LabAllianceTest.Helpers;

namespace LabAllianceTest.Abstractions
{
    internal interface IFileStorage
    {
        Task<string> ReadAccessTokenFromFileJsonAsync();
        Task<string> ReadRefreshTokenFromFileJsonAsync();
        Task WriteToFileJsonAsync(TokenResponse tokenResponse);
    }
}