namespace Testcontainers.Spanner;

/// <inheritdoc cref="ContainerBuilder{TBuilderEntity, TContainerEntity, TConfigurationEntity}" />
[PublicAPI]
public sealed class SpannerBuilder : ContainerBuilder<SpannerBuilder, SpannerContainer, SpannerConfiguration>
{
    public const string SpannerImage = "gcr.io/cloud-spanner-emulator/emulator:latest";

    public  static ushort[] SpannerPorts = new ushort[]{ 9010, 9020 };

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
    public override SpannerContainer Build()
    {
        Validate();
        return new SpannerContainer(DockerResourceConfiguration);
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
