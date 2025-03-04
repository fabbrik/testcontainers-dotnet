namespace Testcontainers.Spanner;

/// <inheritdoc cref="DockerContainer" />
[PublicAPI]
public sealed class SpannerContainer : DockerContainer, IDatabaseContainer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpannerContainer" /> class.
    /// </summary>
    /// <param name="configuration">The container configuration.</param>
    public SpannerContainer(SpannerConfiguration configuration)
        : base(configuration)
    {
    }

    public string GetConnectionString()
    {
      return $"Data Source=projects/{SpannerBuilder.DefaultProjectId}/instances/test-instance/databases/test-database;EmulatorDetection=EmulatorOnly";
    }
}
