using System;

namespace PaymentGateway.Domain.SeedWork
{
  public abstract class Entity
  {
    protected Entity(Guid id)
    {
      Id = id;
    }
    
    public Guid Id { get; }
  }
}
