using System;

namespace PaymentGateway.Domain.SeedWork
{
  public abstract class Entity
  {
    public Guid Id { get; private set; }
    
    protected Entity(Guid id)
    {
      Id = id;
    }
  }
}
