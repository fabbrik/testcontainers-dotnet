namespace Testcontainers.Spanner;

/// <inheritdoc cref="DockerContainer" />
[PublicAPI]
public sealed class SpannerContainer : DockerContainer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpannerContainer" /> class.
    /// </summary>
    /// <param name="configuration">The container configuration.</param>
    public SpannerContainer(PubSubConfiguration configuration)
        : base(configuration)
    {
    }

    /// <summary>
    /// Gets the Spanner emulator endpoint.
    /// </summary>
    /// <returns>The Spanner emulator endpoint.</returns>
    public string GetEmulatorEndpoint()
    {
        return new UriBuilder(Uri.UriSchemeHttp, Hostname, GetMappedPublicPort(PubSubBuilder.PubSubPort)).ToString();
    }
}