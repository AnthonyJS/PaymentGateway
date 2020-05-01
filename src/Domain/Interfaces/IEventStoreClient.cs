using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace PaymentGateway.Domain.Interfaces
{
  public interface IEventStoreClient
  {
    Task<Result> Write(IDomainEvent domainEvent);
    Task<IEnumerable<string>> ReadResults();
  }
}
