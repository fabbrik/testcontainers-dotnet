namespace Testcontainers.Spanner;

public sealed class SpannerContainerTest : IAsyncLifetime
{
    private readonly SpannerContainer _spannerContainer = new SpannerBuilder().Build();

    public Task InitializeAsync()
    {
        return _spannerContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _spannerContainer.DisposeAsync().AsTask();
    }

    [Fact]
    [Trait(nameof(DockerCli.DockerPlatform), nameof(DockerCli.DockerPlatform.Linux))]
    public async Task IsConnectedReturnsTrue()
    {
        // Given
        var host = _spannerContainer.Hostname;

        var port = _spannerContainer.GetMappedPublicPort(SpannerBuilder.SpannerPorts[0]);

        using var spannerClient = new SpannerClient(host, port, SpannerBuilder.DefaultProjectId);

        // When
        await spannerClient.ConnectAsync(CancellationToken.None)
            .ConfigureAwait(true);

        // Then
        Assert.True(spannerClient.IsConnected);
    }
}