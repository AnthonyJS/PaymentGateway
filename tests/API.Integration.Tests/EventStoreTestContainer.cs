using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace PaymentGateway.API.Integration.Tests
{
  public class EventStoreTestContainer
  {
    private readonly DockerClient _client;
    private const string _imageName = "eventstore/eventstore";
    private const string _imageTag = "latest";
    private const string _containerName = "ajs-eventstore-integration-testing";

    public EventStoreTestContainer()
    {
      _client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
    }
    
    public async Task CreateContainer()
    {
      if (await DoesContainerExist(_containerName))
      {
        return;
      }

      await _client.Images.CreateImageAsync(new ImagesCreateParameters() {FromImage = _imageName, Tag = _imageTag},
        new AuthConfig(),
        new Progress<JSONMessage>());

      var config = new Config() {Hostname = "localhost"};

      var hostConfig = new HostConfig()
      {
        PortBindings = new Dictionary<string, IList<PortBinding>>
        {
          {"1113/tcp", new List<PortBinding> {new PortBinding {HostIP = "127.0.0.1", HostPort = "1113"}}},
          {"2113/tcp", new List<PortBinding> {new PortBinding {HostIP = "127.0.0.1", HostPort = "2113"}}}
        }
      };

      // Create the container
      try
      {
        CreateContainerResponse response = await _client.Containers.CreateContainerAsync(
          new CreateContainerParameters(config)
          {
            Image = _imageName + ":" + _imageTag, Name = _containerName, Tty = false, HostConfig = hostConfig,
          });
      }
      catch (Exception e)
      {
        Console.WriteLine($"Summat went wrong {e.Message}");
      }
    }

    public async Task<bool> StartContainer()
    {
      return await _client.Containers.StartContainerAsync(_containerName, new ContainerStartParameters());
    }

    public async Task DeleteContainer()
    {
      await _client.Containers.RemoveContainerAsync(_containerName, new ContainerRemoveParameters() {Force = true});
    }
    
    private async Task<bool> DoesContainerExist(string label)
    {
      IList<ContainerListResponse> containers = await _client.Containers.ListContainersAsync(
        new ContainersListParameters() {Limit = 10,});

      return containers.Any(c => c.Names.Any(n => n.Contains(label)));
    }
  }
}
