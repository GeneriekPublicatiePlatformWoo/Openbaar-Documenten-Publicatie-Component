namespace ODPC.Authentication
{
    public record OdpcUser
    {
        public required bool IsLoggedIn { get; init; }
        public required bool IsAdmin { get; init; }
        public required string? Id { get; init; }
        public required string? FullName { get; init; }
        public required string[] Roles { get; init; }
    }
}
