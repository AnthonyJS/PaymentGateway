
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Infrastructure.Persistence.EventStore
{
  public class EventStoreClient : IEventStoreClient
  {
    private readonly ILogger<EventStoreClient>  _logger;
    private readonly IEventStoreConnection _conn;

    public EventStoreClient(ILogger<EventStoreClient> logger)
    {
      _logger = logger;
      
      // TODO: Add env variables to set this value
      _conn = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113"));
      //_conn = EventStoreConnection.Create(new Uri("tcp://admin:changeit@eventstore:1113"));
      
      _conn.ConnectAsync().Wait();
      _logger.LogInformation("Created connection to EventStore");
    }

    public async Task<Result> Write(IDomainEvent domainEvent)
    {
      var streamName = "paymentstream";
      var eventType = domainEvent.GetType().ToString();
      var data = domainEvent.JSON;
      var eventPayload = new EventData(
        Guid.NewGuid(),
        eventType,
        true,
        Encoding.UTF8.GetBytes(data), 
        null);

      try
      {
        WriteResult result = await _conn.AppendToStreamAsync(streamName, ExpectedVersion.Any, eventPayload);
        _logger.LogDebug($"Sent event {eventPayload.EventId.ToString()} to EventStore");
        return Result.Ok();
      }
      catch (Exception e)
      {
        var errorMessage = $"Failed to send event {eventPayload.EventId.ToString()} to EventStore";
        _logger.LogError(errorMessage, e);
        return Result.Failure<Payment>(errorMessage);
      }
    }

    public async Task<IEnumerable<string>> ReadResults()
    {
      var streamName = "paymentstream";
      var readEvents = await _conn.ReadStreamEventsForwardAsync(streamName, 0, 10, true);

      return readEvents.Events.Select(e => Encoding.UTF8.GetString(e.Event.Data));
    }
  }
}

