namespace Testcontainers.Spanner;

/// <inheritdoc cref="DockerContainer" />
[PublicAPI]
public sealed class SpannerContainer : DockerContainer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpannerContainer" /> class.
    /// </summary>
    /// <param name="configuration">The container configuration.</param>
    public SpannerContainer(SpannerConfiguration configuration)
        : base(configuration)
    {
    }
}
