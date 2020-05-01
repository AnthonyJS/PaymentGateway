using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace PaymentGateway.API.Integration.Tests
{
  public class DockerTestContainer
  {
    private DockerClient _client;
    private string ImageName = "eventstore/eventstore";
    private string ImageTag = "latest";
    private string ContainerName = "ajs-eventstore-integration-testing";

    public DockerTestContainer()
    {
      _client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
    }
    
    public async Task CreateContainer()
    {
      if (await DoesContainerExist(ContainerName))
      {
        Console.WriteLine($"Container {ContainerName} exists so deleting it");
        await DeleteContainer();
      }

      await _client.Images.CreateImageAsync(new ImagesCreateParameters() {FromImage = ImageName, Tag = ImageTag},
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
            Image = ImageName + ":" + ImageTag, Name = ContainerName, Tty = false, HostConfig = hostConfig,
          });
      }
      catch (Exception e)
      {
        Console.WriteLine($"Summat went wrong {e.Message}");
      }
    }

    public async Task StartContainer()
    {
      bool result = await _client.Containers.StartContainerAsync(ContainerName, new ContainerStartParameters());
    }

    public async Task DeleteContainer()
    {
      await _client.Containers.RemoveContainerAsync(ContainerName, new ContainerRemoveParameters() {Force = true});
    }
    
    private async Task<bool> DoesContainerExist(string label)
    {
      IList<ContainerListResponse> containers = await _client.Containers.ListContainersAsync(
        new ContainersListParameters() {Limit = 10,});

      return containers.Any(c => c.Names.Any(n => n.Contains(label)));
    }
  }
}
