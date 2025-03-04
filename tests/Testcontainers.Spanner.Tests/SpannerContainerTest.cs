using System;
using Google.Cloud.Spanner.Data;

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
        var connectionString = $"Data Source=projects/{SpannerBuilder.DefaultProjectId}/instances/test-instance/databases/test-database;EmulatorDetection=EmulatorOnly";
        
        Environment.SetEnvironmentVariable("SPANNER_EMULATOR_HOST", "localhost:9010");
        
        using var connection = new SpannerConnection(connectionString);

        // Then
        await connection.OpenAsync();
    }
}