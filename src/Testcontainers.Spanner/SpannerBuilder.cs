using DotNet.Testcontainers;

namespace Testcontainers.Spanner;

/// <inheritdoc cref="ContainerBuilder{TBuilderEntity, TContainerEntity, TConfigurationEntity}" />
[PublicAPI]
public sealed class SpannerBuilder : ContainerBuilder<SpannerBuilder, SpannerContainer, SpannerConfiguration>
{
    public const string SpannerImage = "gcr.io/cloud-spanner-emulator/emulator:latest";

    internal const int GrpcPort = 9010;
    internal const int RestPort = 9020;

    public const string DefaultProjectId = "test-project";

    /// <summary>
    /// Initializes a new instance of the <see cref="SpannerBuilder" /> class.
    /// </summary>
    public SpannerBuilder()
        : this(new SpannerConfiguration())
    {
        DockerResourceConfiguration = Init().DockerResourceConfiguration;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpannerBuilder" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    private SpannerBuilder(SpannerConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
        DockerResourceConfiguration = resourceConfiguration;
    }

    /// <inheritdoc />
    protected override SpannerConfiguration DockerResourceConfiguration { get; }

    /// <inheritdoc />
    public override SpannerContainer Build()
    {
        return new SpannerContainer(DockerResourceConfiguration);
    }

    /// <inheritdoc />
    protected override SpannerBuilder Init()
    {
        return base.Init()
            .WithImage(SpannerImage)
            .WithPortBinding(GrpcPort, true)
            .WithPortBinding(RestPort, true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilMessageIsLogged($".+REST server listening at 0.0.0.0:{RestPort}")
                .UntilMessageIsLogged($".+gRPC server listening at 0.0.0.0:{GrpcPort}"));
    }

    /// <inheritdoc />
    protected override SpannerBuilder Clone(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new SpannerConfiguration(resourceConfiguration));
    }

    /// <inheritdoc />
    protected override SpannerBuilder Clone(IContainerConfiguration resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new SpannerConfiguration(resourceConfiguration));
    }

    /// <inheritdoc />
    protected override SpannerBuilder Merge(SpannerConfiguration oldValue, SpannerConfiguration newValue)
    {
        return new SpannerBuilder(new SpannerConfiguration(oldValue, newValue));
    }
}
