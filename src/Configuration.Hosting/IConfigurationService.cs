using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Oluso.Configuration.Hosting;

/// <summary>
/// Define used methods in the configuration service to handle event
/// has been keep as simple as possible to allow to use different types
/// of providers and publishers.
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// initialize the configuration service
    /// </summary>
    /// <param name="cancellationToken"></param>
    Task Initialize(CancellationToken cancellationToken = default);

    /// <summary>
    /// called when changes are detected
    /// </summary>
    /// <param name="paths">list of paths that change</param>
    Task OnChange(IEnumerable<string> paths);

    /// <summary>
    /// publish changes in configuration paths
    ///</summary>
    ///<param name="paths">list of paths that changes</param>
    Task PublishChanges(IEnumerable<string> paths);
}