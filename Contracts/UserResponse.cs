namespace LabAllianceTest.Contracts
{
    public record UserResponse(
        Guid id,
        string login,
        string password
        );
}
