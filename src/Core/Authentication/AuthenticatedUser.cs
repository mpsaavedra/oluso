namespace Oluso.Authentication;

/// <summary>
/// basic information of authenticated users
/// </summary>
/// <typeparam name="TUserKey"></typeparam>
public interface IAuthenticatedUser<TUserKey>
{
    /// <summary>
    /// Id of authenticated user
    /// </summary>
    TUserKey UserId { get; set; }
}

/// <summary>
/// <inheritdoc cref="IAuthenticatedUser{TUserKey}"/>
/// </summary>
/// <typeparam name="TUserKey"></typeparam>
public class AuthenticatedUser<TUserKey> : IAuthenticatedUser<TUserKey>
{
    /// <summary>
    /// <inheritdoc cref="IAuthenticatedUser{TUserKey}.UserId"/>
    /// </summary>
    public TUserKey UserId { get; set; }
}