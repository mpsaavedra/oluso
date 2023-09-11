#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Oluso.Extensions;

/// <summary>
/// <see cref="ClaimsPrincipal"/> related extensions
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// returns true if user is authenticated
    /// </summary>
    /// <param name="claimsPrincipal"></param>
    /// <returns></returns>
    public static bool ToIsAuthenticated(this ClaimsPrincipal claimsPrincipal)
    {
        var identity = claimsPrincipal.IsNullOrEmptyThrow(nameof(claimsPrincipal)).Identity;

        return !(identity is null) && identity.IsAuthenticated;
    }

    /// <summary>
    /// returns the authentication type used in this claim
    /// </summary>
    /// <param name="claimsPrincipal"></param>
    /// <returns></returns>
    public static string? ToAuthenticationType(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.IsNullOrEmptyThrow(nameof(claimsPrincipal)).Identity?.AuthenticationType;

    /// <summary>
    /// returns the username if specified
    /// </summary>
    /// <param name="claimsPrincipal"></param>
    /// <returns></returns>
    public static string? ToUserName(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.ToGet<string>(ClaimTypes.Name);

    /// <summary>
    /// return true if requested claim is registered 
    /// </summary>
    /// <param name="claimsPrincipal"></param>
    /// <param name="claimType"></param>
    /// <returns></returns>
    public static bool ToExists(this ClaimsPrincipal claimsPrincipal, string claimType) =>
        claimsPrincipal
            .IsNullOrEmptyThrow(nameof(claimsPrincipal))
            .HasClaim(x => x.Type == claimType);

    /// <summary>
    /// Add claims to the identity in the claims principal
    /// </summary>
    /// <param name="claimsPrincipal"></param>
    /// <param name="authenticationType"></param>
    /// <param name="claims"></param>
    public static void ToAdd(this ClaimsPrincipal claimsPrincipal, string authenticationType,
        IEnumerable<Claim> claims) =>
        claimsPrincipal
            .IsNullOrEmptyThrow(nameof(claimsPrincipal))
            .AddIdentity(new ClaimsIdentity(claims, authenticationType));

    /// <summary>
    /// get first claim by his type. result will deserialized to the TResult. if it does not exists
    /// it returns null.
    /// </summary>
    /// <param name="claimsPrincipal"></param>
    /// <param name="claimType"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static TResult? ToGet<TResult>(this ClaimsPrincipal claimsPrincipal, string claimType)
        where TResult : class =>
        claimsPrincipal
            .IsNullOrEmptyThrow(nameof(claimsPrincipal))
            .FindFirst(claimType)?
            .Value
            .ToDeserialize<TResult>();

    /// <summary>
    /// returns the first value of a given claim if is registered in the source 
    /// </summary>
    /// <param name="claimsPrincipal"></param>
    /// <param name="claimType"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static TResult? TGetValue<TResult>(this ClaimsPrincipal claimsPrincipal, string claimType)
        where TResult : struct =>
        claimsPrincipal
            .IsNullOrEmptyThrow(nameof(claimsPrincipal))
            .FindFirst(claimType)?
            .Value
            .ToDeserialize<TResult>();

    /// <summary>
    /// returns a list with the deserialized claims of a given type
    /// </summary>
    /// <param name="claimsPrincipal"></param>
    /// <param name="claimType"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TResult?>? ToGetAll<TResult>(this ClaimsPrincipal claimsPrincipal, string claimType) =>
        claimsPrincipal
            .IsNullOrEmptyThrow(nameof(claimsPrincipal))
            .FindAll(claimType)?
            .Select(x => x.Value.ToDeserialize<TResult>());
}