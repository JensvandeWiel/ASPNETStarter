namespace ASPNETStarter.Server.Extensions;

public class ExtendedInfoResponse
{
    /// <summary>
    ///     The ID of the authenticated user.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    ///     The email address associated with the authenticated user.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    ///     Indicates whether or not the <see cref="Email" /> has been confirmed yet.
    /// </summary>
    public required bool IsEmailConfirmed { get; init; }

    /// <summary>
    ///     The username of the authenticated user.
    /// </summary>
    public required string UserName { get; init; }

    /// <summary>
    ///     Roles assigned to the user.
    /// </summary>
    public required IEnumerable<string> Roles { get; init; }
}