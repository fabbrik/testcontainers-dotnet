namespace Testcontainers.Spanner;

/// <inheritdoc cref="ContainerBuilder{TBuilderEntity, TContainerEntity, TConfigurationEntity}" />
[PublicAPI]
public sealed class SpannerBuilder : ContainerBuilder<SpannerBuilder, BigQueryContainer, BigQueryConfiguration>
{
    public const string SpannerImage = "gcr.io/cloud-spanner-emulator/emulator:latest";

    public const ushort[] SpannerPorts = { 9010, 9020 };

    public const string DefaultProjectId = "default";

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    public SpannerBuilder WithProject(string projectId)
    {
        return WithCommand("--project", projectId);
    }

    /// <inheritdoc />
    public override BigQueryContainer Build()
    {
        Validate();
        return new BigQueryContainer(DockerResourceConfiguration);
    }

    /// <inheritdoc />
    protected override SpannerBuilder Init()
    {
        return base.Init()
            .WithImage(SpannerImage)
            .WithPortBinding(SpannerPorts[0], true)
            .WithPortBinding(SpannerPorts[1], true)
            .WithProject(DefaultProjectId)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("(?s).*listening.*$"));
    }

    /// <inheritdoc />
    protected override SpannerBuilder Clone(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new BigQueryConfiguration(resourceConfiguration));
    }

    /// <inheritdoc />
    protected override SpannerBuilder Clone(IContainerConfiguration resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new BigQueryConfiguration(resourceConfiguration));
    }

    /// <inheritdoc />
    protected override SpannerBuilder Merge(BigQueryConfiguration oldValue, BigQueryConfiguration newValue)
    {
        return new SpannerBuilder(new BigQueryConfiguration(oldValue, newValue));
    }
}