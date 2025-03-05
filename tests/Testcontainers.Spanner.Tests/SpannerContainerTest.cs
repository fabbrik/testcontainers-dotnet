using System;
using System.ComponentModel;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Google.Cloud.Spanner.Data;

namespace Testcontainers.Spanner;

public sealed class SpannerContainerTest : IAsyncLifetime
{
    private readonly SpannerContainer _spannerContainer = new SpannerBuilder().Build();

    public async Task InitializeAsync()
    {
        await _spannerContainer.StartAsync();

        var initContainer = new ContainerBuilder()
               .WithImage("gcr.io/google.com/cloudsdktool/cloud-sdk:slim")
                .WithCommand("bash", "-c", 
                @$"gcloud config configurations create emulator --universe-domain spanner &&
                gcloud config set auth/disable_credentials true &&
                gcloud config set project test-project &&
                gcloud config set api_endpoint_overrides/spanner http://localhost:9020/)
                gcloud spanner instance create test-instance --config=emulator-config --description=""Test Instance"" --nodes=1 &&
                gcloud spanner database create test-database --instance=test-instance --ddl='CREATE TABLE TestTable (Id INT64, Name STRING(MAX)) PRIMARY KEY (Id)'")
                .Build();
        
        await initContainer.StartAsync();
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

        // When
        connection.CreateSelectCommand("SELECT * FROM TestTable").ExecuteReader();
    }

    [Fact]
    [Trait(nameof(DockerCli.DockerPlatform), nameof(DockerCli.DockerPlatform.Linux))]
    public void CheckContainer()
    {
        Assert.True(_spannerContainer.State == TestcontainersStates.Running);
        Assert.True(_spannerContainer.Image.FullName == "gcr.io/cloud-spanner-emulator/emulator:latest");
    }

}